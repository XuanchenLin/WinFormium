var chrome;
var formium;
var __winformium;

(function ($window) {
    const WEB_VIEW_LISTENERS = {};
    const NATIVE_BOUND_OBJECTS_COLLECTION = new Map();

    chrome = $window.chrome || {};

    function GetNativeObjectByName(name) {
        native function GetNativeObject();
        return GetNativeObject(name);
    }

    class WinFormium {
        emitWebMessage(type, asString, data) {
            try {

                let retval = data;
                if (asString === false) {
                    try {
                        retval = JSON.parse(data);
                    }
                    catch {

                    }
                }
                const event = new Event(type);
                event.data = retval;
                event.source = this;
                formium?.webview?.dispatchEvent(event);
            }
            catch (e) {
                console.error("Error in emitWebMessage:", e);
            }
        }
    }

    __winformium = new WinFormium();

    class NativeBoundObjectRepository {

        constructor() {

            return new Proxy(this, {
                get(target, prop) {

                    if (Reflect.get(target, prop)) {
                        return Reflect.get(target, prop);
                    }

                    const hostObj = GetNativeObjectByName(prop);

                    if (hostObj) {
                        const objProxy = new NativeBoundObject(hostObj);

                        Reflect.set(target,prop, objProxy);

                        return objProxy;
                    }
                    else {
                        throw new Error(`[WinFormium] The required host object "${prop}" is not registered.`);
                    }

                },
                set(target, prop, value) {
                    throw new Error("[WinFormium] The repository can not be set on browser side.");
                },
                has(target, prop) {
                    if (Reflect.get(target, prop)) {
                        return true;
                    }

                    const hostObj = GetNativeObjectByName(prop);

                    if (hostObj) {
                        const objProxy = new NativeBoundObject(hostObj);
                        Reflect.set(target, prop, objProxy);
                        return true;
                    }
                    return false;
                }
            });
        }

    }

    class NativeBoundObject {

        constructor(target) {

            Object.assign(this, target);

            return new Proxy(this, {
                get(target, prop) {

                    if (prop === "__objectId" || Reflect.has(target, prop)) {
                        return Reflect.get(target, prop);
                    }
                                        
                    native function GetNativeObjectProperty();

                    const result = GetNativeObjectProperty(JSON.stringify({
                        objectId: target.__objectId,
                        property: prop
                    }));

                    const { type, data } = result;

                    switch (type) {
                        case "value":
                            return data;
                        case "object":
                            {
                                const obj = new NativeBoundObject({ __objectId: JSON.parse(data) });
                                Reflect.defineProperty(target, prop, {
                                    value: obj,
                                });

                                return obj;
                            }
                            break;
                        case "function":
                            {
                                const objectId = target.__objectId;
                                const func = new Proxy(function () { }, {
                                    apply: (target, thisArg, args) => {
                                        native function CallNativeObjectMethod();
                                        return CallNativeObjectMethod(objectId, prop, JSON.stringify(args));
                                    }
                                });

                                Reflect.defineProperty(target, prop, {
                                    value: func,
                                });

                                return func;
                            }
                            break;
                    }
                },
                set(target, prop, value) {
                    if (prop === "__objectId") {
                        throw new Error("[WinFormium] The __objectId property can not be set on any condition.");
                    }

                    native function SetNativeObjectProperty();

                    SetNativeObjectProperty(target.__objectId, prop, JSON.stringify(value));

                    return Reflect.set({}, prop, value);
                }
            });
        }
    }

    class WebView {
        addEventListener(type, callback) {
            const listeners =
                WEB_VIEW_LISTENERS[type] || (WEB_VIEW_LISTENERS[type] = []);

            if (listeners.indexOf(callback) === -1) {
                listeners.push(callback);
            }
        };

        removeEventListener(event, callback) {
            const listeners = WEB_VIEW_LISTENERS[event];
            if (listeners) {
                const index = listeners.indexOf(callback);
                if (index !== -1) {
                    listeners.splice(index, 1);
                }
            }
        };

        dispatchEvent(event) {
            const listeners = WEB_VIEW_LISTENERS[event.type];
            if (listeners) {
                const array = listeners.slice(0);
                for (let i = 0; i < array.length; i++) {
                    const listener = array[i];
                    if (typeof listener === "function") {
                        listener.call(this, event);
                    } else if (typeof listener.handleEvent === "function") {
                        listener.handleEvent(event);
                    }
                }
            }
        };

        postMessage(message) {
            const data = JSON.stringify(message);
            native function PostMessage();
            return PostMessage(data);
        };
    }

    class Formium {
        version = {
            get WinFormium() {
                native function GetWinFormiumVersion();
                return GetWinFormiumVersion();
            },
            get Chromium() {
                native function GetChromiumVersion();
                return GetChromiumVersion();
            }
        }

        get cultureName() {
            native function GetCulture();
            return GetCulture();
        }

        webview = new WebView();

        nativeObjects = new NativeBoundObjectRepository();
    }

    formium = new Formium();
})(this);
