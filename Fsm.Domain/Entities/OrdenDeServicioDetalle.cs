namespace Fsm.Domain.Entities
{
    public class OrdenDeServicioDetalle
    {
        public Guid Id { get; set; }
        public Servicio Servicio { get; set; }


        public OrdenDeServicioDetalle(Servicio servicio)
        {
            Servicio = servicio;
        }

       

      


    }
}