using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BannerController : ControllerBase
    {
        #region variables
        private readonly BannerService _bannerService;
        private ILogger<BannerController> _logger;
        #endregion
        #region Constructor
        public BannerController(BannerService bannerService, ILogger<BannerController> logger)
        {
            _bannerService = bannerService;
            _logger = logger;
        }
        #endregion
        #region Metodos

        [HttpPut("BannerEditar")]
        public async Task<IActionResult> BannerEditar([FromForm]BannerRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _bannerService.EditarDatos(request);

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
        [HttpGet("BannerObtener")]
        public async Task<IActionResult> BannerObtener()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _bannerService.ObtenerBanner();

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return NoContent();
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
