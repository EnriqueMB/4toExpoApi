using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
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
    public class InformacionConfigService
    {
        #region <---VARIABLES--->
        private readonly IBaseRepository<InformacionConfig> _informacionRepository; 
        private ILogger<InformacionConfigService> _logger;
        #endregion

        #region <---CONSTRUCTOR--->
        public InformacionConfigService(IBaseRepository<InformacionConfig> informacionRepository, ILogger<InformacionConfigService> logger)
        {
            _informacionRepository = informacionRepository;
            _logger = logger; 
        }
        #endregion

        #region <---METODOS--->

        public async Task<InformacionConfigRequest> Index()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var informacionConfig = await _informacionRepository.GetById(1, _logger);

                var datosReq = AppMapper.Map<InformacionConfig, InformacionConfigRequest>(informacionConfig);

                return datosReq; 
            }
            catch(Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw; 
            }
        }

        public async Task<GenericResponse<InformacionConfigRequest>> EditarInformacion(InformacionConfigRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<InformacionConfigRequest>();
                var informacion = await _informacionRepository.GetById(request.Id, _logger);

                informacion.UserUpd = UserUpd;
                informacion.FechaUpd = DateTime.Now;

                var update = await _informacionRepository.Update(informacion, _logger);
                if (update != null)
                {
                    response.Success = true;
                    response.Message = "Se edito correctamente la Informacion.";
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se edito correctamente la Informacion.";
                }

                return response;
            }
            catch(Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message); 
                throw;
            }
        
        }




        #endregion
    }
}
