using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;

using _4toExpoApi.Core.Request;


using System.Linq.Expressions;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Entities;
using Microsoft.Extensions.Logging;
using System.Reflection;



namespace _4toExpoApi.Core.Services
{
    public class ProgramaActividadesService
    {
        #region <-----Variables----->
        private readonly IBaseRepository<ProgramaActividades> _programaActividadesRepository;
        private ILogger<ProgramaActividadesService> _logger;
        #endregion

        #region <-----Constructor----->
        public ProgramaActividadesService(IBaseRepository<ProgramaActividades> programaActividadesRepository, ILogger<ProgramaActividadesService> logger)
        {
            _programaActividadesRepository = programaActividadesRepository;
            _logger = logger;
        }
        #endregion

        #region <-----Metodos----->

        public async Task<GenericResponse> AgregarProgramaActividades(ProgramaActividadesRequest request, int idUsuario)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = new GenericResponse();
                ProgramaActividades programaActividades = new ProgramaActividades
                {
                    Orden = request.Orden,
                    Nombre = request.Nombre,
                    Fecha = request.Fecha,
                    HoraInicio = request.HoraInicio,
                    HoraFinal = request.HoraFinal,
                    Detalles = request.Detalles,
                    
                    FechaAlt = HoraHelper.GetHora("mx"),
                    UserAlt = idUsuario,
                    Activo = true

                };
                var result = await _programaActividadesRepository.Add(programaActividades, _logger);

                if (result != null)
                {
                    response.Success = true;
                    response.Message = "Programa de Actividades agregada correctamente";

                }
                else
                {
                    response.Success = false;
                    response.Message = "El Programa de Actividades no se agrego correctamente";
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Ended Success");

                return response;


            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                throw;
            }
        }
        public async Task<GenericResponse> ActualizarProgramaActividades(ProgramaActividadesRequest request, int userMod)
        {
            try
            {

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var entity = await _programaActividadesRepository.GetById(request.IdProgramaActividades, _logger);

                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "El Programa de Actividad no existe";
                    return response;

                }
                entity.Orden = request.Orden;
                entity.Nombre = request.Nombre;
                entity.Fecha = request.Fecha;
                entity.HoraInicio = request.HoraInicio;
                entity.HoraFinal = request.HoraFinal;
                entity.Detalles = request.Detalles;

                entity.UserUpd = userMod;
                entity.FechaUpd = HoraHelper.GetHora("mx");


                var result = await _programaActividadesRepository.Update(entity, _logger);

                if (result != null)
                {
                    response.Success = true;
                    response.Message = "El Programa de Actividades se actualizo correctamente";

                }
                else
                {
                    response.Success = false;
                    response.Message = "No fue posible actualizar El Programa de Actividades";
                }


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Ended Success");
                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                throw;
            }
        }


        public async Task<GenericResponse> EliminarProgramaActividades(int Id, int userUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var entity = await _programaActividadesRepository.GetById(Id, _logger);

                if (entity == null)
                {
                    response.Success = false;
                    response.Message = "El Programa de Actividades no existe";
                    return response;
                }
                entity.UserUpd = userUpd;
                entity.FechaUpd = HoraHelper.GetHora("mx");
                entity.Activo = false;

                var result = await _programaActividadesRepository.Update(entity, _logger);

                if (result != null)

                {
                    response.Success = true;
                    response.Message = "Programa de Actividades eliminada correctamente";

                }
                else
                {
                    response.Success = false;
                    response.Message = "Error al eliminar Programa de Actividades";

                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Ended Success");
                return response;

            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);


                throw;
            }
        }

        public async Task<ListResponse<ProgramaActividadesVM>> ObtenerProgramaActividades()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                
                var response = new ListResponse<ProgramaActividadesVM>();

                Expression<Func<ProgramaActividades, bool>> expression = x => x.Activo == true;

                var result = await _programaActividadesRepository.GetAll(_logger, [], expression);


                response.Total = result.Count();

                response.Data = result.Select(x => new ProgramaActividadesVM
                {
                    IdProgramaActividades = x.IdProgramaActividades,
                    Orden = x.Orden,
                    Nombre = x.Nombre,
                    Fecha = x.Fecha,
                    HoraInicio = x.HoraInicio,
                    HoraFinal = x.HoraFinal,
                    Detalles = x.Detalles,
                    

                }).ToList();

                response.Message = "Programa de Actividades obtenidas correctamente";

                response.Success = true;



                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Ended Success");

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