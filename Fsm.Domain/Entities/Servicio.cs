using System.IO.Pipes;

namespace Fsm.Domain.Entities
{
    public class Servicio
    {

                public Guid Id { get; set; }

        public  string PublicoId { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        
        public bool? Estado { get; set; } = null!;

        public override string ToString()
        {
            return $"Servicio(Id: {Id}, PublicoId: {PublicoId}, Nombre: {Nombre})";
        }
    }
}
