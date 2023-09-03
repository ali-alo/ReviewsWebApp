// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function addImageClickEvents() {
    console.log("addImageClickEvents called");
    const imgs = document.querySelectorAll(".img-clickable");
    const fullPage = document.querySelector("#fullpage");
    imgs.forEach(img => {
        img.addEventListener("click", function () {
            console.log("Image clicked");
            console.log(`url(${img.src})`);
            fullPage.style.backgroundImage = `url(${img.src})`;
            fullPage.style.display = "block";
        })
    })
}


addImageClickEvents();
