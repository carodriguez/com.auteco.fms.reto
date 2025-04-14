using Fsm.Application.DTOs.Cliente;
using Fsm.Application.Services;
using Fsm.Domain.Common;
using Fsm.WebApp.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace Fsm.WebApp.Controllers
{
    public class GestionClientesController : Controller
    {
        private readonly ClienteService _clienteService;

        private readonly INotificacionServiceHelper _notificacionServiceHelper;
        public GestionClientesController(INotificacionServiceHelper notificacionServiceHelper, ClienteService clienteService)
        {
            _notificacionServiceHelper = notificacionServiceHelper;
            _clienteService = clienteService;
        }
     

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Nota: Se puede mejorar este metodo haciendo un paginado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetClientesActivos()
        {

            var result = await _clienteService.GetAllAsync(c => c.Estado == true);

            var response = new
            {
                success = result.IsSuccess,
                warning = result.IsWarning ? true : (bool?)null,
                error = result.IsError ? true : (bool?)null,
                info = result.IsInfo ? true : (bool?)null,
                message = result.Message,
                data = result.IsSuccess ? result.Value : null
            };


            return Json(response);
        }


        [HttpGet]
        public IActionResult AddCliente()
        {

            CreateClienteRequest createClienteRequest = new CreateClienteRequest
            {
                Nombre = string.Empty,
                Estado = true
            };
            return View(createClienteRequest);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCliente(CreateClienteRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, devolver la vista con los errores de validación
                return View(request);
            }

            // Llamar al servicio para agregar el cliente
            var result = await _clienteService.AddAsync(request);

            if (result.IsSuccess)
            {

                _notificacionServiceHelper.ToastNotification("Ok!", result.Message
                 , NotificationPosition.Top, NotificationType.Success, 1500);


                return RedirectToAction("Index");
            }

            // Si hubo un error, mostrar el mensaje en la vista
            ModelState.AddModelError(string.Empty, result.Message);
            return View(request);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteCliente([FromQuery] string publicoId)
        {

            // Llamar al servicio para eliminar logicamente el cliente
            var result = await _clienteService.DeleteLogicAsync(publicoId);


            var response = new
            {
                success = result.IsSuccess,
                warning = result.IsWarning ? true : (bool?)null,
                error = result.IsError ? true : (bool?)null,
                info = result.IsInfo ? true : (bool?)null,
                message = result.Message,
            };


            return Json(response);


        }



    }
}
