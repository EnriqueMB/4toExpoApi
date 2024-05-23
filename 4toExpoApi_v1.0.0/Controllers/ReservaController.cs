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
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;


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
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }

        }

        [HttpPost("Paypal")]
        public async Task<IActionResult> guardarPagoPaypal(ReservaRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                
                var response = await _payService.reservaProductoPaypal(request,1);

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

        [HttpPost("PagarTransferencia")]
        public async Task<IActionResult> guardarPagoTransfer([FromForm] TranferRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
               
                var response = await _payService.pagarTranferencia(request);

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

        [HttpPost("webhook")]
        public async Task<IActionResult> ConektaWebhook()

        {
            try
            {
                // Dentro de tu método ConektaWebhook o donde necesites realizar la solicitud HTTP
                var apiUrl = "https://api.conekta.io/webhooks";
                var acceptHeader = "application/vnd.conekta-v2.1.0+json";
                var privateKey = "key_iRVbKuhPBqkrw8N4CSGAIXZ";

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(apiUrl),
                    Headers =
                    {
                        { "Authorization", "Bearer " + privateKey },
                        { "accept", acceptHeader },
                        { "Accept-Language", "es" },
                    },
                    Content = new StringContent("{\"synchronous\":false,\"url\":\"https://api-evento.azurewebsites.netReserva/webhook\"}")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };

                // Envía la solicitud HTTP y maneja la respuesta
                using (var response = await client.SendAsync(request))
                {
                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        // Lee el cuerpo de la respuesta
                        var body = await response.Content.ReadAsStringAsync();
                        var respuesta = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);

                        var eventosSuscritos = respuesta["subscribed_events"] as JArray;

                        if (eventosSuscritos != null && eventosSuscritos.Any(e => e.ToString() == "charge.paid"))
                        {
                            // Aquí puedes realizar acciones específicas para eventos de pago
                        }
                        return Ok(response);
                    }
                    else
                    {
                      
                       
                        return StatusCode((int)response.StatusCode);
                    }

                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing Conekta webhook: " + ex.Message);
                return StatusCode(500);
            }
        }

        [HttpGet("getwebhook")]
        public async Task<IActionResult> getConektaWebhook()

        {
            try
            {
                // Dentro de tu método ConektaWebhook o donde necesites realizar la solicitud HTTP
                var apiUrl = "https://api.conekta.io/webhooks";
                var acceptHeader = "application/vnd.conekta-v2.1.0+json";
                var privateKey = "key_iRVbKuhPBqkrw8N4CSGAIXZ";

                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(apiUrl),
                    Headers =
                    {
                        { "Authorization", "Bearer " + privateKey },
                        { "accept", acceptHeader },
                        { "Accept-Language", "es" },
                    },
                    Content = new StringContent("{\"synchronous\":false,\"url\":\"https://api-evento.azurewebsites.netReserva/webhook\"}")
                    {
                        Headers =
                        {
                            ContentType = new MediaTypeHeaderValue("application/json")
                        }
                    }
                };

                // Envía la solicitud HTTP y maneja la respuesta
                using (var response = await client.SendAsync(request))
                {
                    // Verifica si la solicitud fue exitosa
                    if (response.IsSuccessStatusCode)
                    {
                        response.EnsureSuccessStatusCode();
                        // Lee el cuerpo de la respuesta
                        var body = await response.Content.ReadAsStringAsync();
                        var respuesta = JsonConvert.SerializeObject(body);

                      
                        return Ok(respuesta);
                    }
                    else
                    {


                        return StatusCode((int)response.StatusCode);
                    }


                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error processing Conekta webhook: " + ex.Message);
                return StatusCode(500);
            }
        }



        #endregion
    }
}
