using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Fsm.Adapters.Persistence.Models
{
    [Table("OrdenesDeServicioDetalle")]
    public class OrdenDeServicioDetalle 


    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid OrdenDeServicioId { get; set; }

        [Required]
        public Guid ServicioId { get; set; }


        [ForeignKey("OrdenDeServicioId")]
        public virtual OrdenDeServicio OrdenDeServicio { get; set; } = null!;

        [ForeignKey("ServicioId")]
        public virtual Servicio Servicio { get; set; } = null!;

    }
}
