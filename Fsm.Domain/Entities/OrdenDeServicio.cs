using Fsm.Domain.Constants;
using Fsm.Domain.Enums;

namespace Fsm.Domain.Entities
{
    public class OrdenDeServicio
    {
        public Guid Id { get; set; }

        public string PublicoId { get; set; }
        
        public Cliente Cliente { get; set; } = null!;
        public EstadosOrden Estado { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public DateTime FechaDeAtencionEstimada { get; set; } = DateTime.Now;

        public DateTime FechaDeAtencionReal { get; set; } = DateTime.Now;

        
        public string DescripcionIncidencia { get; set; } = null!;
        public string Ubicacion { get; set; } = null!;
        public CategoriaServicio CategoriaServicio { get; set; }
        public PrioridadOrden Prioridad { get; set; }

        public List<OrdenDeServicioDetalle> Detalles { get; private set; } = new();

     
       
        public Tecnico Tecnico { get; set; } = null!;

        public void AsignarTecnico(Tecnico tecnico)
        {
            Tecnico = tecnico ?? throw new ArgumentNullException(nameof(tecnico));


        }
        private OrdenDeServicio() { }

        OrdenDeServicio(Guid id
            , Cliente cliente
            , Tecnico tecnico
            , EstadosOrden estado
            
            , DateTime fechaCreacion
            , DateTime fechaDeAtencionEstimada
            , DateTime fechaDeAtencionReal
              , string descripcionIncidencia
            , string ubicacion
            , CategoriaServicio categoriaServicio
            , PrioridadOrden prioridad)
        {
            Id = id;
            Cliente = cliente;
            Tecnico = tecnico;
            Estado = estado;
            FechaCreacion = fechaCreacion;
            FechaDeAtencionEstimada = fechaDeAtencionEstimada;
            FechaDeAtencionReal = fechaDeAtencionReal;
            DescripcionIncidencia = descripcionIncidencia;
            Ubicacion = ubicacion;
            CategoriaServicio = categoriaServicio;
            Prioridad = prioridad;
        }
        public static OrdenDeServicio Crear(Cliente cliente)
        {
            var ordenDeServicio = new OrdenDeServicio
            {
                Cliente = cliente,
                CategoriaServicio = CategoriaServicio.MantenimientoCorrectivo,  // Valor por defecto
                Prioridad = PrioridadOrden.Media,  // Valor por defecto
                Estado = EstadosOrden.Pendiente
            };
            ordenDeServicio.CalcularFechaDeAtencionEstimada(OrdenDeServicioConstants.SinAcuerdoNivelDeServicio);

            return ordenDeServicio;
        }

        public void EstablecerDetallesAdicionales(string descripcionIncidencia, string ubicacion,
                                              CategoriaServicio categoriaServicio, PrioridadOrden prioridad)
        {
            DescripcionIncidencia = descripcionIncidencia ?? throw new ArgumentNullException(nameof(descripcionIncidencia));
            Ubicacion = ubicacion ?? throw new ArgumentNullException(nameof(ubicacion));
            CategoriaServicio = categoriaServicio;
            Prioridad = prioridad;
        }


        // Agrego una funcion para calcular la fecha de atencion estimada
        public void CalcularFechaDeAtencionEstimada(int dias)
        {
            var nuevoTiempoEstimadoDeAtencion = FechaDeAtencionEstimada.AddDays(dias);

            if (nuevoTiempoEstimadoDeAtencion.Date > FechaDeAtencionEstimada.Date)
            {
                FechaDeAtencionEstimada = nuevoTiempoEstimadoDeAtencion;
            }
        }

        public void AgregarDetalle(OrdenDeServicioDetalle detalle)
        {
            if (detalle == null)
                throw new ArgumentNullException(nameof(detalle));
            Detalles.Add(detalle);
        }

        public void AgregarDetalles(List<OrdenDeServicioDetalle> detalles)
        {
            Detalles.AddRange(detalles);
        }


    }
}
