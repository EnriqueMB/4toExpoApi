using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InformacionController : Controller
    {
        #region <---Variables--->
        private readonly InformacionService _informacionService;
        private readonly ILogger<InformacionController> _logger;
        #endregion

        #region <---Constructor--->
        public InformacionController(InformacionService informacionService, ILogger<InformacionController> logger)
        {
            _informacionService = informacionService;
            _logger = logger;
        }

        [HttpPut("EditarInformacion")]
        public async Task<IActionResult> EditarInformacion([FromForm] InformacionRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = await _informacionService.EditarInformacion(request);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        [HttpGet("ObtenerInformacion")]
        public async Task<IActionResult> ObtenerInformacion()
        {
            try
            {
                var response = await _informacionService.Informacion();

                if (response != null)
                {
                    return Ok(response);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
    }
}
