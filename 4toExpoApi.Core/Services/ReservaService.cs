using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _4toExpoApi.Core.Services
{
    public class ReservaService
    {
        #region<--Variables-->
        private readonly IReservaRepository _reservaRepository;
        private readonly IBaseRepository<IncluyePaquete> _incluyePaqueteRepository;
        private readonly IBaseRepository<PaqueteGeneral> _paqueteGeneralRepository;
        private readonly IBaseRepository<Usuarios> _usuariosRepository;
        private readonly IBaseRepository<Reservas> _reservarEntityRepository;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private ILogger<ReservaService> _logger;
        private IConfiguration _configuration;
        #endregion

        #region <-- Constructor -->
        public ReservaService(IReservaRepository reservaRepository,
            ILogger<ReservaService> logger,
            IConfiguration configuration,
            IAzureBlobStorageService azureBlobStorageService,
            IBaseRepository<IncluyePaquete> incluyePaqueteRepository,
            IBaseRepository<PaqueteGeneral> paqueteGeneralRepository,
            IBaseRepository<Usuarios> usuariosRepository,
            IBaseRepository<Reservas> reservarEntityRepository)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;
            _configuration = configuration;
            _incluyePaqueteRepository = incluyePaqueteRepository;
            _paqueteGeneralRepository = paqueteGeneralRepository;
            _usuariosRepository = usuariosRepository;
            _reservarEntityRepository = reservarEntityRepository;
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
                    IdReserva = request.idReserva,
                    IdTransaccion = request.IdTransaction,
                    TitularTarjeta = request.Nombre,
                    EmailTarjeta = request.Correo,
                    Monto = request.Monto,
                    LogResponse = request.apiResponse,
                    StatusPago = request.StatusReserva,
                    Pasarela = "Paypal",
                    FechaAlt = DateTime.Now,
                    UserAlt = usrAlta,
                    Activo = true
                };


                var result = await _reservaRepository.AgregarReserva(pagos, _logger);

                var enviarCorreo = await EnviarBaucherCorreo(pagos);

                if (result.Success)
                {
                    if(enviarCorreo.Success)
                    {
                        response.Message = "Pago guardado correctamente y se envio el baucher de pago al correo";
                    }
                    else
                    {
                        response.Message = "Pago guardado correctamente";
                    }
                    response.Success = true;
                    response.CreatedId = result.CreatedId;

                }
                else
                {
                    response.Message = "Error al guardar el pago";
                    response.Success = false;
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

        public async Task<GenericResponse> EnviarBaucherCorreo(Pagos pagos)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "started Success");

                var response = new GenericResponse();

                if (pagos != null)
                {
                    // Obtener credenciales de correo
                    var host = _configuration["EmailSettings:Host"];
                    var port = _configuration["EmailSettings:Port"];
                    var usuario = _configuration["EmailSettings:UserName"];
                    var contraseña = _configuration["EmailSettings:Password"];
                    string nombrePlantilla = "plantilla_correoPago.html";

                    // Enviar correo
                    MailHelper.EnviarEmailPago(host, port, usuario, contraseña, pagos, nombrePlantilla);

                    response.Message = "Baucher de pago enviado";
                    response.Success = true;
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

        public async Task<GenericResponse> pagarTranferencia(TranferRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse();

                if (request.ImgFile != null)
                {
                    request.baucherPago = await this._azureBlobStorageService.UploadAsync(request.ImgFile, ContainerEnum.multimedia);
                }

                /***************  DATOS PARA LA TABLA PAGOS ********************/

                var pagos = new Pagos
                {
                    IdReserva = request.idReserva,
                    Monto = request.Monto,
                    StatusPago = "COMPLETADO",
                    Pasarela = "Transfrencia",
                    Banco = request.banco,
                    Cuenta = request.cuenta,
                    ClaveBancaria = request.claveBancaria,
                    BaucherPago = request.baucherPago,
                    FechaAlt = DateTime.Now,
                    UserAlt = 1,
                    Activo = true
                };


                var result = await _reservaRepository.AgregarReserva(pagos, _logger);

                if (result.Success)
                {
                    response.Message = "Pago guardado correctamente";
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

        public async Task<ReservaVM> ObtenerReservaPorId(int IdUser)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var reservaId = await _reservarEntityRepository.GetAll(_logger);
           
                var paquete = await _paqueteGeneralRepository.GetAll(_logger);
               
                var incluye = await _incluyePaqueteRepository.GetAll(_logger);

                var usuario = await _usuariosRepository.GetById(IdUser, _logger);

                var reserva = reservaId.Where(x => x.IdUsuario == IdUser).FirstOrDefault();

                var paqueteUsuario = paquete.Where(x => x.Nombre == reserva.Producto).FirstOrDefault();
                var incluyeUsuario = incluye
                                     .Where(x => x.PaqueteId == paqueteUsuario.Id)
                                     .Select(x => AppMapper.Map<IncluyePaquete, IncluyePaqueteRequest>(x))
                                     .ToList();

                var responseReserva = new ReservaVM()
                {
                    NombrePaquete = paqueteUsuario.Nombre,
                    IdTipoPaquete = reserva.IdPaquete,
                    Monto = paqueteUsuario.Precio,
                    Descripcion = paqueteUsuario.Descripcion,
                    Beneficios = incluyeUsuario,
                    IdTipoUsuario = usuario.IdTipoUsuario,
                    IdUsuario = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Telefono = usuario.Telefono,
                    Correo = usuario.Correo,
                    Edad = usuario.Edad
                };



                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return responseReserva;

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
