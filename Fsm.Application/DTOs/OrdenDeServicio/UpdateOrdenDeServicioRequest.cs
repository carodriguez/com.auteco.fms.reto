using Fsm.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Fsm.Application.DTOs.OrdenDeServicio
{
    public class UpdateOrdenDeServicioRequest
    {
        [Required(ErrorMessage = "El ID de la orden es requerido.")]
        public Guid Id { get; set; }

        [Display(Name = "Estado")]
        public EstadosOrden? Estado { get; set; }

        [Display(Name = "Descripción de la incidencia")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string? DescripcionIncidencia { get; set; }

        [Display(Name = "Ubicación")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        public string? Ubicacion { get; set; }

        [Display(Name = "Categoría de Servicio")]
        public CategoriaServicio? CategoriaServicio { get; set; }

        [Display(Name = "Prioridad")]
        public PrioridadOrden? Prioridad { get; set; }

        [Display(Name = "Fecha de atención estimada")]
        [DataType(DataType.DateTime)]
        public DateTime? FechaDeAtencionEstimada { get; set; }

        [Display(Name = "Técnico asignado")]
        public Guid? TecnicoId { get; set; }

        [Display(Name = "Nuevos servicios a agregar")]
        public List<Guid>? NuevosDetallesIds { get; set; }

        [Display(Name = "Servicios a eliminar")]
        public List<Guid>? DetallesAEliminarIds { get; set; }

        // Propiedades para la vista
        public List<SelectListItem>? Estados { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? Tecnicos { get; set; } = new List<SelectListItem>();
        public List<SelectListItem>? Servicios { get; set; } = new List<SelectListItem>();

        public List<CategoriaServicio> CategoriasServicio { get; set; } = Enum.GetValues<CategoriaServicio>()
            .OrderBy(cs => (int)cs)
            .ToList();

        public List<PrioridadOrden> Prioridades { get; set; } = Enum.GetValues<PrioridadOrden>()
            .OrderBy(p => (int)p)
            .ToList();

        // Propiedades de solo lectura para mostrar información
        [Display(Name = "Cliente")]
        public string? ClienteNombre { get; set; }

        [Display(Name = "Fecha de creación")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Servicios actuales")]
        public List<ServicioItemDto>? ServiciosActuales { get; set; }
    }

    // DTO auxiliar para mostrar los servicios actuales
    public class ServicioItemDto
    {
        public Guid Id { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Editable { get; set; } = true;
    }
}

