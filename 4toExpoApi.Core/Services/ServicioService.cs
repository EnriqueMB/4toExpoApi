using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace _4toExpoApi.Core.Services
{
    public class ServicioService
    {
        #region <---Variables--->
        private readonly IBaseRepository<Servicios> _serviciosRepository;
        private ILogger<ServicioService> _logger;
        #endregion
        #region <---Constructor--->
        public ServicioService(IBaseRepository<Servicios> serviciosRepository, ILogger<ServicioService> logger)
        {
            _serviciosRepository = serviciosRepository;
            _logger = logger;
        }
        #endregion
        #region <---Metodos--->

        public async Task<GenericResponse<ServicioRequest>> AgregarServicio(ServicioRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ServicioRequest>();

                var addServicio = AppMapper.Map<ServicioRequest, Servicios>(request);

                addServicio.FechaAlt = DateTime.Now;
                addServicio.UserAlt = userAlt;
                addServicio.Activo = true;

                var add = await _serviciosRepository.Add(addServicio, _logger);
                if (add != null && add.Id > 0)
                {
                    response.Data = request;
                    response.Message = "Se agrego correctamente el servicio";
                    response.Success = true;
                    response.CreatedId = add.Id.ToString();
                }
                else
                {
                    response.Data = request;
                    response.Message = "No se pudo agregar correctamente el servicio";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<List<ServicioRequest>> ObtenerServicios()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listServicios = await _serviciosRepository.GetAll(_logger);

                if (listServicios == null || listServicios.Count() == 0)
                {
                    return null;
                }
                var listaServiciosFiltrada = listServicios.Where(x => x.Activo == true).ToList();

                var requestListServicios = listaServiciosFiltrada.Select(servicio => AppMapper.Map<Servicios, ServicioRequest>(servicio)).ToList();
               

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return requestListServicios;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<ServicioRequest>> EditarServicios(ServicioRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ServicioRequest>();
                var servicio = await _serviciosRepository.GetById(request.Id, _logger);
                if (servicio == null)
                {
                    response.Message = "El servicio no existe";
                    return response;
                }
                servicio.Servicio = request.Servicio;
                servicio.Descripcion = request.Descripcion;
                servicio.DiasLaborales = request.DiasLaborales;
                servicio.HoraInicio = request.HoraInicio;
                servicio.HoraFinal = request.HoraFinal;
                servicio.IdPatrocinador = request.IdPatrocinador;
                servicio.UserUpd = UserUpd;
                servicio.FechaUpd = DateTime.Now;
                var update = await _serviciosRepository.Update(servicio, _logger);
                if (update != null)
                {
                    response.Message = "Se edito correctamente el servicio";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se edito correctamente el servicio";
                    response.Success = false;

                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<ServicioRequest>> EliminarServicio(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ServicioRequest>();
                var servicio = await _serviciosRepository.GetById(id, _logger);
                if (servicio == null)
                {
                    response.Message = "El servicio no existe";
                    return response;
                }

                servicio.Activo = false;


                var update = await _serviciosRepository.Update(servicio, _logger);
                if (update != null)
                {
                    response.Message = "Se elimino correctamente el servicio";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se elimino correctamente el servicio";
                    response.Success = false;

                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
       
        #endregion

    }
}
