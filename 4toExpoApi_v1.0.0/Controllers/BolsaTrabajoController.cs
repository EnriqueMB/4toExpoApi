using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
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
        #endregion


        #region<-----Constructor----->
        public BolsaTrabajoController(BolsaTrabajoService bolsaTrabajoService, ILogger<BolsaTrabajoController> logger)
        {
            _bolsaTrabajoService = bolsaTrabajoService;
            _logger = logger;
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

                var IdUseralta = "1"; /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var response = await _bolsaTrabajoService.AgregarBolsaTrabajo(request, int.Parse(IdUseralta));

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

                var idUsuario = "1"; /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var response = await _bolsaTrabajoService.ActualizarBolsaTrabajo(request, int.Parse(idUsuario));

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

                var response = await _bolsaTrabajoService.ObtenerBolsaTrabajo();

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


        #endregion


    }
}
