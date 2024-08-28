using _4toExpoApi.Core.Services;
using AbogadosApiV1.Core.Request;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PaquetesFreeController : Controller
    {
        #region VARIABLES
        private readonly PaquetesFreeService _service;
        private readonly ILogger<PaquetesFreeController> _logger;
        #endregion

        #region CONSTRUCTOR
        public PaquetesFreeController(PaquetesFreeService service, ILogger<PaquetesFreeController> logger)
        {
            _service = service;
            _logger = logger;
        }
        #endregion

        #region MÉTODOS
        [HttpPost("GetFreePackageUsers")]
        public async Task<IActionResult> GetFreePackageUsers(PaginadoRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = await _service.GetFreePackageUsers(request);

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
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpPost("ConfirmarCompra")]
        public async Task<IActionResult> ConfirmarCompra(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var user = HttpContext.User.Claims.Where(x => x.Type == "Id").FirstOrDefault()!.Value;

                var response = await _service.ConfirmarCompra(id,int.Parse(user));

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
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }
        #endregion
    }
}
