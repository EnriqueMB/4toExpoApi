using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using AbogadosApiV1.Core.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadosController : ControllerBase
    {
        private readonly EstadosService _baseRepository;
        private ILogger<EstadosController> _logger;

        public EstadosController(EstadosService rolPermisoService, ILogger<EstadosController> logger)
        {
            _baseRepository = rolPermisoService;
            _logger = logger;
        }
        [HttpPost]
        [Route("Estado")]

        public async Task<IActionResult> Estado(PaginadoRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.Name + " - Started Success");

                var response = await _baseRepository.Estado(request);

                if (response != null)
                    return Ok(response);
                else
                    return BadRequest("No se encontró el Estado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name + " - " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");
            }

        }
        [HttpGet("ObtenerEstados")]
        public async Task<IActionResult> ObtenerEstados()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _baseRepository.ObtenerEstados();

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
