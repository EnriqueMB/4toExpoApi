using _4toExpoApi.Core.Services;
using _4toExpoApi.Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaquetePatrocinadoresController : ControllerBase
    {
        #region <---Varibales--->
        private readonly PaquetePatrocinadorService _paquetePatrocinadorService;
        private readonly ILogger<PaquetePatrocinadoresController> _logger;
        #endregion
        #region <---Constructor--->
        public PaquetePatrocinadoresController(PaquetePatrocinadorService paquetePatrocinadorService, ILogger<PaquetePatrocinadoresController> logger)
        {
            _paquetePatrocinadorService = paquetePatrocinadorService;
            _logger = logger;
        }
        #endregion
        #region <---Metodos--->
        [HttpPost]
        public async Task<IActionResult> AgregarPaquete(PaquetePatrocinadoresVM paqueteVM)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _paquetePatrocinadorService.AgregarPaquete(paqueteVM, 1);

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
        [HttpGet("ObtenerPaquetes")]
        public async Task<List<PaqueteBeneficiosPaVM>> ObtenerPaquetes()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _paquetePatrocinadorService.ObtenerPaquetes();

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;

            }
        }
        [HttpPut("EditarPaquete")]
        public async Task<IActionResult> EditarPaquete(PaquetePatrocinadoresVM paqueteVM)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _paquetePatrocinadorService.EditarPaquete(paqueteVM, 1);

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
