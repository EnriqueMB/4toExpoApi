using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
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
    public class ReservaService
    {
        #region<--Variables-->
        private readonly IReservaRepository _reservaRepository;
        private ILogger<ReservaService> _logger;
        #endregion

        #region <-- Constructor -->
        public ReservaService(IReservaRepository reservaRepository,
            ILogger<ReservaService> logger,
            IConfiguration configuration)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
        }
        #endregion

        #region <-- Metodos -->

        public async Task<GenericResponse> reservaProducto(PagoRequest request, int usrAlta)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();


                var persona = AppMapper.Map<PagoRequest, Reserva>(request);

           
                persona.FechaAlt = DateTime.Now;
                persona.UserAlt = usrAlta;
                persona.Activo = true;

                var result = await _reservaRepository.AgregarReserva(persona, _logger);

                if (result.Success)
                {
                    response.Message = "Reserva agregado correctamente";
                    response.Success = true;
                    response.CreatedId = result.CreatedId;
                }

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
