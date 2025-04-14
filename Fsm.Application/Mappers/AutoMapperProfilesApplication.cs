using AutoMapper;

namespace Fsm.Application.Mappers
{
    public class AutoMapperProfilesApplication : Profile
    {
        public AutoMapperProfilesApplication()
        {

            // Mapeo de CreateClienteRequest a Cliente
            CreateMap<Fsm.Application.DTOs.Cliente.CreateClienteRequest
                , Fsm.Domain.Entities.Cliente>();

            // Mapeo de CreateTecnicoRequest a Tecnico
            CreateMap<Fsm.Application.DTOs.Tecnico.CreateTecnicoRequest
                , Fsm.Domain.Entities.Tecnico>();

            // Mapeo de CreateServicoRequest a Servicio
            CreateMap<Fsm.Application.DTOs.Servicio.CreateServicioRequest
                , Fsm.Domain.Entities.Servicio>();


        }
    }
}
