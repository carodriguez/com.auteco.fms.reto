//Al cargar la pagina
$(document).ready(function () {

    // llamar a la función para cargar el datatable
    loadDatatableClientes();


});

//Declaracion de variables globales
var datTableClientes;
var datTableClientesEstaInicializada = false;

//Formato de fecha
const formatDate = "DD/MM/YYYY hh:mm A";
const formatDateShort = "DD/MM/YYYY";

// Función para cargar el datatable de la bandeja de procesos abiertos
function loadDatatableClientes() {


    //Verifico el estado de la tabla
    if (datTableClientesEstaInicializada) {

        //Destruyo la tabla
        datTableClientes.DataTable().destroy();
    }


    //Carga la tabla
    datTableClientes = $('#tblClientes').DataTable({
        //Ordenado por la primera columna
        //"order": [[4, "asc"]],
        "destroy": true,

        "info": true,
        "processing": true,
        "responsive": true,
        "searching": true, // Deshabilita la búsqueda
        "paging": false,
        "search": {
            "regex": true,
            "caseInsensitive": false,
        },// Deshabilita la paginación
        'dom':
            "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'<'float-md-right ml-2'B>f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        "ajax": {
            "url": "/GestionClientes/GetClientesActivos",
            "dataSrc": function (json) {
                if (json.success) {
                    return json.data;
                } else if (json.warning) {
                    showNotification(json.message, 'warning');
                    return [];
                } else {
                    showNotification(json.message, 'error');
                    return [];
                }
            },
            "type": "GET",
            "datatype": "json",
            "error": function (xhr, textStatus, errorThrown) {

                //Muestra el mensaje de error
                showNotification("Ocurrio un error procesando la solicitud", 'error');

            }
        },
        'buttons': [{
            'text': '<i class="fa-solid fa-rotate" aria-hidden="true"></i> Actualizar',
            'action': function (e, dt, node) {

                // Recarga el DataTable
                datTableClientes.ajax.reload();

            },
            'className': 'btn btn-light',
            'attr': {
                'title': 'Recargar datos',
                'data-bs-toggle': 'tooltip',
                'data-bs-placement': 'bottom'
            }
        }],
        "columns": [

            { "data": "publicoId", "className": "text-center" },

            { "data": "nombre", "className": "text-center" },

            { "data": "tipoClienteNombre", "className": "text-center" },


            {
                "data": "publicoId", "width": "10%", "orderable": false, "className": "text-center",
                //Recibir todos los parametros desde del data table

                "render": function (data, type, row, meta) {
                    return `<div class=" text-center">
                                <button type="button" class="btn btn-secondary btn-sm dropdown-toggle" style="text-decoration: none"
                                data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >
                                     <i class="fas fa-cog"></i>
                                </button>
                                <div class="dropdown-menu shadow animated--grow-in justify-content-center">
                                           <a onclick=abrirModalActualizarConcepto(${row.publicoId}) class="dropdown-item">
                                                 <i class='fas fa-edit'></i> Editar
                                           </a>
                                           <a onclick="eliminarCliente('${row.publicoId}','${row.nombre}')" class="dropdown-item">
                                                 <i class="fas fa-trash-alt text-danger"></i> Inactivar
                                           </a>
                                </div>
                           `;

                }
            }


        ],
        "language": {
            "url": '/lib/DataTables/language.json'
        }

    });

    // Previene el mensaje de error del data table cuando el json no tiene datos
    $.fn.dataTable.ext.errMode = 'none';



    datTableClientesEstaInicializada = true;

    // Auto ajusto las columnas del data table
    datTableClientes.columns.adjust().draw();

    //muestra los tools tips en el data table
    datTableClientes.on('draw', function () {
        $('[data-bs-toggle="tooltip"]').tooltip();
    });

}

// Funcion para eliminar un cliente
function eliminarCliente(publicoId, nombre) {

    //Mensaje de Sweet Alert para preguntar si desea inactivar el concepto

    Swal.fire({
        title: `¿Desea inactivar el cliente ${nombre}?`,
        text: `Ya no podrá devolver los cambios!`,
        icon: "question",
        showCancelButton: true,
        color: "white",
        background: "#292b2c",
        confirmButtonColor: '#d9534f',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, Inactivar!',
        cancelButtonText: 'Cancelar',
        allowOutsideClick: false,
    }).then((result) => {

        if (result.isConfirmed) {

            debugger;
            //Url del la api con los parametros
            let url = `/GestionClientes/DeleteCliente?publicoId=${publicoId}`;

            //Llamo a la api para guardar los parametros
            apiModule.postFromQuery(url)
                .then(function (response) {

                    if (response.success) {


                        showNotification(response.message, 'success');


                        // hago un delay de 2 segundos para mostrar el mensaje
                        setTimeout(function () {

                            //Recarga la tabla
                            datTableClientes.ajax.reload();

                        }, 2000);


                    }

                    if (response.warning) {

                        //Muestra el mensaje de alerta
                        showNotification(response.message, 'warning');
                    }

                    if (response.error) {

                        //Muestra el mensaje de error
                        showNotification(response.message, 'error');
                    }

                })
                .catch(function (error) {
                    showNotification('Error al procesar la solicitud', 'error');
                });
        }
    })



}


