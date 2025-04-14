using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Fsm.Domain.Entities
{
    public class Cliente
    {
        public Guid? Id { get; set; } = null!;
        public string PublicoId { get; set; } = null!;

        public string Nombre { get; set; } = null!;
   
        public int? TipoCliente { get; set; } = null!;

        public string? TipoClienteNombre { get; set; } = null!;

        public bool? Estado { get; set; } = null!;

        public DateTime? FechaModificacion { get; set; } = null!;
         

        public override string ToString()
        {
            return $"ClienteId(Id: {Id}, PublicoId: {PublicoId}, Nombre: {Nombre}";
        }
    }
}