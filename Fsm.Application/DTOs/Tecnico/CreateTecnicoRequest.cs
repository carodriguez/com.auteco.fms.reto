namespace Fsm.Application.DTOs.Tecnico
{
    public class CreateTecnicoRequest
    {
        public string? Cedula { get; set; }
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public int? Especialidad { get; set; }

        public bool? Estado { get; set; } = true;
    }
}
