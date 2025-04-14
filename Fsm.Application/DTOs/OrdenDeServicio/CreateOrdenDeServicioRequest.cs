using Fsm.Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;


namespace Fsm.Application.DTOs.OrdenDeServicio
{
    public class CreateOrdenDeServicioRequest
    {
        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Cliente")]
        public Guid ClienteId { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Descripción de la incidencia")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string DescripcionIncidencia { get; set; } = null!;

        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Ubicación")]
        [StringLength(200, ErrorMessage = "La ubicación no puede exceder los 200 caracteres.")]
        public string Ubicacion { get; set; } = null!;

        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Categoría de Servicio")]
        public CategoriaServicio CategoriaServicio { get; set; }

        [Required(ErrorMessage = "Campo requerido.")]
        [Display(Name = "Prioridad")]
        public PrioridadOrden Prioridad { get; set; }

        [Display(Name = "Fecha de atención estimada")]
        [DataType(DataType.Date)]
        public DateTime? FechaAtencionEstimada { get; set; }

        [Display(Name = "Técnico asignado")]
        public Guid? TecnicoId { get; set; }

        [Display(Name = "Servicios")]
        [Required(ErrorMessage = "Debe seleccionar al menos un servicio.")]
        public List<Guid> DetallesIds { get; set; } = new List<Guid>();

        // Propiedad adicional para la vista
        public List<SelectListItem> Servicios { get; set; } = new List<SelectListItem>();

        // Listas para los dropdowns
        public List<CategoriaServicio> CategoriasServicio { get; set; } = Enum.GetValues<CategoriaServicio>()
            .OrderBy(cs => (int)cs)
            .ToList();

        public List<PrioridadOrden> Prioridades { get; set; } = Enum.GetValues<PrioridadOrden>()
            .OrderBy(p => (int)p)
            .ToList();

        // Se inicializarán desde el controlador
        public List<SelectListItem> Clientes { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Tecnicos { get; set; } = new List<SelectListItem>();
    }
}
