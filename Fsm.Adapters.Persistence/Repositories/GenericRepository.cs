using AutoMapper;
using AutoMapper.QueryableExtensions;
using Fsm.Domain.Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Fsm.Adapters.Persistence.Repositories
{
    public abstract class GenericRepository<TDomain, TPersistence> 
        where TDomain : class
        where TPersistence : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly DbSet<TPersistence> _dbSet;

        protected GenericRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _dbSet = context.Set<TPersistence>();
        }

        /// <summary>
        /// Virtual async me permite sobreescribir el método en las clases que hereden de esta clase.   
        /// </summary>
      

        public virtual async Task<Result<TDomain>> GetByIdAsync(Guid id)
        {
            try
            {
                // Buscar la entidad en la base de datos por su Id
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return Result<TDomain>.Warning($"Entidad con ID {id} no encontrada.");

                // Mapear la entidad persistente al dominio
                var domainEntity = _mapper.Map<TDomain>(entity);
                return Result<TDomain>.Success(domainEntity, "Entidad encontrada exitosamente");
            }
            catch (Exception ex)
            {
                return Result<TDomain>.Error($"Error al obtener la entidad: {ex.Message}");
            }
        }

        public virtual async Task<Result<TDomain>> GetByPublicId(string publicoId)
        {
            try
            {
                var entity = await _dbSet.FirstOrDefaultAsync(e => EF.Property<string>(e, "PublicoId") == publicoId);
                if (entity == null)
                    return Result<TDomain>.Warning($"Entidad con PublicoId {publicoId} no encontrada.");
                var domainEntity = _mapper.Map<TDomain>(entity);
                return Result<TDomain>.Success(domainEntity, "Entidad encontrada exitosamente");
            }
            catch (Exception ex)
            {
                return Result<TDomain>.Error($"Error al obtener la entidad: {ex.Message}");
            }
        }

        public virtual async Task<Result<TDomain>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return Result<TDomain>.Warning($"Entidad con ID {id} no encontrada.");

                var domainEntity = _mapper.Map<TDomain>(entity);
                return Result<TDomain>.Success(domainEntity, "Entidad encontrada exitosamente");
            }
            catch (Exception ex)
            {
                return Result<TDomain>.Error($"Error al obtener la entidad: {ex.Message}");
            }
        }

        //public virtual async Task<Result<IEnumerable<TDomain>>> GetAllAsync(
        //   Expression<Func<TDomain, bool>> filter = null,
        //   Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>> orderBy = null,
        //   string includeProperties = null)
        //{
        //    // Construir la consulta a partir del DbSet de la entidad de persistencia.
        //    IQueryable<TPersistence> query = _dbSet;

        //    // Incluir propiedades de navegación si se especifica.
        //    if (!string.IsNullOrEmpty(includeProperties))
        //    {
        //        foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            query = query.Include(includeProperty);
        //        }
        //    }

        //    // Proyectar la consulta a la entidad de dominio
        //    var projectedQuery = query.ProjectTo<TDomain>(_mapper.ConfigurationProvider);

        //    // Aplicar filtro si se proporciona.
        //    if (filter != null)
        //    {
        //        projectedQuery = projectedQuery.Where(filter);
        //    }

        //    // Aplicar ordenamiento si se proporciona.
        //    if (orderBy != null)
        //    {
        //        projectedQuery = orderBy(projectedQuery);
        //    }

        //    var resultList = await projectedQuery.ToListAsync();

        //    return resultList.Any()
        //        ? Result<IEnumerable<TDomain>>.Success(resultList, "Registros obtenidos exitosamente")
        //        : Result<IEnumerable<TDomain>>.Info("No se encontraron registros.");
        //}
        public virtual async Task<Result<IEnumerable<TDomain>>> GetAllAsync(
    Expression<Func<TDomain, bool>> filter = null,
    Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>> orderBy = null,
    string includeProperties = null)
        {
            try
            {
                // Construir la consulta a partir del DbSet de la entidad de persistencia.
                IQueryable<TPersistence> query = _dbSet;

                // Incluir propiedades de navegación si se especifica.
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty.Trim());

                        // Si hay propiedades anidadas como "Detalles.Servicio", verificar explícitamente
                        if (includeProperty.Contains("."))
                        {
                            // Registrar la inclusión para depuración
                            Console.WriteLine($"Incluyendo propiedad anidada: {includeProperty}");
                        }
                    }
                }

                // Convertir las entidades de persistencia a entidades de dominio
                var domainEntities = await query.AsNoTracking()
                    .Select(e => _mapper.Map<TDomain>(e))
                    .ToListAsync();

                return domainEntities.Any()
                    ? Result<IEnumerable<TDomain>>.Success(domainEntities, "Registros obtenidos exitosamente")
                    : Result<IEnumerable<TDomain>>.Info("No se encontraron registros.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAllAsync: {ex.Message}");
                return Result<IEnumerable<TDomain>>.Error($"Error al obtener los registros: {ex.Message}");
            }
        }

        public virtual async Task<Result<TDomain>> FirstOrDefaultAsync(
        Expression<Func<TDomain, bool>>? filter = null,
        Func<IQueryable<TDomain>, IOrderedQueryable<TDomain>>? orderBy = null,
        string? includeProperties = null)
            {
                // Start with the persistence query.
                IQueryable<TPersistence> query = _dbSet;

                // Include navigation properties if specified.
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                // Project the query to the domain type.
                var projectedQuery = query.ProjectTo<TDomain>(_mapper.ConfigurationProvider);

                // Apply filter if provided.
                if (filter != null)
                {
                    projectedQuery = projectedQuery.Where(filter);
                }

                // Apply custom ordering if provided.
                if (orderBy != null)
                {
                    projectedQuery = orderBy(projectedQuery);
                }

                // Retrieve the first or default entity.
                var entity = await projectedQuery.FirstOrDefaultAsync();

                // Return a warning if not found.
                if (entity == null)
                {
                    return Result<TDomain>.Warning("Registro no encontrado.");
                }

                return Result<TDomain>.Success(entity, "Registro obtenido exitosamente");
            }


        public virtual async Task<Result<IEnumerable<TDomain>>> GetAllAsync()
        {
            try
            {
                var entities = await _dbSet.ToListAsync();
                if (!entities.Any())
                    return Result<IEnumerable<TDomain>>.Warning("No se encontraron registros.");

                var domainEntities = _mapper.Map<IEnumerable<TDomain>>(entities);
                return Result<IEnumerable<TDomain>>.Success(domainEntities, "Registros obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<TDomain>>.Error($"Error al obtener los registros: {ex.Message}");
            }
        }

        public virtual async Task<Result> AddAsync(TDomain domainEntity)
        {
            try
            {
                var persistenceEntity = _mapper.Map<TPersistence>(domainEntity);
                await _dbSet.AddAsync(persistenceEntity);
                return Result.Success("Registro agregado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al agregar el registro: {ex.Message}");
            }
        }

        public virtual Result Update(TDomain domainEntity)
        {
            try
            {
                var persistenceEntity = _mapper.Map<TPersistence>(domainEntity);
                _dbSet.Update(persistenceEntity);
                return Result.Success("Registro actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al actualizar el registro: {ex.Message}");
            }
        }

        public virtual async Task<Result> UpdatePartialAsync(Guid id, Dictionary<string, object> propertiesToUpdate)
        {
            try
            {
                
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                {
                    return Result.Warning($"Registro con ID {id} no encontrado.");
                }

             
                var entityEntry = _context.Entry(entity);

               
                foreach (var property in propertiesToUpdate)
                {
                  
                    var propertyInfo = typeof(TPersistence).GetProperty(property.Key);
                    if (propertyInfo == null)
                    {
                        
                        continue;
                    }

                    
                    propertyInfo.SetValue(entity, property.Value);

                    // Mark only this property as modified
                    entityEntry.Property(property.Key).IsModified = true;
                }

               
                return Result.Success("Propiedades actualizadas correctamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al actualizar parcialmente el registro: {ex.Message}");
            }
        }

        public virtual async Task<Result> DeleteAsync(int id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return Result.Warning($"Registro con ID {id} no encontrado.");

                _dbSet.Remove(entity);
                return Result.Success("Registro eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al eliminar el registro: {ex.Message}");
            }
        }


        public virtual async Task<Result> DeleteAsync(TDomain domainEntity)
        {
            try
            {
               
                var persistenceEntity = _mapper.Map<TPersistence>(domainEntity);

               
                var keyProperty = typeof(TPersistence).GetProperty("Id");
                if (keyProperty == null)
                {
                    return Result.Error("No se pudo determinar la clave primaria de la entidad.");
                }

                
                var keyValue = keyProperty.GetValue(persistenceEntity);
                if (keyValue == null)
                {
                    return Result.Error("La clave primaria es nula.");
                }

              
                var existingEntity = await _dbSet.FindAsync(keyValue);
                if (existingEntity == null)
                {
                    return Result.Warning("Registro no encontrado.");
                }

              
                _dbSet.Remove(existingEntity);
                return Result.Success("Registro eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al eliminar el registro: {ex.Message}");
            }
        }


        public virtual async Task<Result> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _dbSet.FindAsync(id);
                if (entity == null)
                    return Result.Warning($"Registro con ID {id} no encontrado.");
                _dbSet.Remove(entity);
                return Result.Success("Registro eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al eliminar el registro: {ex.Message}");
            }
        }


        public virtual async Task<Result> UpdateAsync(TDomain domainEntity)
        {
            try
            {
                // Mapear la entidad de dominio a la entidad de persistencia.
                var persistenceEntity = _mapper.Map<TPersistence>(domainEntity);

                // Obtener la propiedad que representa la clave primaria (asumiendo que se llama "Id").
                var keyProperty = typeof(TPersistence).GetProperty("Id");
                if (keyProperty == null)
                {
                    return Result.Error("No se pudo determinar la clave primaria de la entidad.");
                }

                var keyValue = keyProperty.GetValue(persistenceEntity);
                if (keyValue == null)
                {
                    return Result.Error("La clave primaria es nula.");
                }

                // Verificar que la entidad exista en la base de datos.
                var existingEntity = await _dbSet.FindAsync(keyValue);
                if (existingEntity == null)
                {
                    return Result.Warning("Registro no encontrado.");
                }

                // Actualizar la entidad existente con los nuevos valores.
                _context.Entry(existingEntity).CurrentValues.SetValues(persistenceEntity);
                return Result.Success("Registro actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Error($"Error al actualizar el registro: {ex.Message}");
            }
        }

    }
}