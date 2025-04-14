using Fsm.Application.DTOs.Tecnico;
using Fsm.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fsm.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TecnicoController : ControllerBase
    {
        private readonly TecnicoService _tecnicoService;
        public TecnicoController(TecnicoService tecnicoService)
        {
            _tecnicoService = tecnicoService;
        }

        [HttpPost]
        [Route("CrearTecnico")]
        public async Task<IActionResult> AddTecnico() // [FromBody] CreateTecnicoRequest tecnico
        {
            // Necesito crear varios tecnicos para pruebas
            List<CreateTecnicoRequest> createTecnicoRequests = new List<CreateTecnicoRequest>
            {
                new CreateTecnicoRequest
                {
                    Cedula = "123456789",
                    Nombre = "Carlos",
                    Apellido = "Rodriguez",
                    Especialidad = 1,
                    Estado = true
                },
                new CreateTecnicoRequest
                {
                    Cedula = "987654321",
                    Nombre = "Ana",
                    Apellido = "Gomez",
                    Especialidad = 2,
                    Estado = true
                },
                new CreateTecnicoRequest
                {
                    Cedula = "456789123",
                    Nombre = "Luis",
                    Apellido = "Martinez",
                    Especialidad = 3,
                    Estado = true
                }
            };

            // Crear los tecnicos
            foreach (var tecnico in createTecnicoRequests)
            {
                var result = await _tecnicoService.AddAsync(tecnico);
                if (!result.IsSuccess)
                    return BadRequest(result.Message);
            }


            // Retornar el resultado
            return Ok("Tecnicos creados exitosamente");

        }
    }
}
