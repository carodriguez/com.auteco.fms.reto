using AutoMapper;
using Fsm.Application.DTOs.OrdenDeServicio;
using Fsm.Application.Interfaces.Persistence;
using Fsm.Domain.Common;
using Fsm.Domain.Constants;
using Fsm.Domain.Entities;
using Fsm.Domain.Enums;
using System.Linq.Expressions;

namespace Fsm.Application.Services
{
    public class OrdenDeServicioService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public OrdenDeServicioService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Crea una nueva orden de servicio
        /// </summary>
        public async Task<Result<OrdenDeServicio>> AddAsync(CreateOrdenDeServicioRequest request)
        {
            // Validar cliente y servicios solicitados
            var validationResult = await ValidarClienteYServicios(request);
            if (!validationResult.IsSuccess)
            {
                return Result<OrdenDeServicio>.Error(validationResult.Message);
            }

            var cliente = validationResult.Value!.Cliente;
            var servicios = validationResult.Value!.Servicios;

            // Crear la orden de servicio
            var ordenDeServicio = OrdenDeServicio.Crear(cliente);


            // Establecer los campos adicionales
            ordenDeServicio.EstablecerDetallesAdicionales(
                request.DescripcionIncidencia,
                request.Ubicacion,
                request.CategoriaServicio,
                request.Prioridad
            );


            // Asignar técnicos si es necesario
            if (request.TecnicoId.HasValue)
            {
                var tecnicoResult = await _unitOfWork.Tecnicos.GetByIdAsync(request.TecnicoId.Value);
                if (tecnicoResult.IsSuccess && tecnicoResult.Value != null)
                {
                    //Asignar técnico
                    ordenDeServicio.AsignarTecnico(tecnicoResult.Value);
                }
            }

            // Configurar fecha estimada de atención si se especificó
            if (request.FechaAtencionEstimada.HasValue)
            {
                // Calcular días adicionales basados en la fecha especificada
                var diasAdicionales = (request.FechaAtencionEstimada.Value - DateTime.UtcNow).Days;
                if (diasAdicionales > 0)
                {
                    ordenDeServicio.CalcularFechaDeAtencionEstimada(diasAdicionales);
                }
            }

            // Agregar detalles de servicios
            foreach (var servicio in servicios)
            {
                var detalle = new OrdenDeServicioDetalle(servicio);
                ordenDeServicio.AgregarDetalle(detalle);
            }

            // Guardar la orden en la base de datos
            await _unitOfWork.OrdenesDeServicio.AddAsync(ordenDeServicio);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                return Result<OrdenDeServicio>.Error("No se pudo agregar la orden de servicio");
            }

