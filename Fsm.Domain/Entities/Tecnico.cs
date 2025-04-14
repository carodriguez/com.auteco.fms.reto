using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fsm.Domain.Entities
{
    public class Tecnico
    {
        [Key]
        public Guid Id { get; set; }

        public string? Cedula { get; set; }

     
        public string Nombre { get; set; } = null!;

        public string Apellido { get; set; } = null!;

        public int? Especialidad { get; set; }

        public bool? Estado { get; set; } = true;

        // Propiedad de navegación para la especialidad (si existe un enum Especialidad)
        // [NotMapped]
        // public string? EspecialidadNombre { get; set; }

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        public override string ToString() => NombreCompleto;
    }
}
