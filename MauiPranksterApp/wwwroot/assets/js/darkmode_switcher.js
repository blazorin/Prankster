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
                var hiddentextField = document.querySelector('#hiddenTextField');
                hiddentextField.classList.add('hiddenTextField-white');
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
    var hiddentextField = document.querySelector('#hiddenTextField');

    if (localStorage.mode) {
        add.forEach(d => {
            d.classList.add('dz-dark-mode');
        });

        hiddentextField.classList.remove('hiddenTextField-white');

        // Set switch to true
        mode.checked = true;
    }

    if (isRemoving) {
        add.forEach(d => {
            d.classList.remove('dz-dark-mode');
        });

        hiddentextField.classList.add('hiddenTextField-white');
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

$(function () {
    "use strict";

    setOverscrollBackground();
});