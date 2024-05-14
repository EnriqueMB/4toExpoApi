using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.InteropServices;
using System.Text.Unicode;
using Newtonsoft.Json;


namespace _4toExpoApi_v1._0._0.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ReservaController : Controller
    {
        #region <--- Variables --->
      
        private ILogger<ReservaController> _logger; 
        private readonly ReservaService _payService;
        private readonly IHttpClientFactory _clientFactory;
        #endregion

        #region <--- Constructor --->
        public ReservaController(ILogger<ReservaController> logger, IHttpClientFactory clientFactory,
            ReservaService oxxoPayService
            )
        {
            _logger = logger;
            _clientFactory = clientFactory;
            _payService = oxxoPayService;
        }
        #endregion

        #region<--- Metodos->

        [HttpPost("GenerarRefefencia")]
        public async Task <IActionResult> pagarOxxo(ReservaRequest request)
        {

            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                
                if (request.TipoPago == "OxxoPay")
                {
                    var apiUrl = "https://api.conekta.io/orders";
                    var privateKey = "key_iRVbKuhPBqkrw8N4CSGAIXZ";

                    // Calcular la fecha y hora actual
                    DateTime now = DateTime.Now;
                    // Agregar 12 horas a la fecha y hora actual
                    DateTime expiresAt = now.AddHours(12);
                    // Convertir la fecha y hora a segundos desde el 1 de enero de 1970 (época UNIX)
                    long expiresAtUnixTimestamp = (long)(expiresAt - new DateTime(1970, 1, 1)).TotalSeconds;

                    var requestData = new
                    {
                        line_items = new[]
                        {
                        new
                        {
                            name = request.Producto,
                            unit_price = request.Monto * 100,
                            quantity = request.Cantidad
                        }
                    },
                        currency = "MXN",
                        customer_info = new
                        {
                            name = request.Nombre,
                            email = request.Correo,
                            phone = request.Telefono
                        },
                        metadata = new
                        {
                            datos_extra = "1234"
                        },
                        charges = new[]
                                        {
                        new
                        {
                            payment_method = new
                            {
                                type = "cash",
                                expires_at = expiresAtUnixTimestamp // Aquí se utiliza el valor calculado
                            }
                        }
                    }
                    };
                    using (var client = _clientFactory.CreateClient())
                    {
                        client.DefaultRequestHeaders.Clear();
                        client.DefaultRequestHeaders.Add("Accept", "application/vnd.conekta-v2.0.0+json");
                        //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                        var byteArray = Encoding.ASCII.GetBytes($"{privateKey}:");
                        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                        var requestContent = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

                        var responseOxo = await client.PostAsync(apiUrl, requestContent);

                        if (responseOxo.IsSuccessStatusCode)
                        {
                            var responseContent = await responseOxo.Content.ReadAsStringAsync();
                            var conektaResponse = JsonConvert.DeserializeObject<PagoRequest>(responseContent);
                            var requestDataDb = JsonConvert.SerializeObject(requestData);
                            // return Ok(responseContent);
                            var response = await _payService.reservaProducto(request, conektaResponse, requestDataDb, responseContent, 1);

                            if (response.Success)
                            {
                                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                                return Ok(response);
                            }

                            _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                            return BadRequest(response);
                        }
                        else
                        {
                            return StatusCode((int)responseOxo.StatusCode);
                        }
                    }

                }
                else if(request.TipoPago=="Paypal")
                {
                    var response = await _payService.reservaProductoPaypal(request);

                    if (response.Success)
                    {
                        _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                        return Ok(response);
                    }

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return BadRequest(response);
                }
                else
                {
                    var response = await _payService.reservaProductoPaypal(request);

                    if (response.Success)
                    {
                        _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                        return Ok(response);
                    }

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return BadRequest(response);
                }

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
