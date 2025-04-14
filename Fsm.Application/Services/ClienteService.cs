using AutoMapper;
using Fsm.Application.DTOs.Cliente;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Fsm.Domain.Entities;
using System.Linq.Expressions;

namespace Fsm.Application.Services
{
    public class ClienteService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClienteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<Cliente>> AddAsync(CreateClienteRequest request)
        {


            // Mapeo con AutoMapper CreateClienteRequest -> Cliente
            var cliente = _mapper.Map<Cliente>(request);


            await _unitOfWork.Clientes.AddAsync(cliente);

            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            // Verificar si se guardaron filas
            if (filasAfectadas == 0)
            {
                return Result<Cliente>.Error("No se pudo agregar el cliente");
            }

            // Retornar el resultado
            return Result<Cliente>.Success(cliente, "Cliente agregado exitosamente");

        }


        public async Task<Result<Cliente>> GetByIdAsync(Guid id)
        {
            // Llamar al repositorio para obtener el cliente por Id
            var cliente = await _unitOfWork.Clientes.GetByIdAsync(id);

            if (cliente == null)
            {
                return Result<Cliente>.Warning("Cliente no encontrado");
            }

            return Result<Cliente>.Success(cliente.Value, "Cliente encontrado exitosamente");
        }


        public async Task<Result<IEnumerable<Cliente>>> GetAllAsync(
         Expression<Func<Cliente, bool>>? filter = null,
         Func<IQueryable<Cliente>, IOrderedQueryable<Cliente>>? orderBy = null,
         string? includeProperties = null)
        {
            // Llamar al repositorio para obtener todos los clientes
            var result = await _unitOfWork.Clientes.GetAllAsync(filter, orderBy, includeProperties);

            // Verificar si hay datos y fueron obtenidos exitosamente
            if (result.IsSuccess && result.Value != null)
            {
                // Mapear TipoCliente a TipoClienteNombre para cada cliente
                foreach (var cliente in result.Value)
                {
                    if (cliente.TipoCliente.HasValue)
                    {
                        // Obtener el nombre del enum directamente
                        cliente.TipoClienteNombre = Enum.IsDefined(typeof(Domain.Enums.TipoCliente), cliente.TipoCliente.Value)
                            ? Enum.GetName(typeof(Domain.Enums.TipoCliente), cliente.TipoCliente.Value)
                            : "Desconocido";
                    }
                }
            }

            // Retornar el resultado
            return result;

          
          
        }




        public async Task<Result<Cliente>> UpdateAsync(Cliente cliente)
        {
            // Se asume que el cliente ya tiene la propiedad Id asignada correctamente.
            var result = await _unitOfWork.Clientes.UpdateAsync(cliente);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();
            if (filasAfectadas == 0)
            {
                return Result<Cliente>.Error("No se pudo actualizar el cliente");
            }
            return Result<Cliente>.Success(cliente, "Cliente actualizado exitosamente");
        }

        public async Task<Result> DeleteLogicAsync(string publicoId )
        {
            // Buscar el cliente por su PublicoId.
            var clienteResult = await _unitOfWork.Clientes.GetByPublicId(publicoId);
            if (clienteResult == null || clienteResult.Value == null)
            {
                return Result.Warning("Cliente no encontrado");
            }

            // Actualizar el estado a false (inactivo)
            clienteResult.Value.Estado = false;
            
            // Llamar al repositorio para actualizar el cliente
            var updateResult = await _unitOfWork.Clientes.UpdateAsync(clienteResult.Value);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();
            if (filasAfectadas == 0)
            {
                return Result.Error("No se pudo inactivar el cliente");
            }
            return Result.Success("Cliente inactivado correctamente");
        }

    }
}
