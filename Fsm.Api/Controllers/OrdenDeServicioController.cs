using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fsm.Api.Controllers
{

    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ordenes")]
    public class OrdenDeServicioController : ControllerBase
    {
    }
}
