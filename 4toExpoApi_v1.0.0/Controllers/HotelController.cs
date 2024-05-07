using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class HotelController : Controller
    {
        

        #region <---Variables--->
        private readonly HotelService hotelService;
        private readonly ILogger<ServiciosController> _logger;
        #endregion
        #region <---Constructor--->
        public HotelController(HotelService hotelServiceParam, ILogger<ServiciosController> logger)
        {
            hotelService = hotelServiceParam;
            _logger = logger;
        }
        #endregion

        [HttpPost("AgregarHotel")]
        public async Task<IActionResult> AgregarHotel(HotelRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


                var response = await hotelService.AgregarHotel(request, 1);

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
