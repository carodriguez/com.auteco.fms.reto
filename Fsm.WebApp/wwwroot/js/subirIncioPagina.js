//Boton subir
var btnSubir = document.getElementById("btnSubirInicio");
window.onscroll = function () { scrollFunction() };
function scrollFunction() {
    if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
        btnSubir.style.display = "block";
    } else {
        btnSubir.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
function subirInicio() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}