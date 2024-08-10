using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers

{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BolsaTrabajoController : ControllerBase
    {
        #region<-----Variable----->
        private readonly BolsaTrabajoService _bolsaTrabajoService;
        private ILogger<BolsaTrabajoController> _logger;
        private readonly IPatrocinadoresRepository _patrocinadorRepository;
        #endregion


        #region<-----Constructor----->
        public BolsaTrabajoController(BolsaTrabajoService bolsaTrabajoService, ILogger<BolsaTrabajoController> logger, IPatrocinadoresRepository patrocinadorRepository)
        {
            _bolsaTrabajoService = bolsaTrabajoService;
            _logger = logger;
            _patrocinadorRepository = patrocinadorRepository;
        }
        #endregion

        #region<-----Metodos----->

        //Agregar Bolsa trabajo
        [HttpPost]
        [Route("AgregarBolsaTrabajo")]
        
        public async Task<IActionResult> AgregarBolsaTrabajo(BolsaTrabajoRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

               /* var IdUseralta = "1";*/ /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

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

                var response = await _bolsaTrabajoService.AgregarBolsaTrabajo( request, userAlt, idPatrocinador);

                if (response.Success)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        //Actualizar BolsaTrabajo
        [HttpPut]
        [Route("ActualizarBolsaTrabajo")]
        
        public async Task<IActionResult> ActualizarBolsaTrabajo(BolsaTrabajoRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                /*var idUsuario = "1";*/ /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuario no autenticado");
                }

                if (!int.TryParse(userIdClaim, out int userUpd))
                {
                    return BadRequest("ID de usuario inválido");
                }

                var response = await _bolsaTrabajoService.ActualizarBolsaTrabajo(request, userUpd);

                if (response.Success)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("ObtenerBolsaTrabajo")]
        public async Task<IActionResult> ObtenerBolsaTrabajo()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    // El usuario no está logueado, retornar un error
                    return Unauthorized("Usuario no autenticado");
                }
                int userAlt = int.Parse(userId);

                // Buscar el patrocinador asociado al usuario logueado
                var patrocinador = await _patrocinadorRepository.GetByUserIdAsync(userAlt);
                if (patrocinador == null)
                {
                    // El usuario no tiene un patrocinador asociado, retornar un error
                    return BadRequest("El usuario no tiene un patrocinador asociado");
                }
                int idPatrocinador = patrocinador.Id;

                var response = await _bolsaTrabajoService.ObtenerBolsaTrabajo(idPatrocinador);

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

        [HttpGet("ObtenerBolsaTrabajoPorId")]
        public async Task<IActionResult> ObtenerBolsaTrabajoPorId(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _bolsaTrabajoService.ObtenerBolsaTrabajoPorId(id);

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
        //Eliminar Bolsa Trabajo
        [HttpDelete]
        [Route("EliminarBolsaTrabajo")]
        
        public async Task<IActionResult> EliminarBolsaTrabajo(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var idUsuario = "1"; /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var response = await _bolsaTrabajoService.EliminarBolsaTrabajo(id, int.Parse(idUsuario));

                if (response.Success)
                    return Ok(response);
                else
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ObtenerBolsaTrabajoPorIdPatrocinador")]
        public async Task<IActionResult> ObtenerBolsaTrabajoId(int idPatrocinador)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                // Llamar al servicio para obtener los servicios filtrados por el ID del patrocinador
                var response = await _bolsaTrabajoService.ObtenerBolsaTrabajoIdPat(idPatrocinador);

                if (response != null && response.Any())
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                    return Ok(response); // Devuelve la lista de servicios
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return NotFound("No se encontraron servicios para el patrocinador proporcionado.");
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }

        #endregion


    }
}
