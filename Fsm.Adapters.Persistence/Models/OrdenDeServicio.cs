using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fsm.Adapters.Persistence.Models
{
    [Table("OrdenesDeServicio")]
    public class OrdenDeServicio: BaseModel
    {


        [Required]
        [StringLength(50)]
        public string PublicoId { get; set; } = null!;

        [Required]
        public Guid ClienteId { get; set; }

        [Required]
        public Guid TecnicoId { get; set; }

        [Required]
        public int Estado { get; set; }

        [Required]
        public DateTime FechaDeAtencionEstimada { get; set; }

        [Required]
        public DateTime FechaDeAtencionReal { get; set; }
       
       
        [Required]
        [StringLength(500)]
        public string DescripcionIncidencia { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Ubicacion { get; set; } = null!;

        [Required]
        public int CategoriaServicio { get; set; }

        [Required]
        public int Prioridad { get; set; }

        // Navigation properties
        [ForeignKey("ClienteId")]
        public Cliente Cliente { get; set; } = null!;


        // Navigation properties
        [ForeignKey("TecnicoId")]
        public Tecnico Tecnico { get; set; } = null!;

        public ICollection<OrdenDeServicioDetalle> Detalles { get; set; } = new List<OrdenDeServicioDetalle>();
    }
}
