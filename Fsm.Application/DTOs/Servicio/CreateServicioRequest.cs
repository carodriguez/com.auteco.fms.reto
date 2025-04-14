namespace Fsm.Application.DTOs.Servicio
{
    public class CreateServicioRequest
    {
        
        public string Nombre { get; set; } = null!;

        public bool? Estado { get; set; } = true;
    }
}
