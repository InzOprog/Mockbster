// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function unset() {
    Cookies.remove('loggedUser')
    document.getElementById("upage").style.display = "none"
}

function hideCart() {
    let v = document.getElementById('cart')
    v.style.display = "none"
}

$(document).ready(function () {
    if (Cookies.get('loggedUser') == undefined) {
        let v = document.getElementById('logout')
        v.style.display = "none"
    } else {
        let v = document.getElementById('logout')
        v.style.display = "inline"
    }
});