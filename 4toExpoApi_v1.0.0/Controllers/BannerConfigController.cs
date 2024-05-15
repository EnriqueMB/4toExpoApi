using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BannerConfigController : ControllerBase
    {
        #region <---Variables--->
        private readonly BannerConfigService _bannerConfigService;
        private readonly ILogger<BannerConfigController> _logger;
        #endregion
        #region <---Constructor--->
        public BannerConfigController(BannerConfigService bannerConfigService, ILogger<BannerConfigController> logger)
        {
            _bannerConfigService = bannerConfigService;
            _logger = logger;
        }


        [HttpGet("ObtenerBannerConfig/{id}")]
        public async Task<IActionResult> ObtenerBannerConfig(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = await _bannerConfigService.ObtenerBannerConfig(id);

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return Ok(response); // Devuelve una respuesta 200 OK con los datos
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                return StatusCode(500, "Ha ocurrido un error" + ex.Message); // Devuelve un error 500 en caso de excepción
            }
        }



        [HttpGet("ObtenerTodosLosBanners")]
        public async Task<IActionResult> ObtenerTodosLosBanners()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = await _bannerConfigService.ObtenerTodosLosBanners();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                if (response == null || response.Count == 0)
                {
                    return NoContent(); // Devuelve un código 204 No Content si no hay datos
                }

                return Ok(response); // Devuelve una respuesta 200 OK con los datos
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                return StatusCode(500, "Ha ocurrido un error" + ex.Message); // Devuelve un error 500 en caso de excepción
            }
        }


        [HttpPut("EditarBannerConfig")]
public async Task<IActionResult> EditarBannerConfig(BannerConfigRequest request)
{
    try
    {
        _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

        var response = await _bannerConfigService.EditarBannerConfig(request, 1);

        if (response.Success)
        {
            _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

            return Ok(response);
        }

        if (response.Message == "El Banner no existe")
        {
            _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

            return NotFound("No se encontró el banner");
        }

        _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

        return BadRequest(response);
    }
    catch (Exception ex)
    {
        _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
        return StatusCode(500, "Ha ocurrido un error" + ex.Message);
    }
}





        [HttpDelete("EliminarBannerConfig")]
        public async Task<IActionResult> EliminarBannerConfig(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = await _bannerConfigService.EliminarBannerConfig(id);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");
                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished with BadRequest");
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }



        #endregion

    }
}