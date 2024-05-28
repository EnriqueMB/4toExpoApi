using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class EmailService
    {
        #region Variables
        private ILogger<EmailService> _logger;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public EmailService(ILogger<EmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        #endregion

        #region Metodos
        public async Task<GenericResponse> EnviarEmail(DatosEmailRequest.Emails request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var host = _configuration["EmailSettings:Host"];
                var port = _configuration["EmailSettings:Port"];
                var usuario = _configuration["EmailSettings:UserName"];
                var contraseña = _configuration["EmailSettings:Password"];
                string nombrePlantilla = string.Empty;
                string correoEnviar = string.Empty;

                if (request.EmailContacto != null)
                {
                    nombrePlantilla = "plantilla_contacto.html";
                    correoEnviar = _configuration["Correos:nimia"];
                }

                if (request.EmailAlquiler != null)
                {
                    nombrePlantilla = "plantilla_alquiler.html";
                    correoEnviar = _configuration["Correos:nimia"];
                }

                if (request.EmailProductos != null)
                {
                    nombrePlantilla = "plantilla_productos.html";
                    correoEnviar = _configuration["Correos:nimia"];
                }

                if (request.EmailBolsaDeTrabajo != null)
                {
                    nombrePlantilla = "plantilla_bolsadetrabajo.html";
                    correoEnviar = _configuration["Correos:nimia"];
                }

                MailHelper.EnviarEmail(host, port, usuario, contraseña, request, nombrePlantilla, correoEnviar);

                response.Success = true;
                response.Message = "Correo enviado correctamente.";

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
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
