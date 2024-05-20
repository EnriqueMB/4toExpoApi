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

        public async Task<GenericResponse> reservaProducto(ReservaRequest request,PagoRequest requestPago,string requestDataDb,string responseContent ,int usrAlta)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var persona = AppMapper.Map<ReservaRequest, Reservas>(request);

               
                //***************  DATOS PARA TABLA PAGOS ********************/

                var pagos = new Pagos
                {
                    IdReserva = request.idReserva,
                    Pasarela = "OxxoPay",
                    StatusPago = requestPago.payment_status,
                    IdTransaccion = requestPago.id,
                    Monto = requestPago.amount / 100,
                    LogRequest = requestDataDb,
                    LogResponse = responseContent,
                    FechaAlt = DateTime.Now,
                    UserAlt = usrAlta,
                    Activo = true
                };
                


                var result = await _reservaRepository.AgregarReserva(pagos, _logger);

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

        public async Task<GenericResponse> reservaProductoPaypal(ReservaRequest request, int usrAlta)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse();

                var reserva = AppMapper.Map<ReservaRequest, Reservas>(request);

                /***************  DATOS PARA LA TABLA PAGOS ********************/

                var pagos = new Pagos
                {
                    IdTransaccion = request.IdTransaction,
                    TitularTarjeta = request.Nombre,
                    EmailTarjeta = request.Correo,
                    Monto = request.Monto,
                    StatusPago = request.StatusReserva,
                    Pasarela = "Paypal",
                    FechaAlt = DateTime.Now,
                    UserAlt = usrAlta,
                    Activo = true
                };


                var result = await _reservaRepository.AgregarReserva(pagos, _logger);

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
