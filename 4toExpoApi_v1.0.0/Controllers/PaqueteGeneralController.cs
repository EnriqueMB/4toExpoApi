using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using Azure;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaqueteGeneralController : Controller
    {
        #region <--Variables-->
        private ILogger<PaqueteGeneralController> _logger;
        private readonly PaqueteGeneralService _paqueteGeneralService;
        #endregion

        #region <--Constructor-->

        public PaqueteGeneralController(ILogger<PaqueteGeneralController> logger,PaqueteGeneralService paqueteGeneralService)
        {
            _logger = logger;
            _paqueteGeneralService= paqueteGeneralService;
        }
        #endregion

        #region <--Metodos-->

        [HttpPost("AgregarPaqueteGeneral")]
        public async Task<IActionResult> AgregarPaquete(PaqueteGeneralRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _paqueteGeneralService.AgregarPaquetesGeneral(request,1);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        [HttpPut("EditarPqueteGeneral")]
        public async Task<IActionResult> EditarPaquete(PaqueteGeneralRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                
                var response = await _paqueteGeneralService.EditarPaqueteGeneral(request, 1);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return BadRequest(response);
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
