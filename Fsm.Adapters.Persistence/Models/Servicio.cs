using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fsm.Adapters.Persistence.Models
{
    [Table("Servicios")]
    public class Servicio 

    {
        public Guid Id { get; set; }

        /// Propiedades de la entidad
        [StringLength(50)]
        public string? PublicoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;
      
        [Required]
        public bool? Estado { get; set; } = true;
    


    }
}
