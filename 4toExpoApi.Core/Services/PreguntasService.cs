using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class PreguntasService
    {
        #region <---Variables--->
        private readonly IBaseRepository<Preguntas> _preguntasRepository;
        private ILogger<PreguntasService> _logger;
        #endregion

        #region <---Constructor--->
        public PreguntasService(IBaseRepository<Preguntas> preguntasRepository, ILogger<PreguntasService> logger)
        {
            _preguntasRepository = preguntasRepository;
            _logger = logger;
        }
        #endregion
        #region <---Metodos--->

        public async Task<GenericResponse<PreguntasRequest>> AgregarPregunta(PreguntasRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PreguntasRequest>();

                var addPregunta = AppMapper.Map<PreguntasRequest, Preguntas>(request);

                addPregunta.FechaAlt = DateTime.Now;
                addPregunta.UserAlt = userAlt;
                addPregunta.Activo = true;

                var add = await _preguntasRepository.Add(addPregunta, _logger);
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

        public async Task<List<PreguntasRequest>> ObtenerPreguntas()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listPreguntas = await _preguntasRepository.GetAll(_logger);

                if (listPreguntas == null || listPreguntas.Count() == 0)
                {
                    return null;
                }
                var listaPreguntasFiltrada = listPreguntas.Where(x => x.Activo == true).ToList();

                var requestListPreguntas = listaPreguntasFiltrada.Select(pregunta => AppMapper.Map<Preguntas, PreguntasRequest>(pregunta)).ToList();


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return requestListPreguntas;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<PreguntasRequest>> EditarPregunta(PreguntasRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PreguntasRequest>();
                var pregunta = await _preguntasRepository.GetById(request.Id, _logger);
                if (pregunta == null)
                {
                    response.Message = "El servicio no existe";
                    return response;
                }
                pregunta.Pregunta = request.Pregunta;
                pregunta.Descripcion = request.Descripcion;
                
                pregunta.UserUpd = UserUpd;
                pregunta.FechaUpd = DateTime.Now;
                var update = await _preguntasRepository.Update(pregunta, _logger);
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
        public async Task<GenericResponse<PreguntasRequest>> EliminarPregunta(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PreguntasRequest>();
                var pregunta = await _preguntasRepository.GetById(id, _logger);
                if (pregunta == null)
                {
                    response.Message = "El servicio no existe";
                    return response;
                }

                pregunta.Activo = false;


                var update = await _preguntasRepository.Update(pregunta, _logger);
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
