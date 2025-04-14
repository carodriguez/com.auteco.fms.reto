// Al cargar la página
$(document).ready(function () {
    // Llamar a la función para cargar el DataTable
    loadDatatableOrdenes();
});

// Declaración de variables globales
var datTableOrdenes;
var datTableOrdenesEstaInicializada = false;

// Función para cargar el DataTable de órdenes de servicio
function loadDatatableOrdenes() {
    // Verificar el estado de la tabla
    if (datTableOrdenesEstaInicializada) {
        // Destruir la tabla si ya está inicializada
        datTableOrdenes.DataTable().destroy();
    }

    // Cargar la tabla
    datTableOrdenes = $('#tblOrdenes').DataTable({
        "destroy": true,
        "info": true,
        "processing": true,
        "responsive": true,
        "searching": true,
        "paging": true,
        "pageLength": 50,
        "lengthMenu": [
            [10, 15, 20, 50, 100, -1],
            ['10', '15', '20', '50', '100', 'Todos']
        ],
        "search": {
            "regex": true,
            "caseInsensitive": false,
        },
        // Modified DOM to include searchPanes container
        'dom':
            "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'<'float-md-right ml-2'B>f>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
        // Enhanced searchPanes configuration
        "searchPanes": {
            "cascadePanes": true,
            "viewTotal": true,
            "layout": 'columns-3',
            //"initCollapsed": true  // Set to true if you want panes collapsed at start
        },
        "columnDefs": [
            {
                "searchPanes": {
                    "show": true
                },
                "targets": [1, 2]
            },
            {
                "searchPanes": {
                    "show": false
                },
                "targets": [0, 3, 4, 5]
            },
        ],
        'buttons': [
            {
                'text': '<i class="fa-solid fa-rotate" aria-hidden="true"></i> Actualizar',
                'action': function (e, dt, node) {
                    datTableOrdenes.ajax.reload();
                },
                'className': 'btn btn-light',
                'attr': {
                    'title': 'Recargar datos',
                    'data-bs-toggle': 'tooltip',
                    'data-bs-placement': 'bottom'
                }
            },
            {
                // Add button to toggle searchPanes
                'text': '<i class="fas fa-filter"></i> Filtros Avanzados',
                'action': function (e, dt, node, config) {
                    toggleSearchPanes();
                },
                'className': 'btn btn-primary',
                'attr': {
                    'title': 'Mostrar/Ocultar Filtros Avanzados',
                    'data-bs-toggle': 'tooltip',
                    'data-bs-placement': 'bottom',
                    'id': 'toggleSearchPanesBtn'
                }
            }
        ],
        "ajax": {
            "url": "/GestionOrdenesServicio/GetOrdenesDeServicio",
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
                // Mostrar mensaje de error
                showNotification("Ocurrió un error procesando la solicitud", 'error');
            }
        },
      
       
        "columns": [
            { "data": "publicoId", "className": "text-center", "title": "Documento" },
            { "data": "cliente.nombre", "className": "text-center", "title": "Cliente" },
            { "data": "estado", "className": "text-center", "title": "Estado" },
            { "data": "fechaCreacion", "className": "text-center", "title": "Fecha de Creación", "render": formatDate },
            { "data": "fechaDeAtencionEstimada", "className": "text-center", "title": "Fecha Atención Est", "render": formatDateShort },
            {
                "data": "id", "className": "text-center", "orderable": false, "title": "Acciones",
                "render": function (data, type, row, meta) {
                    return `<div class="text-center">
                                <button type="button" class="btn btn-secondary btn-sm dropdown-toggle" style="text-decoration: none"
                                data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                                     <i class="fas fa-cog"></i>
                                </button>
                                <div class="dropdown-menu shadow animated--grow-in justify-content-center">
                                    <a onclick="verDetallesOrden('${row.id}')" class="dropdown-item">
                                        <i class='fas fa-eye'></i> Ver Detalles
                                    </a>
                                    <a onclick="cambiarEstadoOrden('${row.id}')" class="dropdown-item">
                                        <i class="fas fa-edit text-primary"></i> Cambiar Estado
                                    </a>
                                    <a onclick="eliminarOrden('${row.id}')" class="dropdown-item">
                                        <i class="fas fa-trash-alt text-danger"></i> Eliminar
                                    </a>
                                </div>
                            </div>`;
                }
            }
        ],
        "language": {
            "url": '/lib/DataTables/language.json'
        }
    });

    // Previene el mensaje de error del DataTable cuando el JSON no tiene datos
    $.fn.dataTable.ext.errMode = 'none';

    datTableOrdenesEstaInicializada = true;

    // Ajustar las columnas del DataTable
    datTableOrdenes.columns.adjust().draw();

    // Mostrar tooltips en el DataTable
    datTableOrdenes.on('draw', function () {
        $('[data-bs-toggle="tooltip"]').tooltip();
    });
}

