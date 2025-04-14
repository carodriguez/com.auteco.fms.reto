using Fsm.Application.DTOs.Cliente;
using Fsm.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fsm.Api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ClienteService _clienteService;

        public ClienteController(ClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost]
        [Route("CrearCliente")]
        public async Task<IActionResult> AddCliente() // [FromBody] CreateClienteRequest cliente
        {
            var cliente = new CreateClienteRequest
            {
                Nombre = "Cliente de Prueba",
                TipoCliente = 1,
                Estado = true
            };

            var result = await _clienteService.AddAsync(cliente);
            if (result.IsSuccess)
                return Ok(result.Value);

            return BadRequest(result.Message);
        }

      
    }
}
