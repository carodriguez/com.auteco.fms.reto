using Fsm.Domain.Entities;

namespace Fsm.Application.DTOs.OrdenDeServicio
{
    internal class OrdenValidationResult
    {
        /// <summary>
        /// Cliente validado para la orden de servicio
        /// </summary>
        public Fsm.Domain.Entities.Cliente Cliente { get; }

        /// <summary>
        /// Lista de servicios validados para la orden de servicio
        /// </summary>
        public List<Fsm.Domain.Entities.Servicio> Servicios { get; }

        /// <summary>
        /// Constructor que recibe el cliente y la lista de servicios validados
        /// </summary>
        /// <param name="cliente">Cliente validado</param>
        /// <param name="servicios">Lista de servicios validados</param>
        public OrdenValidationResult(Fsm.Domain.Entities.Cliente cliente, List<Fsm.Domain.Entities.Servicio> servicios)
        {
            Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
            Servicios = servicios ?? throw new ArgumentNullException(nameof(servicios));
        }

    }
}
