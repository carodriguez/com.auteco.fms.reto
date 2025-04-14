using Fsm.Application.DTOs.OrdenDeServicio;
using Fsm.Application.Services;
using Fsm.Domain.Enums;
using Fsm.WebApp.Helper;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Fsm.WebApp.Controllers
{
    public class GestionOrdenesServicioController : Controller
    {
        private readonly OrdenDeServicioService _ordenDeServicioService;
        private readonly ClienteService _clienteService;
        private readonly TecnicoService _tecnicoService;
        private readonly ServicioService _servicioService;
        private readonly INotificacionServiceHelper _notificacionServiceHelper;

        public GestionOrdenesServicioController(
            OrdenDeServicioService ordenDeServicioService,
            ClienteService clienteService,
            TecnicoService tecnicoService,
            ServicioService servicioService,
            INotificacionServiceHelper notificacionServiceHelper)
        {
            _ordenDeServicioService = ordenDeServicioService;
            _clienteService = clienteService;
            _tecnicoService = tecnicoService;
            _servicioService = servicioService;
            _notificacionServiceHelper = notificacionServiceHelper;
        }

        [HttpGet]
        public IActionResult Seguimiento()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AddOrdenServicio()
        {
            var request = new CreateOrdenDeServicioRequest
            {
                Clientes = await LoadClientesAsync(),
                Tecnicos = await LoadTecnicosAsync(),
                Servicios = await LoadServiciosAsync() 
            };

            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrdenServicio(CreateOrdenDeServicioRequest request)
        {
            if (!ModelState.IsValid)
            {
                request.Clientes = await LoadClientesAsync();
                request.Tecnicos = await LoadTecnicosAsync();
                request.Servicios = await LoadServiciosAsync();
                return View(request);
            }

            var result = await _ordenDeServicioService.AddAsync(request);

            if (result.IsSuccess)
            {
                _notificacionServiceHelper.ToastNotification(
                    "Ok!",
                    result.Message,
                    NotificationPosition.Top,
                    NotificationType.Success,
                    1500);

                return RedirectToAction("Seguimiento");
            }

            ModelState.AddModelError(string.Empty, result.Message);
            request.Clientes = await LoadClientesAsync();
            request.Tecnicos = await LoadTecnicosAsync();
            request.Servicios = await LoadServiciosAsync();

            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstadoOrden(Guid id, int nuevoEstado)
        {
            try
            {
                EstadosOrden estadoEnum = (EstadosOrden)nuevoEstado;
                var result = await _ordenDeServicioService.CambiarEstadoAsync(id, estadoEnum);

                if (result.IsSuccess)
                {
                    return Json(new
                    {
                        success = true,
                        message = result.Message
                    });
                }
                else if (result.IsWarning)
                {
                    return Json(new
                    {
                        success = false,
                        warning = true,
                        message = result.Message
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        error = true,
                        message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    error = true,
                    message = "Error al cambiar el estado de la orden: " + ex.Message
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrdenesDeServicio()
        {
            try
            {
                // Obtener todas las órdenes de servicio con datos relacionados incluidos
                var result = await _ordenDeServicioService.GetAllOrderServiceAsync();

               
                return Json(result);
            }
            catch (Exception ex)
            {
                // Registrar la excepción
                Console.WriteLine($"Error al obtener órdenes de servicio: {ex.Message}");

                // Devolver respuesta de error con el mismo formato
                var errorResponse = new
                {
                    success = false,
                    error = true,
                    warning = (bool?)null,
                    info = (bool?)null,
                    message = "Error interno del servidor al obtener las órdenes de servicio",
                    data = (object)null
                };

                return StatusCode(500, errorResponse);
            }
        }

        private async Task<List<SelectListItem>> LoadClientesAsync()
        {
            var clientesResult = await _clienteService.GetAllAsync(c => c.Estado == true);
            return clientesResult.IsSuccess && clientesResult.Value != null
                ? clientesResult.Value.Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Nombre }).ToList()
                : new List<SelectListItem>();
        }

        private async Task<List<SelectListItem>> LoadTecnicosAsync()
        {
            try
            {
                var tecnicosResult = await _tecnicoService.GetAllAsync(t => t.Estado == true);
                return tecnicosResult.IsSuccess && tecnicosResult.Value != null
                    ? tecnicosResult.Value.Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.NombreCompleto }).ToList()
                    : new List<SelectListItem>();
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging framework like Serilog or NLog)
                Console.WriteLine($"Error loading technicians: {ex.Message}");
                return new List<SelectListItem>();
            }
        }

        private async Task<List<SelectListItem>> LoadServiciosAsync()
        {
            var serviciosResult = await _servicioService.GetAllAsync(s => s.Estado== true);
            return serviciosResult.IsSuccess && serviciosResult.Value != null
                ? serviciosResult.Value.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.Nombre }).ToList()
                : new List<SelectListItem>();
        }

    }
}
