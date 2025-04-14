//  Función para mostrar notificaciones toast con SweetAlert2
function showNotification(message, type) {

    Swal.fire({
        toast: true,
        position: "top",
        icon: type,
        title: type === "success" ? "Ok!" : type === "warning" ? "Atencion!" : type === "question" ? "Atencion!" : type === "info" ? "Informacion!" : "Error!",
        text: message,
        color: "white",
        background: "#292b2c",
        showConfirmButton: false,
        timer: type === "success" ? 1500 : type === "warning" ? 5000 : 5000,
        timerProgressBar: true,
    });

}


// Función para mostrar notificaciones con SweetAlert2 (sin toast)
function showNotificationBigWithLoader(message, type) {

    Swal.fire({
        icon: type,
        //posicion en la parte superior derecha
        position: "center",
        title: type === "success" ? "Ok!" : type === "warning" ? "Atencion!" : type === "question" ? "Atencion!" : type === "info" ? "Informacion!" : "Error!",
        text: message,
        color: "white",
        background: "#292b2c",
        showConfirmButton: false,
       
        allowOutsideClick: false,
        allowEscapeKey: false,
        didOpen: () => {
            Swal.showLoading();
           
        },
    });

}


