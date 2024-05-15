using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PatrocinadorController : Controller
    {

        #region <---Variables--->
        private readonly PatrocinadorService _patrocinadorService;
        private readonly ILogger<PatrocinadorController> _logger;
        #endregion
        #region <---Constructor--->
        public PatrocinadorController(PatrocinadorService patrocinadorService, ILogger<PatrocinadorController> logger)
        {
            _patrocinadorService = patrocinadorService;
            _logger = logger;
        }
        #endregion

        [HttpPost("AgregarPatrocinador")]
        public async Task<IActionResult> AgregarPatrocinador([FromForm] PatrocinadorRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

            
                var response = await _patrocinadorService.AgregarPatrocinador(request, 1);

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

        [HttpGet("ObtenerPatrocinador")]
        public async Task<IEnumerable> ObtenerPatrocinador()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _patrocinadorService.ObtenerPatrocinador();

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
        [HttpPut("EditarPatrocinador")]
        public async Task<IActionResult> EditarPatrocinador(PatrocinadorRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _patrocinadorService.EditarPatrocinador(request, 1);

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
        [HttpDelete("EliminarPatrocinador")]
        public async Task<IActionResult> EliminarPatrocinador(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _patrocinadorService.EliminarPatrocinador(id);

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
       
    }
}
