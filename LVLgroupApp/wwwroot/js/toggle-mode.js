/*!
 * Color mode toggler for Bootstrap's docs (https://getbootstrap.com/)
 * Copyright 2011-2023 The Bootstrap Authors
 * Licensed under the Creative Commons Attribution 3.0 Unported License.
 */

(() => {
    'use strict'

    const getStoredTheme = () => localStorage.getItem('theme')
    const setStoredTheme = theme => localStorage.setItem('theme', theme)

    const getPreferredTheme = () => {
        const storedTheme = getStoredTheme()
        if (storedTheme) {
            return storedTheme
        }

        return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light'
    }

    //const setThemeDataTables = theme => {

    //    const elementsHeader = document.querySelectorAll('table.dataTable thead tr').forEach(element => {
    //        if (theme === 'dark') {
    //            element.style.backgroundColor = "orange";
    //            element.style.color = "black";
    //        }
    //        if (theme === 'light') {
    //            element.style.backgroundColor = "greenyellow";
    //            element.style.color = "black";
    //        }
    //        console.log("setThemeDataTables: " + theme);
    //    })
    //}

    //const setThemeSelect2 = theme => {

    //    const selectArrowLightUrl = `url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23adb5bd' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e")`;
    //    const selectArrowDarkUrl = `url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 16 16'%3e%3cpath fill='none' stroke='%23343a40' stroke-linecap='round' stroke-linejoin='round' stroke-width='2' d='M2 5l6 6 6-6'/%3e%3c/svg%3e")`;

    //    const elementsSelect2 = document.querySelectorAll('.select2-selection--single').forEach(element => {
    //        if (theme === 'dark') {
    //            //element.classList.remove('select-arrow-dark');
    //            //element.classList.add('select-arrow-light');
    //            element.style.backgroundImage = selectArrowLightUrl;
    //        }
    //        if (theme === 'light') {
    //            //element.classList.remove('select-arrow-light');
    //            //element.classList.add('select-arrow-dark');
    //            element.style.backgroundImage = selectArrowDarkUrl;
    //        }
    //        console.log("setThemeSelect2: " + theme);
    //    })
    //}

    const setThemeImages = theme => {
        if (theme === 'dark') {
            $("#header-logo").attr("src", "/images/logo-lvl-light.png");
            $("#brand-logo").attr("src", "/images/logo-transparent-light.png");
            $("#home-logo").attr("src", "/images/logo-transparent-light.png");
            $("#login-logo").attr("src", "/images/bg-login-light-portal.png");
            $("#register-logo").attr("src", "/images/bg-register-light-portal.png");
            $("#shoeicon").attr("src", "/images/Icons/shoe-icon-light.png");
        }
        if (theme === 'light') {
            $("#header-logo").attr("src", "/images/logo-lvl-dark.png");
            $("#brand-logo").attr("src", "/images/logo-transparent-dark.png");
            $("#home-logo").attr("src", "/images/logo-transparent-dark.png");
            $("#login-logo").attr("src", "/images/bg-login-dark-portal.png");
            $("#register-logo").attr("src", "/images/bg-register-dark-portal.png");
            $("#shoeicon").attr("src", "/images/Icons/shoe-icon-dark.png");
        }
    }

    const setTheme = theme => {
        if (theme === 'auto' && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            document.documentElement.setAttribute('data-bs-theme', 'dark');           
        } else {
            document.documentElement.setAttribute('data-bs-theme', theme)
        }
        setThemeImages(theme);
        //setThemeDataTables(theme);
        //setThemeSelect2(theme);
        console.log("Theme toggle to: " + theme);
    }

    setTheme(getPreferredTheme())

    const showActiveTheme = (theme, focus = false) => {
        const themeSwitcher = document.querySelector('#bd-theme')

        if (!themeSwitcher) {
            return
        }

        const themeSwitcherText = document.querySelector('#bd-theme-text')
        const activeThemeIcon = document.querySelector('.theme-icon-active use')
        const btnToActive = document.querySelector(`[data-bs-theme-value="${theme}"]`)
        const svgOfActiveBtn = btnToActive.querySelector('svg use').getAttribute('href')

        document.querySelectorAll('[data-bs-theme-value]').forEach(element => {
            element.classList.remove('active')
            element.setAttribute('aria-pressed', 'false')
        })

        btnToActive.classList.add('active')
        btnToActive.setAttribute('aria-pressed', 'true')
        activeThemeIcon.setAttribute('href', svgOfActiveBtn)
        const themeSwitcherLabel = `${themeSwitcherText.textContent} (${btnToActive.dataset.bsThemeValue})`
        themeSwitcher.setAttribute('aria-label', themeSwitcherLabel)

        if (focus) {
            themeSwitcher.focus()
        }
    }

    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', () => {
        const storedTheme = getStoredTheme()
        if (storedTheme !== 'light' && storedTheme !== 'dark') {
            setTheme(getPreferredTheme())
        }
    })

    window.addEventListener('DOMContentLoaded', () => {
        showActiveTheme(getPreferredTheme())

        document.querySelectorAll('[data-bs-theme-value]').forEach(toggle => {
                toggle.addEventListener('click', () => {
                    const theme = toggle.getAttribute('data-bs-theme-value')
                    setStoredTheme(theme)
                    setTheme(theme)
                    showActiveTheme(theme, true)
                })
        })
    })
})()