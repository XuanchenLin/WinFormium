(function () {
    const WINFORMIUM_VERSION_INFO = `{{WINFORMIUM_VERSION_INFO}}`;

    function showVersionLog() {
        const badgeLeftCss = "font-size:0.8em;background: #35495e;padding: 4px;border-radius: 5px 0 0 5px;color: #fff;font-weight:bold;";
        const badgeRightCss = "font-size:0.8em;background: #b8416e;padding: 4px;border-radius: 0 5px 5px 0;color: #fff;text-shadow: #000 0 0 2px;";

        console.log(`==== Welcome to the%c WinFormium %cProject ====`, "color: #fff;font-weight:bold;font-size:1.1em;text-shadow: #333 1px 1px 3px;", "");


        console.log(WINFORMIUM_VERSION_INFO, badgeLeftCss, badgeRightCss, "", badgeLeftCss, badgeRightCss, "", badgeLeftCss, badgeRightCss, "");

        console.log(`Copyrights (C) ${new Date().getFullYear()} Xuanchen Lin all rights reserved.`);

    }


    if (document.readyState === "loading") {
        window.addEventListener("DOMContentLoaded", () => {
            showVersionLog();
        });
    }
    else {
        showVersionLog();
    }

})();