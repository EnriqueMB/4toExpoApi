using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Mappers
{
    public class AppMapper
    {
        private static readonly Mapper _mapper;
        static AppMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UsuariosVM, Usuarios>().ReverseMap();
                cfg.CreateMap<UsuarioRequest, Usuarios>().ReverseMap();
                cfg.CreateMap<PermisosVM, Permisos>().ReverseMap();
                cfg.CreateMap<UsuarioPermisosVM, UsuarioPermisos>().ReverseMap();
                cfg.CreateMap<UsuariosResponse, Usuarios>().ReverseMap();
                cfg.CreateMap<ReservaRequest, Reservas>().ReverseMap();
                cfg.CreateMap<PagoRequest, Pagos>().ReverseMap();
                cfg.CreateMap<ServicioRequest, Servicios>().ReverseMap();
                cfg.CreateMap<Servicios, ServicioRequest>().ReverseMap();
                cfg.CreateMap<BeneficioPaqueteRequest, BeneficioPaquete>().ReverseMap();
                cfg.CreateMap<BeneficioPaquete, BeneficioPaqueteRequest > ().ReverseMap();
                cfg.CreateMap<PaquetePatrocinadoresRequest, PaquetePatrocinadores>().ReverseMap();
                cfg.CreateMap<ContadorRequets, Contador>().ReverseMap();
            });

            _mapper = new Mapper(config);
        }

        public static TDestination Map<TSource, TDestination>(TSource obj)
        {
            return _mapper.Map<TSource, TDestination>(obj);
        }
    }
}