// Función para formatear fechas
function formatDate(data, type, row) {
    return moment(data).format("DD/MM/YYYY hh:mm A");
}

function formatDateShort(data, type, row) {
    return moment(data).format("DD/MM/YYYY");
}

// Función para eliminar una orden
function eliminarOrden(id) {
    Swal.fire({
        title: `¿Desea eliminar la orden con ID ${id}?`,
        text: `Esta acción no se puede deshacer.`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: '#d9534f',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar',
        allowOutsideClick: false,
    }).then((result) => {
        if (result.isConfirmed) {
            // Llamar a la API para eliminar la orden
            let url = `/GestionOrdenesServicio/DeleteOrden?id=${id}`;
            apiModule.postFromQuery(url)
                .then(function (response) {
                    if (response.success) {
                        showNotification(response.message, 'success');
                        datTableOrdenes.ajax.reload();
                    } else {
                        showNotification(response.message, response.warning ? 'warning' : 'error');
                    }
                })
                .catch(function () {
                    showNotification('Error al procesar la solicitud', 'error');
                });
        }
    });
}

// Función para cambiar el estado de una orden
function cambiarEstadoOrden(id) {
    // Obtener los datos de la orden para mostrar estado actual
    let ordenActual = datTableOrdenes.rows().data().toArray().find(row => row.id === id);

    if (!ordenActual) {
        showNotification("No se pudo encontrar la información de la orden", 'error');
        return;
    }

    // Definir los estados disponibles para el cambio
    const estados = [
        { id: 0, nombre: "Pendiente" },
        { id: 1, nombre: "En Proceso" },
        { id: 2, nombre: "Finalizada" },
        { id: 3, nombre: "Cancelada" }
    ];

    // Filtrar estados según las reglas de transición
    // (las reglas se manejan en el backend, pero mejoramos UX)
    let estadosPermitidos = [];
    const estadoActualId = ordenActual.estadoId;

    if (estadoActualId === 0) { // Pendiente
        // Desde pendiente se puede pasar a cualquier otro estado
        estadosPermitidos = estados.filter(e => e.id !== estadoActualId);
    } else if (estadoActualId === 1) { // EnProceso
        // De EnProceso solo se puede finalizar o cancelar
        estadosPermitidos = estados.filter(e => e.id === 2 || e.id === 3);
    } else {
        // Estados finalizados o cancelados no se pueden cambiar
        estadosPermitidos = [];
    }

    if (estadosPermitidos.length === 0) {
        showNotification(`No es posible cambiar el estado de una orden ${ordenActual.estado}`, 'warning');
        return;
    }

    // Construir opciones para el select
    let opcionesHtml = estadosPermitidos.map(e =>
        `<option value="${e.id}">${e.nombre}</option>`).join('');

    // Modal para seleccionar el nuevo estado
    Swal.fire({
        title: 'Cambiar Estado de la Orden',
        html: `
            <div class="form-group">
                <label class="col-form-label">Estado Actual: <strong>${ordenActual.estado}</strong></label>
            </div>
            <div class="form-group">
                <label for="nuevoEstado" class="col-form-label">Nuevo Estado:</label>
                <select id="nuevoEstado" class="form-control">
                    ${opcionesHtml}
                </select>
            </div>
        `,
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Cambiar Estado',
        cancelButtonText: 'Cancelar',
        allowOutsideClick: false,
        preConfirm: () => {
            return {
                nuevoEstado: document.getElementById('nuevoEstado').value
            };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            const nuevoEstado = result.value.nuevoEstado;

            // Llamada al API para cambiar el estado
            let url = `/GestionOrdenesServicio/CambiarEstadoOrden?id=${id}&nuevoEstado=${nuevoEstado}`;

            // Mostrar indicador de carga
            Swal.fire({
                title: 'Cambiando estado...',
                text: 'Por favor espere',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            apiModule.postFromQuery(url)
                .then(function (response) {
                    Swal.close();

                    if (response.success) {
                        showNotification(response.message, 'success');
                        // Recargar datos de la tabla
                        datTableOrdenes.ajax.reload();
                    } else {
                        showNotification(response.message, response.warning ? 'warning' : 'error');
                    }
                })
                .catch(function (error) {
                    Swal.close();
                    showNotification('Error al procesar la solicitud', 'error');
                });
        }
    });
}


// Create container for searchPanes that can be toggled
$('<div id="searchPanesContainer" style="display:none;"></div>').insertBefore('#tblOrdenes');

// Paneles de busqueda
function toggleSearchPanes() {
    var container = $('#searchPanesContainer');

    if (container.is(':empty')) {
        
        datTableOrdenes.searchPanes.container().appendTo(container);
        datTableOrdenes.searchPanes.resizePanes();
    }

    if (container.is(':visible')) {
        container.slideUp(300);
        $('#toggleSearchPanesBtn').removeClass('active');
    } else {
        container.slideDown(300);
        $('#toggleSearchPanesBtn').addClass('active');
        // Ensure proper sizing when shown
        setTimeout(function () {
            datTableOrdenes.searchPanes.resizePanes();
        }, 350);
    }
}

// Función para ver los detalles de una orden
function verDetallesOrden(id) {
    // Mostrar indicador de carga
    Swal.fire({
        title: 'Cargando detalles...',
        text: 'Por favor espere',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    // Encontrar la orden en el DataTable para tener acceso rápido a datos básicos
    let ordenData = datTableOrdenes.rows().data().toArray().find(row => row.id === id);

    if (!ordenData) {
        Swal.close();
        showNotification("No se pudo encontrar la información de la orden", 'error');
        return;
    }

    // Formatear la información de la orden para el modal
    let estadoClass;
    switch (ordenData.estadoId) {
        case 0: estadoClass = 'badge bg-warning'; break;     // Pendiente
        case 1: estadoClass = 'badge bg-primary'; break;     // En Proceso
        case 2: estadoClass = 'badge bg-success'; break;     // Finalizada
        case 3: estadoClass = 'badge bg-danger'; break;      // Cancelada
        default: estadoClass = 'badge bg-secondary';
    }

    let prioridadClass;
    switch (ordenData.prioridadId) {
        case 0: prioridadClass = 'badge bg-danger'; break;   // Alta
        case 1: prioridadClass = 'badge bg-warning'; break;  // Media
        case 2: prioridadClass = 'badge bg-info'; break;     // Baja
        default: prioridadClass = 'badge bg-secondary';
    }

    // Construir HTML de servicios
    let serviciosHtml = '';
    if (ordenData.servicios && ordenData.servicios.length > 0) {
        serviciosHtml = '<ul class="list-group">';
        ordenData.servicios.forEach(servicio => {
            serviciosHtml += `<li class="list-group-item d-flex justify-content-between align-items-center">
                               ${servicio.nombre}
                               <span class="badge bg-primary rounded-pill">${servicio.publicoId || 'N/A'}</span>
                             </li>`;
        });
        serviciosHtml += '</ul>';
    } else {
        serviciosHtml = '<p class="text-muted">No hay servicios asociados a esta orden</p>';
    }

    // Construir HTML del técnico asignado
    let tecnicoHtml = ordenData.tecnico
        ? `<p><strong>Nombre:</strong> ${ordenData.tecnico.nombreCompleto}</p>
           <p><strong>Cédula:</strong> ${ordenData.tecnico.cedula}</p>`
        : '<p class="text-muted">No hay técnico asignado a esta orden</p>';

    // Cerrar el indicador de carga y mostrar el modal con los detalles
    Swal.close();

    Swal.fire({
        title: `Detalles de la Orden #${ordenData.publicoId}`,
        html: `
            <div class="container-fluid">
                <div class="row mb-3">
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="card-title">Información General</h5>
                            </div>
                            <div class="card-body">
                                <p><strong>Cliente:</strong> ${ordenData.cliente.nombre}</p>
                                <p><strong>Estado:</strong> <span class="${estadoClass}">${ordenData.estado}</span></p>
                                <p><strong>Prioridad:</strong> <span class="${prioridadClass}">${ordenData.prioridad}</span></p>
                                <p><strong>Fecha de Creación:</strong> ${moment(ordenData.fechaCreacion).format("DD/MM/YYYY HH:mm")}</p>
                                <p><strong>Fecha Estimada de Atención:</strong> ${moment(ordenData.fechaEstimadaReal).format("DD/MM/YYYY")}</p>
                                ${ordenData.estadoId === 2 ?
                                 `<p><strong>Fecha Real de Atención:</strong> ${moment(ordenData.fechaRealValue).format("DD/MM/YYYY")}</p>` : ''}
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="card-title">Detalles de la Incidencia</h5>
                            </div>
                            <div class="card-body">
                                <p><strong>Descripción:</strong></p>
                                <p class="text-muted">${ordenData.descripcionIncidencia || 'Sin descripción'}</p>
                                <p><strong>Ubicación:</strong> ${ordenData.ubicacion || 'No especificada'}</p>
                                <p><strong>Categoría:</strong> ${ordenData.categoriaServicio}</p>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="card-title">Servicios Asociados</h5>
                            </div>
                            <div class="card-body">
                                ${serviciosHtml}
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="card">
                            <div class="card-header">
                                <h5 class="card-title">Técnico Asignado</h5>
                            </div>
                            <div class="card-body">
                                ${tecnicoHtml}
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        `,
        width: '800px',
        showCloseButton: true,
        showConfirmButton: true,
        confirmButtonText: 'Cerrar',
        confirmButtonColor: '#3085d6',
        customClass: {
            container: 'swal2-orden-detalles'
        }
    });
}
