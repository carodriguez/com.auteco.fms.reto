using Fsm.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Fsm.Application.DTOs.Cliente
{
    public class CreateClienteRequest
    {
        [Required(ErrorMessage = "Campo requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        public bool? Estado { get; set; } = true;

        [Display(Name = "Tipos Cliente")]
        [Required(ErrorMessage = "Campo requerido.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de cliente válido.")]
        public int TipoCliente { get; set; } = 0;


        [Display(Name = "Tipos Cliente")]
        public List<TipoCliente> TiposCliente { get; set; } = Enum.GetValues<TipoCliente>()
                 .OrderBy(tc => (int)tc) // Ordenar por el valor numérico del enum
                 .ToList();

    }
}