            Console.WriteLine($"[INFO] Orden de servicio creada con ID: {ordenDeServicio.Id}");
            return Result<OrdenDeServicio>.Success(ordenDeServicio, "Orden de servicio agregada exitosamente");
        }


        /// <summary>
        /// Obtiene una orden de servicio por su ID
        /// </summary>
        public async Task<Result<OrdenDeServicioResponse>> GetByIdAsync(Guid id)
        {
            var ordenResult = await _unitOfWork.OrdenesDeServicio.GetByIdAsync(id);

            if (!ordenResult.IsSuccess || ordenResult.Value == null)
            {
                return Result<OrdenDeServicioResponse>.Warning($"Orden de servicio con ID '{id}' no encontrada");
            }

            var ordenResponse = _mapper.Map<OrdenDeServicioResponse>(ordenResult.Value);
            return Result<OrdenDeServicioResponse>.Success(ordenResponse, "Orden de servicio encontrada exitosamente");
        }

        /// <summary>
        /// Obtiene todas las órdenes de servicio con filtros opcionales
        /// </summary>
        public async Task<Result<IEnumerable<OrdenDeServicioResponse>>> GetAllAsync(
            Expression<Func<OrdenDeServicio, bool>>? filter = null,
            Func<IQueryable<OrdenDeServicio>, IOrderedQueryable<OrdenDeServicio>>? orderBy = null,
            string? includeProperties = null)
        {
            var result = await _unitOfWork.OrdenesDeServicio.GetAllAsync(
                filter,
                orderBy,
                includeProperties ?? "Cliente,Detalles.Servicio");

            if (!result.IsSuccess)
            {
                return Result<IEnumerable<OrdenDeServicioResponse>>.Error(result.Message);
            }

            var ordenesResponse = _mapper.Map<IEnumerable<OrdenDeServicioResponse>>(result.Value);
            return Result<IEnumerable<OrdenDeServicioResponse>>.Success(ordenesResponse, result.Message);
        }

        /// <summary>
        /// Actualiza una orden de servicio existente
        /// </summary>
        public async Task<Result<OrdenDeServicio>> UpdateAsync(UpdateOrdenDeServicioRequest request)
        {
            var ordenResult = await _unitOfWork.OrdenesDeServicio.GetByIdAsync(request.Id);
            if (!ordenResult.IsSuccess || ordenResult.Value == null)
            {
                return Result<OrdenDeServicio>.Warning("Orden de servicio no encontrada");
            }

            var orden = ordenResult.Value;

            // Actualizar propiedades según sea necesario
            if (request.Estado.HasValue)
            {
                orden.Estado = request.Estado.Value;

                // Si se está finalizando la orden, actualizar la fecha de atención real
                if (request.Estado == EstadosOrden.Finalizada)
                {
                    orden.FechaDeAtencionReal = DateTime.UtcNow;
                }
            }

            if (request.FechaDeAtencionEstimada.HasValue)
            {
                orden.FechaDeAtencionEstimada = request.FechaDeAtencionEstimada.Value;
            }

            // Actualizar detalles si es necesario
            if (request.NuevosDetallesIds != null && request.NuevosDetallesIds.Any())
            {
                foreach (var servicioId in request.NuevosDetallesIds)
                {
                    await AgregarDetalleInterno(orden, servicioId);
                }
            }

            var updateResult = await _unitOfWork.OrdenesDeServicio.UpdateAsync(orden);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                return Result<OrdenDeServicio>.Error("No se pudo actualizar la orden de servicio");
            }

            return Result<OrdenDeServicio>.Success(orden, "Orden de servicio actualizada exitosamente");
        }

        /// <summary>
        /// Cambia el estado de una orden de servicio
        /// </summary>
        public async Task<Result> CambiarEstadoAsync(Guid id, EstadosOrden nuevoEstado)
        {
            try
            {
                var ordenResult = await _unitOfWork.OrdenesDeServicio.GetByIdAsync(id);

                if (!ordenResult.IsSuccess || ordenResult.Value == null)
                {
                    return Result.Warning("Orden de servicio no encontrada");
                }

                var orden = ordenResult.Value;

                // Validar transiciones de estado
                if (!ValidarTransicionEstado(orden.Estado, nuevoEstado))
                {
                    return Result.Warning($"No se permite cambiar de estado {orden.Estado} a {nuevoEstado}");
                }

                // Modificar solo los campos que cambian
                var updateProps = new Dictionary<string, object>
                {
                    { "Estado", nuevoEstado }
                };

                if (nuevoEstado == EstadosOrden.Finalizada)
                {
                    updateProps.Add("FechaDeAtencionReal", DateTime.Now);
                }

                // Actualizar solo los campos específicos
                var updateResult = await _unitOfWork.OrdenesDeServicio.UpdatePartialAsync(id, updateProps);

                int filasAfectadas = await _unitOfWork.SaveChangesAsync();

                if (filasAfectadas == 0)
                {
                    return Result.Error("No se pudo actualizar el estado de la orden de servicio");
                }

                return Result.Success($"Estado de la orden actualizado a {nuevoEstado} correctamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al actualizar el estado de la orden: {ex.Message}");
            }
        }


        /// <summary>
        /// Elimina una orden de servicio
        /// </summary>
        public async Task<Result> DeleteAsync(Guid id)
        {
            var ordenResult = await _unitOfWork.OrdenesDeServicio.GetByIdAsync(id);

            if (!ordenResult.IsSuccess || ordenResult.Value == null)
            {
                return Result.Warning("Orden de servicio no encontrada");
            }

            // Verificar si la orden se puede eliminar (por ejemplo, si no está en proceso)
            if (ordenResult.Value.Estado == EstadosOrden.EnProceso)
            {
                return Result.Error("No se puede eliminar una orden que está en proceso");
            }

            var result = await _unitOfWork.OrdenesDeServicio.DeleteAsync(ordenResult.Value);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                return Result.Error("No se pudo eliminar la orden de servicio");
            }

            return Result.Success("Orden de servicio eliminada correctamente");
        }

        /// <summary>
        /// Agrega un servicio a una orden existente
        /// </summary>
        public async Task<Result> AgregarDetalleAsync(Guid ordenId, Guid servicioId)
        {
            var ordenResult = await _unitOfWork.OrdenesDeServicio.GetByIdAsync(ordenId);
            if (!ordenResult.IsSuccess || ordenResult.Value == null)
            {
                return Result.Warning("Orden de servicio no encontrada");
            }

            // Verificar si la orden ya está finalizada
            if (ordenResult.Value.Estado == EstadosOrden.Finalizada || ordenResult.Value.Estado == EstadosOrden.Cancelada)
            {
                return Result.Error("No se pueden agregar servicios a una orden finalizada o cancelada");
            }

            var orden = ordenResult.Value;
            var resultado = await AgregarDetalleInterno(orden, servicioId);

            if (!resultado.IsSuccess)
            {
                return resultado;
            }

            var updateResult = await _unitOfWork.OrdenesDeServicio.UpdateAsync(orden);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                return Result.Error("No se pudo agregar el detalle a la orden de servicio");
            }

            return Result.Success("Detalle agregado correctamente a la orden de servicio");
        }

        /// <summary>
        /// Recalcula la fecha estimada de atención basada en la prioridad o SLA
        /// </summary>
        public async Task<Result> RecalcularFechaEstimadaAsync(Guid ordenId, PrioridadOrden prioridad)
        {
            var ordenResult = await _unitOfWork.OrdenesDeServicio.GetByIdAsync(ordenId);
            if (!ordenResult.IsSuccess || ordenResult.Value == null)
            {
                return Result.Warning("Orden de servicio no encontrada");
            }

            var orden = ordenResult.Value;

            // Aplicar diferentes días según la prioridad
            int dias = prioridad switch
            {
                PrioridadOrden.Alta => OrdenDeServicioConstants.AcuerdoNivelDeServicioAlta,
                PrioridadOrden.Media => OrdenDeServicioConstants.AcuerdoNivelDeServicioMedia,
                PrioridadOrden.Baja => OrdenDeServicioConstants.AcuerdoNivelDeServicioBaja,
                _ => OrdenDeServicioConstants.SinAcuerdoNivelDeServicio
            };

            orden.CalcularFechaDeAtencionEstimada(dias);

            var updateResult = await _unitOfWork.OrdenesDeServicio.UpdateAsync(orden);
            int filasAfectadas = await _unitOfWork.SaveChangesAsync();

            if (filasAfectadas == 0)
            {
                return Result.Error("No se pudo actualizar la fecha estimada de la orden");
            }

            return Result.Success($"Fecha estimada actualizada correctamente a {orden.FechaDeAtencionEstimada:dd/MM/yyyy}");
        }

        #region Métodos privados

        /// <summary>
        /// Valida que el cliente exista y los servicios requeridos estén disponibles
        /// </summary>
        private async Task<Result<OrdenValidationResult>> ValidarClienteYServicios(CreateOrdenDeServicioRequest request)
        {
            // Validar que el cliente exista
            Console.WriteLine("[INFO] Validando cliente existente...");
            var clienteResult = await _unitOfWork.Clientes.GetByIdAsync(request.ClienteId);
            if (!clienteResult.IsSuccess || clienteResult.Value == null)
            {
                return Result<OrdenValidationResult>.Error("El cliente especificado no existe");
            }

            Cliente cliente = clienteResult.Value;

            // Validar que los servicios existan
            Console.WriteLine("[INFO] Validando servicios existentes...");
            List<Servicio> servicios = new();

            if (request.DetallesIds != null && request.DetallesIds.Any())
            {
                foreach (var servicioId in request.DetallesIds)
                {
                    var servicioResult = await _unitOfWork.Servicios.GetByIdAsync(servicioId);
                    if (!servicioResult.IsSuccess || servicioResult.Value == null)
                    {
                        return Result<OrdenValidationResult>.Error($"El servicio con ID '{servicioId}' no existe");
                    }
                    servicios.Add(servicioResult.Value);
                }
            }
            else
            {
                // Si no hay servicios especificados
                return Result<OrdenValidationResult>.Error("Debe especificar al menos un servicio para la orden");
            }

            return Result<OrdenValidationResult>.Success(new OrdenValidationResult(cliente, servicios), "Validación exitosa");
        }

        /// <summary>
        /// Método interno para agregar un detalle a una orden
        /// </summary>
        private async Task<Result> AgregarDetalleInterno(OrdenDeServicio orden, Guid servicioId)
        {
            // Verificar si el servicio ya existe en la orden
            if (orden.Detalles.Any(d => d.Servicio.Id == servicioId))
            {
                return Result.Warning("El servicio ya está incluido en la orden");
            }

            var servicioResult = await _unitOfWork.Servicios.GetByIdAsync(servicioId);
            if (!servicioResult.IsSuccess || servicioResult.Value == null)
            {
                return Result.Warning("Servicio no encontrado");
            }

            var detalle = new OrdenDeServicioDetalle(servicioResult.Value);
            orden.AgregarDetalle(detalle);

            return Result.Success("Servicio agregado correctamente a la orden");
        }

        /// <summary>
        /// Valida si una transición de estado es permitida
        /// </summary>
        private bool ValidarTransicionEstado(EstadosOrden estadoActual, EstadosOrden nuevoEstado)
        {
            return (estadoActual, nuevoEstado) switch
            {
                // De pendiente se puede pasar a cualquier estado
                (EstadosOrden.Pendiente, _) => true,

                // De en proceso solo se puede finalizar o cancelar
                (EstadosOrden.EnProceso, EstadosOrden.Finalizada) => true,
                (EstadosOrden.EnProceso, EstadosOrden.Cancelada) => true,

                // Una orden finalizada o cancelada no puede cambiar de estado
                (EstadosOrden.Finalizada, _) => false,
                (EstadosOrden.Cancelada, _) => false,

                // Por defecto no permitir
                _ => false
            };
        }

        /// <summary>
        /// Obtiene todas las órdenes de servicio con mapeo completo de entidades y enums 
        /// para ser usadas en aplicación cliente
        /// </summary>
        /// <param name="includeProperties">Propiedades relacionadas a incluir en la consulta</param>
        /// <returns>Objeto de respuesta estándar con datos de órdenes de servicio</returns>
        public async Task<object> GetAllOrderServiceAsync(string includeProperties = null)
        {
            try
            {
                // Obtener todas las órdenes con sus relaciones
                var result = await _unitOfWork.OrdenesDeServicio.GetAllAsync(
                    includeProperties: includeProperties ?? "Cliente,Tecnico,Detalles.Servicio");


                if (!result.IsSuccess)
                {
                    return new
                    {
                        success = false,
                        warning = result.IsWarning ? true : (bool?)null,
                        error = result.IsError ? true : (bool?)null,
                        info = result.IsInfo ? true : (bool?)null,
                        message = result.Message,
                        data = (object)null
                    };
                }

                // Realizar un mapeo más detallado con todos los campos requeridos
                var ordenesData = result.Value.Select(o => new
                {
                    // Información básica
                    id = o.Id,

                    // Información del cliente
                    cliente = new
                    {
                        id = o.Cliente?.Id,
                        publicoId = o.Cliente?.PublicoId,
                        nombre = o.Cliente?.Nombre ?? "Sin cliente",
                        tipoCliente = o.Cliente?.TipoCliente,
                        tipoClienteNombre = o.Cliente?.TipoClienteNombre
                    },

                    // Estado de la orden
                    publicoId = o.PublicoId,
                    estado = o.Estado.ToString(),
                    estadoId = (int)o.Estado,

                    // Fechas
                    fechaCreacion = o.FechaCreacion,
                    fechaCreacionReal = o.FechaCreacion,
                    fechaEstimada = o.FechaDeAtencionEstimada.ToString("dd/MM/yyyy"),
                    fechaEstimadaReal = o.FechaDeAtencionEstimada,
                    fechaReal = o.FechaDeAtencionReal.ToString("dd/MM/yyyy"),
                    fechaRealValue = o.FechaDeAtencionReal,

                    // Detalles de la orden
                    descripcionIncidencia = o.DescripcionIncidencia,
                    ubicacion = o.Ubicacion,

                    // Categoría de servicio
                    categoriaServicio = o.CategoriaServicio.ToString(),
                    categoriaServicioId = (int)o.CategoriaServicio,

                    // Prioridad
                    prioridad = o.Prioridad.ToString(),
                    prioridadId = (int)o.Prioridad,

                    // Información del técnico
                    tecnico = o.Tecnico != null ? new
                    {
                        id = o.Tecnico.Id,
                        cedula = o.Tecnico.Cedula,
                        nombre = o.Tecnico.Nombre,
                        apellido = o.Tecnico.Apellido,
                        nombreCompleto = o.Tecnico.NombreCompleto
                    } : null,

                    // Servicios asociados
                    servicios = o.Detalles.Select(d => new
                    {
                        id = d.Servicio.Id,
                        publicoId = d.Servicio.PublicoId,
                        nombre = d.Servicio.Nombre
                    }).ToList()


                }).ToList();

                // Construir respuesta
                var response = new
                {
                    success = true,
                    warning = (bool?)null,
                    error = (bool?)null,
                    info = (bool?)null,
                    message = "Órdenes de servicio obtenidas exitosamente",
                    data = ordenesData
                };

                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Error al obtener órdenes de servicio: {ex.Message}");

                return new
                {
                    success = false,
                    error = true,
                    warning = (bool?)null,
                    info = (bool?)null,
                    message = "Error interno al obtener las órdenes de servicio: " + ex.Message,
                    data = (object)null
                };
            }
        }


        #endregion
    }
}
