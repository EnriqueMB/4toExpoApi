using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Collections;
using _4toExpoApi.DataAccess.IRepositories;


namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoServise _ProductoService;
        private readonly ILogger<ProductoController> _logger;
        private readonly IPatrocinadoresRepository _patrocinadorRepository;

        public ProductoController(ProductoServise productoService, ILogger<ProductoController> logger, IPatrocinadoresRepository patrocinadorRepository)
        {
            _ProductoService = productoService;
            _logger = logger;
             _patrocinadorRepository = patrocinadorRepository;
        }
        [HttpPost("AgregarProducto")]
        public async Task<IActionResult> AgregarProducto(ProductosRequest request)
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

                var response = await _ProductoService.AgregarProducto(request, userAlt, idPatrocinador);

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
        [HttpPut("EditarProducto")]
        public async Task<IActionResult> EditarProducto(ProductosRequest request)
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

                var response = await _ProductoService.EditarProducto(request, userUpd);

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
        [HttpGet("ObtenerProducto")]
        public async Task<IEnumerable> ObtenerProducto()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var userId = User.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    // El usuario no está logueado, retornar un error
                    return (IEnumerable)Unauthorized("Usuario no autenticado");
                }
                int userAlt = int.Parse(userId);

                // Buscar el patrocinador asociado al usuario logueado
                var patrocinador = await _patrocinadorRepository.GetByUserIdAsync(userAlt);
                if (patrocinador == null)
                {
                    // El usuario no tiene un patrocinador asociado, retornar un error
                    return (IEnumerable)BadRequest("El usuario no tiene un patrocinador asociado");
                }
                int idPatrocinador = patrocinador.Id;

                var response = await _ProductoService.ObtenerProducto(idPatrocinador);

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
        [HttpDelete("EliminarProducto")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _ProductoService.EliminarProducto(id);

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
