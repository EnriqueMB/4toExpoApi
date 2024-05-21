using _4toExpoApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UniversidadController : Controller
    {
        #region<--Variables-->
        private readonly UniversidadService _universidadService;
        private ILogger<UniversidadController>  _logger;

        #endregion
        #region <--Contructor-->
        public UniversidadController(ILogger<UniversidadController> logger,UniversidadService universidad) 
        {
            _logger = logger;
            _universidadService = universidad;
        }
        #endregion
        #region<--Metodos-->

        [HttpGet("ObtenerUniversidad")]
        public async Task <IEnumerable> ObtenerUniversidad()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = await _universidadService.ObtenerUniversidades();

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
        #endregion
    }
}
