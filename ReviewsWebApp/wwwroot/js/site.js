﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var focusInput = document.getElementById("focusInput");
if (focusInput) {
    window.onload = function () {
        focusInput.focus();
        focusInput.setSelectionRange(focusInput.value.length, focusInput.value.length)
    }
}

function addImageClickEvents() {
    const imgs = document.querySelectorAll(".img-clickable");
    const fullPage = document.querySelector("#fullpage");
    imgs.forEach(img => {
        img.addEventListener("click", function () {
            fullPage.style.backgroundImage = `url(${img.src})`;
            fullPage.style.display = "block";
        })
    })
}

addImageClickEvents();


// code for markdown display
const converter = new showdown.Converter();
const markdownInput = document.getElementById("markdownInput");
const markdownPreview = document.getElementById("markdownPreview");

function updatePreview() {
    const markdownText = markdownInput.value;
    const html = converter.makeHtml(markdownText);
    markdownPreview.innerHTML = html;
}

function transformMarkdownText() {
    const markdownElems = document.querySelectorAll(".markdown");
    markdownElems.forEach(e => {
        const html = converter.makeHtml(e.innerHTML);
        e.innerHTML = html;
    })
}

if (markdownInput !== null) {
    markdownInput.addEventListener("input", updatePreview);
    updatePreview();
}
transformMarkdownText();


// using bootstrap + popper.js tooltips
var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'))
var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) {
    return new bootstrap.Tooltip(tooltipTriggerEl)
})


// rating visualization
const stars = document.querySelectorAll(".ratings > label");
const choiceEl = document.getElementById("choice");
stars.forEach(s => {
    s.addEventListener("click", function () {
        stars.forEach(s => s.removeAttribute("data-clicked"));
        this.setAttribute("data-clicked", "true");
        choiceEl.textContent = this.getAttribute("for");
    })
})
const initialValue = document.querySelector('input[checked="checked"]');
if (initialValue)
    choiceEl.textContent = initialValue.value;