using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;
using System.Security.Claims;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ServiciosController : ControllerBase
    {
        #region <---Variables--->
        private readonly ServicioService _servicioService;
        private readonly ILogger<ServiciosController> _logger;
        private readonly IPatrocinadoresRepository _patrocinadorRepository;
        #endregion
        #region <---Constructor--->
        public ServiciosController(ServicioService servicioService, ILogger<ServiciosController> logger, IPatrocinadoresRepository patrocinadorRepository)
        {
            _servicioService = servicioService;
            _logger = logger;
            _patrocinadorRepository = patrocinadorRepository;
        }
        #endregion
        #region <---Metodos--->

        [HttpPost("AgregarServicio")]
        public async Task<IActionResult> AgregarServicio(ServicioRequest request)
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

                var response = await _servicioService.AgregarServicio(request, userAlt, idPatrocinador);

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

        [HttpGet("ObtenerServicios")]
        public async Task<IEnumerable> ObtenerServicios()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return (IEnumerable)Unauthorized("Usuario no autenticado");
                }
                int userAlt = int.Parse(userId);

                var patrocinador = await _patrocinadorRepository.GetByUserIdAsync(userAlt);
                if (patrocinador == null)
                {
                    return (IEnumerable)BadRequest("El usuario no tiene un patrocinador asociado");
                }
                int idPatrocinador = patrocinador.Id;

                var response = await _servicioService.ObtenerServicios(idPatrocinador);

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
        [HttpPut("EditarServicio")]
        public async Task<IActionResult> EditarServicio(ServicioRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuario no autenticado");
                }

                if (!int.TryParse(userIdClaim, out int userUpd))
                {
                    return BadRequest("ID de usuario inválido");
                }

                var response = await _servicioService.EditarServicios(request, userUpd);

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
        [HttpDelete("EliminarServicio")]
        public async Task<IActionResult> EliminarServicio(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _servicioService.EliminarServicio(id);

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

        [HttpGet("ObtenerServiciosPorId")]
        public async Task<IActionResult> ObtenerServiciosId (int idPatrocinador)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                // Llamar al servicio para obtener los servicios filtrados por el ID del patrocinador
                var response = await _servicioService.ObtenerServiciosIdPra(idPatrocinador);

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
