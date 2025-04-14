using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fsm.Adapters.Persistence.Models
{
    [Table("Tecnicos")]
    public class Tecnico 
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(50)]
        public string? Cedula { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Apellido { get; set; } = null!;

        public int? Especialidad { get; set; }

        [Required]
        public bool? Estado { get; set; } = true;

        // Propiedad de navegación para la especialidad (si existe un enum Especialidad)
        // [NotMapped]
        // public string? EspecialidadNombre { get; set; }

        [NotMapped]
        public string NombreCompleto => $"{Nombre} {Apellido}";

        public override string ToString() => NombreCompleto;
    }
}
