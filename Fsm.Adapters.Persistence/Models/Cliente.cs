using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fsm.Adapters.Persistence.Models
{
    [Table("Clientes")]
    public class Cliente : BaseModel
    {
       

        [Required]
        [StringLength(50)]
        public string PublicoId { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = null!;

        [Required]
       
        public int? TipoCliente { get; set; } = null!;

        [Required]

        public bool? Estado { get; set; } = null!;


    }
}
