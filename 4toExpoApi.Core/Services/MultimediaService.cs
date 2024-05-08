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
    public class MultimediaService
    {
        private readonly IBaseRepository<Multimedia> _multimediaRepository;
        private readonly ILogger<MultimediaService> _logger;
        public MultimediaService(IBaseRepository<Multimedia> multimediaRepository, ILogger<MultimediaService> logger) {

            _multimediaRepository = multimediaRepository;
            _logger = logger;
        }

        
        public async Task<MultimediaRequest> Index()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var multimedia = await _multimediaRepository.GetById(1, _logger);

                var datosReq = AppMapper.Map<Multimedia, MultimediaRequest>(multimedia);

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return datosReq;
            }
            catch (Exception ex) {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<MultimediaRequest>> EditarMultimedia(MultimediaRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Se ha iniciado exitosamente");

                var response = new GenericResponse<MultimediaRequest>();
                var multimedia = await _multimediaRepository.GetById(request.Id, _logger);

                multimedia.Calidad = request.Calidad;
                multimedia.Resolucion = request.Resolucion;
                multimedia.FechaAlt = request.FechaAlt;
                multimedia.UserAlt = request.UserAlt;
                multimedia.FechaUpd = DateTime.Now;
                multimedia.Resolucion = request.Resolucion;
                multimedia.UserUpd = request.UserUpd;
                multimedia.IdTipo = request.IdTipo;

                var update = await _multimediaRepository.Update(multimedia, _logger);
                if (update != null)
                {
                    response.Message = "Se actualizó correctamente la multimedia";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "Algo ocurrió durante el proceso.";
                    response.Success = false;

                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Se ha finalizado exitosamente");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
    }
}
