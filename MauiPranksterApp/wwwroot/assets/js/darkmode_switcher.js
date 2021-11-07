/*=================================

PranksterApp Dark Mode Switcher (Vanilla JS)

===================================*/

window.darkMode = (function () {
    return {
        initialize: function () {

            /* Deprecated. For better visual experience, now is done directly on Blazor
             * Check Shared/Layouts/MainLayout.Razor
             */
            addDark();

            // Enable switcher
            listenSwitch();

        },
        refresh: function () {
            addDark();
        },
        initializeFromBlazor: function () {

            // Just enable switcher
            listenSwitch();

            if (localStorage.mode) {
                var mode = document.querySelector('#enableMode');
                mode.checked = true;
            }
            else {
                if (window.location.pathname == "/home") {
                    var hiddentextField = document.querySelector('#hiddenTextField');
                    hiddentextField.classList.add('hiddenTextField-white');
                }
            }
        },
        get: function () {
            var status = Boolean(localStorage.getItem("mode"));
            return status;
        }
    };
})();

function listenSwitch() {

    var mode = document.querySelector('#enableMode');

    mode.addEventListener("input", () => {
        if (mode.checked) {
            localStorage.setItem("mode", true);
            addDark();
        } else {
            localStorage.removeItem("mode");
            addDark(true);
        }

    });

} 

function addDark(isRemoving) {

    // query elements
    var mode = document.querySelector('#enableMode');
    var add = document.querySelectorAll('.dz-mode');
    if (window.location.pathname == "/home") {
        var hiddentextField = document.querySelector('#hiddenTextField');
    }

    if (localStorage.mode) {
        add.forEach(d => {
            d.classList.add('dz-dark-mode');
        });

        if (window.location.pathname == "/home") {
            hiddentextField.classList.remove('hiddenTextField-white');
        }

        // Set switch to true
        mode.checked = true;
    }

    if (isRemoving) {
        add.forEach(d => {
            d.classList.remove('dz-dark-mode');
        });

        if (window.location.pathname == "/home") {
            hiddentextField.classList.add('hiddenTextField-white');
        }
    }

    setOverscrollBackground();

}

// Set container color for scroll bouncing (iOS)

function setOverscrollBackground() {

    var dom = document.getElementsByTagName("BODY")[0];
    var attribute = document.createAttribute("style");

    if (!darkMode.get()) {
        attribute.value = "background: var(--white);";
    }
    else {
        attribute.value = "background: var(--grey70);";
    }

    attribute.value += " overscroll-behavior-x: none";
    dom.setAttributeNode(attribute);
}

async function checkIfExceptionOcurred() {

    while (!(document.getElementById('blazor-error-ui').style.display == "block")) {
        await delay(200);
    }

    document.getElementById("approot").remove();
}

function delay(t) {
    return new Promise(resolve => setTimeout(resolve, t));
}

window.PlayAudio = (elementName) => {
    document.getElementById(elementName).play();
}

window.PauseAudio = (elementName) => {
    document.getElementById(elementName).pause();
}

$(function () {
    "use strict";

    setOverscrollBackground();

    // fire and forget
    checkIfExceptionOcurred();
});