using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Collections;


namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly ProductoServise _ProductoService;
        private readonly ILogger<ProductoController> _logger;

        public ProductoController(ProductoServise productoService, ILogger<ProductoController> logger)
        {
            _ProductoService = productoService;
            _logger = logger;
        }
        [HttpPost("AgregarProducto")]
        public async Task<IActionResult> AgregarProducto(ProductosRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _ProductoService.AgregarProducto(request, 1);

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


                var response = await _ProductoService.EditarProducto(request, 1);

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
        public async Task<IEnumerable> ObtenerProducto(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await _ProductoService.ObtenerProducto();

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
