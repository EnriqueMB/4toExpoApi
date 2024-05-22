using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MultimediaController : ControllerBase
    {
        private readonly MultimediaServise _MultimediaService;
        private readonly ILogger<MultimediaController> _logger;

        public MultimediaController(MultimediaServise multimediaServise, ILogger<MultimediaController> logger)
        {
            _MultimediaService = multimediaServise;
            _logger = logger;
        }
        [HttpPut("UpdateMultimedia")]
        public async Task<IActionResult> UpdateMultimedia(CMultimedia request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _MultimediaService.UpdateMultimedia(request);

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
        [HttpGet("ObtenerMultimedia")]
        public async Task<IActionResult> ObtenerMultimedia()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _MultimediaService.ObtenerMultimedia();

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return Ok(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
    }
}
