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

                var consecutivo = await _reservaRepository.GetAll(_logger);
                var ultimoConsecutivo = consecutivo.OrderByDescending(x => x.Id).FirstOrDefault();

            

                var persona = AppMapper.Map<ReservaRequest, Reservas>(request);

               
                //***************  DATOS PARA TABLA PAGOS ********************/

                var pagos = new Pagos
                {
                    Pasarela = "OxxoPay",
                    StatusPago = requestPago.payment_status,
                    IdTransaccion = requestPago.id,
                    Monto = requestPago.amount / 100,
                    FechaAlt = DateTime.Now,
                    UserAlt = usrAlta,
                    Activo = true
                };
                

                //***************  DATOS ´PARA TABLA RESERVA ********************/
                string nuevoConsecutivo;

                if (ultimoConsecutivo.Consecutivo != null)
                {
                    string numeroConsecutivoString = ultimoConsecutivo.Consecutivo.Substring(6);
                    int numeroConsecutivo = int.Parse(numeroConsecutivoString);
                   
                    numeroConsecutivo++;

                    nuevoConsecutivo = "ECIES-" +numeroConsecutivo.ToString();
                }
                else
                {
                    nuevoConsecutivo = "ECIES-1";
                }
                persona.Consecutivo = nuevoConsecutivo;
                persona.LogRequest = requestDataDb;
                persona.LogResponse = responseContent;
                persona.StatusReserva = requestPago.payment_status;
                persona.FechaAlt = DateTime.Now;
                persona.UserAlt = usrAlta;
                persona.Activo = true;

                //***************  DATOS ´PARA TABLA CLIENTES ********************/

                var clientes = new Clientes
                {
                    Identificador = nuevoConsecutivo,
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Telefono = request.Telefono,
                    Correo = request.Correo,
                    FechaAlt = DateTime.Now,
                    Activo = true
                };


                var result = await _reservaRepository.AgregarReserva(persona, pagos, clientes, _logger);

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

                var reserva = AppMapper.Map<ReservaRequest,Reservas>(request);

                var consecutivo = await _reservaRepository.GetAll(_logger);
                var ultimoConsecutivo = consecutivo.OrderByDescending(x => x.Id).FirstOrDefault();
                
                
                /***************  DATOS ´PARA TABLA RESERVA ********************/
                string nuevoConsecutivo;

                if (ultimoConsecutivo.Consecutivo != null)
                {
                    string numeroConsecutivoString = ultimoConsecutivo.Consecutivo.Substring(6);
                    int numeroConsecutivo = int.Parse(numeroConsecutivoString);

                    numeroConsecutivo++;

                    nuevoConsecutivo = "ECIES-" + numeroConsecutivo.ToString();
                }
                else
                {
                    nuevoConsecutivo = "ECIES-1";
                }
                reserva.Consecutivo = nuevoConsecutivo;
                reserva.FechaAlt = DateTime.Now;
                reserva.UserAlt = usrAlta;
                reserva.Activo = true;

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

                /***************  DATOS PARA LA TABLA CLIENTES******/
               
                var clientes = new Clientes
                {
                    Identificador = nuevoConsecutivo,
                    Nombre = request.Nombre,
                    Apellidos = request.Apellidos,
                    Telefono = request.Telefono,
                    Correo = request.Correo,
                    FechaAlt = DateTime.Now,
                    Activo = true
                };

                var result = await _reservaRepository.AgregarReserva(reserva,pagos, clientes,_logger);

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
