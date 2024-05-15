
﻿using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;

﻿using _4toExpoApi.Core.Request;


using System.Linq.Expressions;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using System.Reflection;



namespace _4toExpoApi.Core.Services
{
   public class BolsaTrabajoService
    {
        #region <-----Variables----->
        private readonly IBaseRepository<BolsaTrabajo> _bolsaTrabajoRepository;
        private ILogger<BolsaTrabajoService> _logger;
        #endregion

        #region <-----Constructor----->
        public BolsaTrabajoService(IBaseRepository<BolsaTrabajo> bolsaTrabajoRepository, ILogger<BolsaTrabajoService> logger)
        {
            _bolsaTrabajoRepository = bolsaTrabajoRepository;
            _logger = logger;
        }
        #endregion

        #region <-----Metodos----->

        public async Task<GenericResponse>AgregarBolsaTrabajo(BolsaTrabajoRequest request, int IdUsuario)

        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = new GenericResponse();
                BolsaTrabajo bolsaTrabajo = new BolsaTrabajo
                {
                    Puesto = request.Puesto,
                    Tipo = request.Tipo,
                    Requisitos = request.Requisitos,
                    Descripcion = request.Descripcion,
                    DiasLaborales = request.DiasLaborales,
                    HoraInicio = TimeSpan.Parse(request.HoraInicio),
                    HoraFinal = TimeSpan.Parse(request.HoraFinal),
                    Ciudad = request.Ciudad,
                    Direccion = request.Direccion,


                    FechaAlt = HoraHelper.GetHora("mx"),
                    UserAlt = IdUsuario,
                    Activo = true

                };

                var result = await _bolsaTrabajoRepository.Add(bolsaTrabajo, _logger);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Bolsa de Trabajo agregada correctamente";
                    
                }
                else
                {
                    response.Success = false;
                    response.Message = "La Bolsa de Trabajo no se agrego correctamente";
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                throw;
            }
        }
        public async Task<GenericResponse>ActualizarBolsaTrabajo(BolsaTrabajoRequest request, int userMod)
        {
            try
            {

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse();
                var entity = await _bolsaTrabajoRepository.GetById(request.IdBolsaTrabajo, _logger);

                if(entity == null)
                {
                    response.Success = false;
                    response.Message = "La Bolsa de Trabajo no existe";
                    return response;

                }
                entity.Puesto = request.Puesto;
                entity.Tipo = request.Tipo;
                entity.Requisitos = request.Requisitos;
                entity.Descripcion = request.Descripcion;   
                entity.DiasLaborales = request.DiasLaborales;   
                entity.HoraInicio = TimeSpan.Parse( request.HoraInicio); 
                entity.HoraFinal = TimeSpan.Parse( request.HoraFinal);
                entity.Ciudad = request.Ciudad;
                entity.Direccion = request.Direccion;
                entity.UserUpd = userMod;
                entity.FechaUpd = HoraHelper.GetHora("mx");


                var result = await _bolsaTrabajoRepository.Update(entity,_logger);

                if (result != null)
                {
                    response.Success=true;
                    response.Message = "La Bolsa de Trabajo se actualizo correctamente";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No fue posible actualizar la Bolsa de Trabajo";
                }


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                throw;
            }
        }


        public async Task<GenericResponse>EliminarBolsaTrabajo(int Id, int userUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse();
                var entity = await _bolsaTrabajoRepository.GetById(Id, _logger);
                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "La Bolsa de Trabajo no existe";
                    return response;
                }
                entity.UserUpd = userUpd;
                entity.FechaUpd = HoraHelper.GetHora("mx");
                entity.Activo = false;
                 var result = await _bolsaTrabajoRepository.Update(entity, _logger);
                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Bolsa de Trabajo eliminada correctamente";


                }
                else
                {
                    response.Success= false;
                    response.Message = "Error al eliminar la Bolsa de Trabajo";

                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                return response;

        //    }
        //    catch (Exception ex)
        //    {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);


                throw;
            }
        }


        public async Task<ListResponse<BolsaTrabajoVM>> ObtenerBolsaTrabajo()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new ListResponse<BolsaTrabajoVM>();

                Expression<Func<BolsaTrabajo, bool>> expression = x => x.Activo == true;

                var result = await _bolsaTrabajoRepository.GetAll(_logger, [], expression);


                response.Total = result.Count();
                response.Data = result.Select(x => new BolsaTrabajoVM
                {
                    IdBolsaTrabajo = x.IdBolsaTrabajo,
                    Tipo = x.Tipo,
                    Descripcion = x.Descripcion,
                    Puesto = x.Puesto,
                    Requisitos = x.Requisitos,
                    DiasLaborales = x.DiasLaborales,
                    HoraInicio = x.HoraInicio.ToString(),
                    HoraFinal = x.HoraFinal.ToString(),
                    Ciudad = x.Ciudad,
                    Direccion = x.Direccion,
                    
                }).ToList();
                
               



                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                throw;
            }


        }

        #endregion


    }
}
