using AutoMapper;
using Fsm.Domain.Enums;

namespace Fsm.Adapters.Persistence.Mappers
{
    public class AutoMapperProfilesPersistence : Profile
    {
        public AutoMapperProfilesPersistence()
        {
            // Mapeo a Cliente
            CreateMap<Fsm.Domain.Entities.Cliente, Models.Cliente>().ReverseMap();

            // Mapeo a Tecnico
            CreateMap<Fsm.Domain.Entities.Tecnico, Models.Tecnico>().ReverseMap();

            // Mapeo Servicio
            CreateMap<Fsm.Domain.Entities.Servicio, Fsm.Adapters.Persistence.Models.Servicio>().ReverseMap();

            // Mapeo OrdenDeServicio (Dominio -> Persistencia)
            CreateMap<Fsm.Domain.Entities.OrdenDeServicio, Fsm.Adapters.Persistence.Models.OrdenDeServicio>()
               .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.Cliente.Id))
               .ForMember(dest => dest.TecnicoId, opt => opt.MapFrom(src => src.Tecnico != null ? src.Tecnico.Id : Guid.Empty))
               .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => (int)src.Estado))
               .ForMember(dest => dest.CategoriaServicio, opt => opt.MapFrom(src => (int)src.CategoriaServicio))
               .ForMember(dest => dest.Prioridad, opt => opt.MapFrom(src => (int)src.Prioridad))
               .ForMember(dest => dest.Cliente, opt => opt.Ignore())
               .ForMember(dest => dest.Tecnico, opt => opt.Ignore())
               .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));

            // Mapeo inverso OrdenDeServicio (Persistencia -> Dominio)
            CreateMap<Fsm.Adapters.Persistence.Models.OrdenDeServicio, Fsm.Domain.Entities.OrdenDeServicio>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => (EstadosOrden)src.Estado))
                .ForMember(dest => dest.CategoriaServicio, opt => opt.MapFrom(src => (CategoriaServicio)src.CategoriaServicio))
                .ForMember(dest => dest.Prioridad, opt => opt.MapFrom(src => (PrioridadOrden)src.Prioridad))
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Tecnico, opt => opt.Ignore())
                .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));

            // Mapeo OrdenDeServicioDetalle (Dominio -> Persistencia)
            CreateMap<Fsm.Domain.Entities.OrdenDeServicioDetalle, Fsm.Adapters.Persistence.Models.OrdenDeServicioDetalle>()
                  .ForMember(dest => dest.OrdenDeServicio, opt => opt.Ignore())
                  .ForMember(dest => dest.ServicioId, opt => opt.MapFrom(src => src.Servicio.Id))
                  .ForMember(dest => dest.Servicio, opt => opt.Ignore());

            // Mapeo inverso OrdenDeServicioDetalle (Persistencia -> Dominio)
            CreateMap<Fsm.Adapters.Persistence.Models.OrdenDeServicioDetalle, Fsm.Domain.Entities.OrdenDeServicioDetalle>()
                  .ForMember(dest => dest.Servicio, opt => opt.Ignore())
                  .ConstructUsing((src, ctx) => new Fsm.Domain.Entities.OrdenDeServicioDetalle(
                      ctx.Mapper.Map<Fsm.Domain.Entities.Servicio>(src.Servicio)));

            //// Mapeo OrdenDeServicio
            //CreateMap<Fsm.Domain.Entities.OrdenDeServicio, Fsm.Adapters.Persistence.Models.OrdenDeServicio>()
            //   .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.Cliente.Id))
            //   .ForMember(dest => dest.TecnicoId, opt => opt.MapFrom(src => src.Tecnico != null ? src.Tecnico.Id : Guid.Empty))
            //   .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => (int)src.Estado))
            //   .ForMember(dest => dest.CategoriaServicio, opt => opt.MapFrom(src => (int)src.CategoriaServicio))
            //   .ForMember(dest => dest.Prioridad, opt => opt.MapFrom(src => (int)src.Prioridad))
            //   .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            //   .ForMember(dest => dest.Tecnico, opt => opt.Ignore())
            //   .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));

            //// También necesitas el mapeo inverso
            //CreateMap<Fsm.Adapters.Persistence.Models.OrdenDeServicio, Fsm.Domain.Entities.OrdenDeServicio>()
            //    .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => (EstadosOrden)src.Estado))
            //    .ForMember(dest => dest.CategoriaServicio, opt => opt.MapFrom(src => (CategoriaServicio)src.CategoriaServicio))
            //    .ForMember(dest => dest.Prioridad, opt => opt.MapFrom(src => (PrioridadOrden)src.Prioridad))
            //    .ForMember(dest => dest.Cliente, opt => opt.Ignore())
            //    .ForMember(dest => dest.Tecnico, opt => opt.Ignore())
            //   .ForMember(dest => dest.Detalles, opt => opt.MapFrom(src => src.Detalles));



            //CreateMap<Fsm.Domain.Entities.OrdenDeServicioDetalle, Fsm.Adapters.Persistence.Models.OrdenDeServicioDetalle>()
            //      .ForMember(dest => dest.OrdenDeServicio, opt => opt.Ignore())
            //      .ForMember(dest => dest.ServicioId, opt => opt.MapFrom(src => src.Servicio.Id))
            //      .ForMember(dest => dest.Servicio, opt => opt.Ignore());





        }
    }
}
