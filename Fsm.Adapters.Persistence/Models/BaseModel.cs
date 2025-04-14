using System.ComponentModel.DataAnnotations;

namespace Fsm.Adapters.Persistence.Models
{
    public class BaseModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime? FechaModificacion { get; set; } = null;
    }
}
