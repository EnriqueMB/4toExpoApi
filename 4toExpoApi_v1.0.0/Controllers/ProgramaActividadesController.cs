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
    public class ProgramaActividadesController : ControllerBase
    {
        #region<-----Variable----->
        private readonly ProgramaActividadesService _programaActividadesService;
        private ILogger<ProgramaActividadesController> _logger;
        #endregion


        #region<-----Constructor----->
        public ProgramaActividadesController(ProgramaActividadesService programaActividadesService, ILogger<ProgramaActividadesController> logger)
        {
            _programaActividadesService = programaActividadesService;
            _logger = logger;
        }
        #endregion


        #region<-----Metodos----->

        //Agregar Programa de Actividades
        [HttpPost]
        [Route("AgregarProgramaActividades")]


        public async Task<IActionResult> AgregarProgramaActividades(ProgramaActividadesRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var IdUseralta = "1"; /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var response = await _programaActividadesService.AgregarProgramaActividades(request, int.Parse(IdUseralta));

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
        //Actualizar Programa de Actividades
        [HttpPut]
        [Route("ActualizarProgramaActividades")]

        public async Task<IActionResult> ActualizarProgramaActividades(ProgramaActividadesRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var idUsuario = "1"; /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var response = await _programaActividadesService.ActualizarProgramaActividades(request, int.Parse(idUsuario));

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
        //Obtener Programa de Actividades
        [HttpGet("ObtenerProgramaActividades")]
        public async Task<IActionResult> ObtenerProgramaActividades()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _programaActividadesService.ObtenerProgramaActividades();

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
        //Eliminar Programa de Actividades
        [HttpDelete]
        [Route("EliminarProgramaActividades")]

        public async Task<IActionResult> EliminarProgramaActividades(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var idUsuario = "1"; /*User.Claims.FirstOrDefault(x => x.Type == "Id").Value;*/

                var response = await _programaActividadesService.EliminarProgramaActividades(id, int.Parse(idUsuario));

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
       
   
