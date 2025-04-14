using AutoMapper;
using Fsm.Application.DTOs.Tecnico;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Fsm.Domain.Entities;
using System.Linq.Expressions;

namespace Fsm.Application.Services
{
    public class TecnicoService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public TecnicoService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // necesito crear el servicio para crear un tecnico
        public async Task<Result> AddAsync(CreateTecnicoRequest tecnico)
        {
            // Validar el request
            if (tecnico == null)
                return Result.Warning("El técnico no puede ser nulo.");
            // Mapear el request a la entidad
            var tecnicoEntity = _mapper.Map<Tecnico>(tecnico);
            // Llamar al repositorio para agregar el técnico
            var result = await _unitOfWork.Tecnicos.AddAsync(tecnicoEntity);
            // Guardar los cambios en la base de datos
            await _unitOfWork.SaveChangesAsync();
            // Retornar el resultado
            return result;
        }

        public async Task<Result<IEnumerable<Tecnico>>> GetAllAsync(
            Expression<Func<Tecnico, bool>>? filter = null,
            Func<IQueryable<Tecnico>, IOrderedQueryable<Tecnico>>? orderBy = null,
            string? includeProperties = null)
        {
            // Llamar al repositorio para obtener todos los técnicos
            var result = await _unitOfWork.Tecnicos.GetAllAsync(filter, orderBy, includeProperties);


            // Retornar el resultado
            return result;
        }
    }
}
