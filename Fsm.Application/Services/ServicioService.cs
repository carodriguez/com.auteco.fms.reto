using AutoMapper;
using Fsm.Application.DTOs.OrdenDeServicio;
using Fsm.Application.DTOs.Servicio;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Fsm.Domain.Entities;
using System.Linq.Expressions;


namespace Fsm.Application.Services
{
    public class ServicioService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ServicioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Agrega un nuevo servicio
        /// </summary>
        public async Task<Result<Servicio>> AddAsync(CreateServicioRequest request)
        {
            // Mapeo con AutoMapper CreateServicioRequest -> Servicio
            var servicio = _mapper.Map<Servicio>(request);

            // Generar el ID público si es necesario
            if (string.IsNullOrEmpty(servicio.PublicoId))
            {
                // Asumiendo que existe un método similar en IServicioRepository
                servicio.PublicoId = await _unitOfWork.Servicios.GeneratePublicoIdAsync();
            }

            await _unitOfWork.Servicios.AddAsync(servicio);

            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            // Verificar si se guardaron filas
            if (filasAfectadas == 0)
            {
                return Result<Servicio>.Error("No se pudo agregar el servicio");
            }

            // Retornar el resultado
            return Result<Servicio>.Success(servicio, "Servicio agregado exitosamente");
        }

        /// <summary>
        /// Obtiene un servicio por su ID
        /// </summary>
        public async Task<Result<Servicio>> GetByIdAsync(int id)
        {
            // Llamar al repositorio para obtener el servicio por Id
            var servicio = await _unitOfWork.Servicios.GetByIdAsync(id);

            if (servicio == null || !servicio.IsSuccess)
            {
                return Result<Servicio>.Warning("Servicio no encontrado");
            }

            return Result<Servicio>.Success(servicio.Value, "Servicio encontrado exitosamente");
        }

        /// <summary>
        /// Obtiene todos los servicios con filtros opcionales
        /// </summary>
        public async Task<Result<IEnumerable<Servicio>>> GetAllAsync(
            Expression<Func<Servicio, bool>>? filter = null,
            Func<IQueryable<Servicio>, IOrderedQueryable<Servicio>>? orderBy = null,
            string? includeProperties = null)
        {
            // Llamar al repositorio para obtener todos los servicios
            var result = await _unitOfWork.Servicios.GetAllAsync(filter, orderBy, includeProperties);

            // No es necesario ningún procesamiento adicional como en el caso de Cliente con TipoClienteNombre

            // Retornar el resultado
            return result;
        }

        /// <summary>
        /// Actualiza un servicio existente
        /// </summary>
        public async Task<Result<Servicio>> UpdateAsync(Servicio servicio)
        {
            // Se asume que el servicio ya tiene la propiedad Id asignada correctamente.
            var result = await _unitOfWork.Servicios.UpdateAsync(servicio);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                return Result<Servicio>.Error("No se pudo actualizar el servicio");
            }

            return Result<Servicio>.Success(servicio, "Servicio actualizado exitosamente");
        }

      
    }
}

