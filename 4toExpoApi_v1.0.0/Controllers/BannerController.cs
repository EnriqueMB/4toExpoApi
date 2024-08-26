using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.IRepositories;
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
        private readonly IPatrocinadoresRepository _patrocinadorRepository;

        #endregion
        #region Constructor
        public BannerController(BannerService bannerService, ILogger<BannerController> logger, IPatrocinadoresRepository patrocinadoresRepository)
        {
            _bannerService = bannerService;
            _logger = logger;
            _patrocinadorRepository = patrocinadoresRepository;
        }
        #endregion
        #region Metodos

        [HttpPost("BannerAgregar")]
        public async Task<IActionResult> BannerAgregar([FromForm] BannerRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuario no autenticado");
                }

                if (!int.TryParse(userIdClaim, out int userAlt))
                {
                    return BadRequest("ID de usuario inválido");
                }

                var patrocinador = await _patrocinadorRepository.GetByUserIdAsync(userAlt);
                if (patrocinador == null)
                {
                    return BadRequest("El usuario no tiene un patrocinador asociado");
                }
                int idPatrocinador = patrocinador.Id;
                request.IdPatrocinador = idPatrocinador;

                var response = await _bannerService.AgregarBanner(request);

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

        [HttpPut("BannerEditar")]
        public async Task<IActionResult> BannerEditar([FromForm] BannerRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuario no autenticado");
                }

                if (!int.TryParse(userIdClaim, out int userAlt))
                {
                    return BadRequest("ID de usuario inválido");
                }

                var patrocinador = await _patrocinadorRepository.GetByUserIdAsync(userAlt);
                if (patrocinador == null)
                {
                    return BadRequest("El usuario no tiene un patrocinador asociado");
                }
                int idPatrocinador = patrocinador.Id;

                request.IdPatrocinador = idPatrocinador;

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
        public async Task<IActionResult> BannerObtener(int idPat)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuario no autenticado");
                }

                if (!int.TryParse(userIdClaim, out int userAlt))
                {
                    return BadRequest("ID de usuario inválido");
                }

                var patrocinador = await _patrocinadorRepository.GetByUserIdAsync(userAlt);
                if (patrocinador == null)
                {
                    return BadRequest("El usuario no tiene un patrocinador asociado");
                }
                int idPatrocinador = patrocinador.Id;
                idPat = idPatrocinador;
                var response = await _bannerService.ObtenerBanner(idPat);

                if (response.Id > 0)
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
