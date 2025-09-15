(function ($window) {

    const MESSAGE_PASSCODE = `{{MESSAGE_PASSCODE}}`;
    const HAS_TITLE_BAR = (`{{HAS_TITLE_BAR}}` === "true");
    const WINDOW_COMMAND_ATTR_NAME = `app-command`;

    const getHostWindow = () => {
        return formium?.nativeObjects?.__hostWindowObject;
    }

    class HostWindow {
        #target() {
            return formium?.nativeObjects?.__hostWindowObject;
        }

        get activated() {
            return this.#target()?.activated ?? false;
        }

        get hasTitleBar() {
            return this.#target()?.hasTitleBar ?? false;
        }

        get windowState() {
            return this.#target()?.windowState ?? "normal";
        }

        get left() {
            return this.#target()?.x ?? 0;
        }

        set left(value) {
            if (typeof value !== 'number') return;

            this.#target().x = value;
        }

        get top() {
            return this.#target()?.y ?? 0;
        }

        set top(value) {
            if (typeof value !== 'number' || !this.#target()) return;
            this.#target().y = value;
        }

        get width() {
            return this.#target()?.width ?? 0;
        }

        set width(value) {
            if (typeof value !== 'number' || !this.#target()) return;

            this.#target().width = value;
        }

        get height() {
            return this.#target()?.height ?? 0;
        }

        set height(value) {
            if (typeof value !== 'number' || !this.#target()) return;

            this.#target().height = value;
        }

        get right() {
            return this.left + this.width;
        }

        set right(value) {
            if (typeof value !== 'number' || !this.#target()) return;

            this.#target().width = this.left + this.width + value;
        }
        get bottom() {
            return this.top + this.height;
        }

        set bottom(value) {
            if (typeof value !== 'number' || !this.#target()) return;

            this.#target().height = this.top + this.height + value;
        }

        constructor() {

        }

        activate() {
            this.#target()?.activate();
        }

        close() {
            this.#target()?.close();
        }

        maximize() {
            this.#target()?.maximize();
        }

        minimize() {
            this.#target()?.minimize();
        }

        restore() {
            this.#target()?.restore();
        }

        fullscreen() {
            this.#target()?.fullscreen();
        }

        toggleFullscreen() {
            this.#target()?.toggleFullscreen();
        }

    }

    Object.defineProperty($window, "hostWindow", {
        configurable: false,
        writable: false,
        enumerable: false,
        value: new HostWindow(),
    });

    function postMessage(args) {
        if (formium?.webview?.postMessage && args.message) {
            args.passcode = MESSAGE_PASSCODE;
            formium.webview.postMessage(args);
        }
        else {
            console.error("[WinFormium] Formium webview not found or message is empty!!!");
        }
    }

    function moveTo(x, y) {
        postMessage({
            message: "FormiumWindowMoveTo",
            x,
            y
        });
    }

    function moveBy(x, y) {
        postMessage({
            message: "FormiumWindowMoveBy",
            x,
            y
        });
    }

    function resizeTo(width, height) {
        postMessage({
            message: "FormiumWindowResizeTo",
            width,
            height
        });
    }

    function resizeBy(width, height) {
        postMessage({
            message: "FormiumWindowResizeBy",
            width,
            height
        });
    }

    $window.moveTo = function (x, y) {
        moveTo(x, y);
    };

    $window.moveBy = function (x, y) {
        moveBy(x, y);
    };

    $window.resizeTo = function (width, height) {
        resizeTo(width, height);
    };

    $window.resizeBy = function (width, height) {
        resizeBy(width, height);
    };


    function RegisterMessageHandler() {

        setInterval(() => {
            const state = getHostWindow()?.activated;

            if (state === null || state === undefined) return;

            const htmlEl = document.querySelector("html");

            if (state) {
                htmlEl?.classList.add("window--activated");
                htmlEl?.classList.remove("window--deactivated");
            }
            else {
                htmlEl?.classList.remove("window--activated");
                htmlEl?.classList.add("window--deactivated");
            }

        }, 200);

        function raiseHostWindowEvent(eventName, detail) {
            window.dispatchEvent(new CustomEvent(eventName, {
                detail: detail,
            }));
        }

        function OnNotifyWindowActivated(data) {
            const { state } = data;


            const htmlEl = document.querySelector("html");

            if (state) {
                raiseHostWindowEvent("windowactivated", {});
            }
            else {
                raiseHostWindowEvent("windowdeactivate", {});
            }

            if (state) {
                htmlEl?.classList.add("window--activated");
                htmlEl?.classList.remove("window--deactivated");
            }
            else {
                htmlEl?.classList.remove("window--activated");
                htmlEl?.classList.add("window--deactivated");
            }

        }

        function OnNotifyWindowStateChange(data) {
            const { state } = data;
            if (!state) return;

            raiseHostWindowEvent("windowstatechange", { state });

            const htmlEl = document.querySelector("html");

            htmlEl?.classList.remove("window--maximized", "window--minimized", "window--fullscreen");

            switch (state) {
                case "maximized":
                    htmlEl?.classList.add("window--maximized");
                    break;
                case "minimized":
                    htmlEl?.classList.add("window--minimized");
                    break;
                case "fullscreen":
                    htmlEl?.classList.add("window--fullscreen");
                    break;
            }

        }

        function OnNotifyWindowResize(data) {
            const { x, y, width, height } = data;
            raiseHostWindowEvent("windowresize", { x, y, width, height });
        }

        function OnNotifyWindowMove(data) {
            const { x, y, screenX, screenY } = data;
            raiseHostWindowEvent("windowmove", { x, y, screenX, screenY });
        }

        const htmlEl = document.querySelector("html");

        if (formium?.webview?.addEventListener) {

            formium.webview.addEventListener("message", (e) => {
                const { passcode, message } = e.data;
                if (passcode !== MESSAGE_PASSCODE) {
                    return;
                }

                switch (message) {
                    case "FormiumNotifyWindowStateChange":
                        OnNotifyWindowStateChange(e.data);
                        break;
                    case "FormiumNotifyWindowResize":
                        OnNotifyWindowResize(e.data);
                        break;
                    case "FormiumNotifyWindowMove":
                        OnNotifyWindowMove(e.data);
                        break;
                    case "FormiumNotifyWindowActivated":
                        OnNotifyWindowActivated(e.data);
                        break;
                }
            });

            OnNotifyWindowActivated({
                passcode: MESSAGE_PASSCODE,
                message: "FormiumNotifyWindowActivated",
                state: true
            });

            if (HAS_TITLE_BAR) {
                htmlEl?.classList.add("window__titlbar--shown");
                htmlEl?.classList.remove("window__titlbar--hidden");

            }
            else {
                htmlEl?.classList.add("window__titlbar--hidden");
                htmlEl?.classList.remove("window__titlbar--shown");
            }
        }

        window.addEventListener("click", (e) => {
            const button = e.button;

            if (button === 0) {
                let srcElement = e.target;

                while (srcElement && !srcElement.hasAttribute(WINDOW_COMMAND_ATTR_NAME)) {
                    srcElement = srcElement.parentElement;
                }

                if (srcElement) {
                    const command = srcElement.getAttribute(WINDOW_COMMAND_ATTR_NAME)?.toLowerCase();
                    postMessage({
                        passcode: MESSAGE_PASSCODE,
                        message: "FormiumWindowCommand",
                        command: command,
                    });
                }
            }
        });
    }



    if (document.readyState === "loading") {
        $window.addEventListener("DOMContentLoaded", () => {
            RegisterMessageHandler();
        });
    }
    else {
        RegisterMessageHandler();
    }



})(window);
