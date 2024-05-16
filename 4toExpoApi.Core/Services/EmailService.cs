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
        public async Task<GenericResponse> EnviarEmail(DatosEmailRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var host = _configuration["EmailSettings:Host"];
                var port = _configuration["EmailSettings:Port"];
                var usuario = _configuration["EmailSettings:UserName"];
                var contraseña = _configuration["EmailSettings:Password"];
                string nombrePlantilla = "plantilla_correo.html";

                MailHelper.EnviarEmail(host, port, usuario, contraseña, request, nombrePlantilla);

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
