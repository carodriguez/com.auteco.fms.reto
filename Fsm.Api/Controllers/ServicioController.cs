using Fsm.Application.DTOs.Servicio;
using Fsm.Application.DTOs.Tecnico;
using Fsm.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fsm.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServicioController : ControllerBase
    {
        private readonly ServicioService _servicioService;
        public ServicioController(ServicioService servicioService)
        {
            _servicioService = servicioService;
        }


        [HttpPost]
        [Route("CrearTecnico")]
        public async Task<IActionResult> AddServicio() // [FromBody] CreateTecnicoRequest tecnico
        {
            // Creación de varios servicios para pruebas
            List<CreateServicioRequest> createServicioRequests = new List<CreateServicioRequest>
            {
                new CreateServicioRequest
                {
                    Nombre = "Instalación de Climatización",
                    Estado = true
                },
                new CreateServicioRequest
                {
                    Nombre = "Mantenimiento de Aire Acondicionado",
                    Estado = true
                },
                new CreateServicioRequest
                {
                    Nombre = "Reparación de Calefacción",
                    Estado = true
                },
                new CreateServicioRequest
                {
                    Nombre = "Limpieza de Conductos",
                    Estado = true
                },
                new CreateServicioRequest
                {
                    Nombre = "Revisión de Sistemas HVAC",
                    Estado = true
                }
            };


            // Crear los tecnicos
            foreach (var servicio in createServicioRequests)
            {
                var result = await _servicioService.AddAsync(servicio);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);
            }


            // Retornar el resultado
            return Ok("Servicios creados exitosamente");

        }
    }
}
