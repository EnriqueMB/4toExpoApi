using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
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
        private readonly IBaseRepository<Pagos> _pagosRepository;
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
            IBaseRepository<Reservas> reservarEntityRepository,
            IBaseRepository<Pagos> pagosRepository)
        {
            _reservaRepository = reservaRepository;
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;
            _configuration = configuration;
            _incluyePaqueteRepository = incluyePaqueteRepository;
            _paqueteGeneralRepository = paqueteGeneralRepository;
            _usuariosRepository = usuariosRepository;
            _reservarEntityRepository = reservarEntityRepository;
            _pagosRepository = pagosRepository;
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
                    Pasarela = "ConectaTarjeta",
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

                var usuario = await _usuariosRepository.GetById(IdUser, _logger);
                if (usuario == null)
                {
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " User not found.");
                    return null;
                }

                var reserva = (await _reservarEntityRepository.GetAll(_logger))
                    .FirstOrDefault(x => x.IdUsuario == IdUser);
                if (reserva == null)
                {
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Reservation not found.");
                    return null;
                }

                var paqueteUsuario = (await _paqueteGeneralRepository.GetAll(_logger))
                    .FirstOrDefault(x => x.Nombre == reserva.Producto);
                if (paqueteUsuario == null)
                {
                    _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Package not found.");
                    return null;
                }

                var incluyeUsuario = (await _incluyePaqueteRepository.GetAll(_logger))
                    .Where(x => x.PaqueteId == paqueteUsuario.Id)
                    .Select(x => AppMapper.Map<IncluyePaquete, IncluyePaqueteRequest>(x))
                    .ToList();

                var responseReserva = new ReservaVM()
                {
                    NombrePaquete = paqueteUsuario.Nombre,
                    IdTipoPaquete = reserva.IdTipoPaquete,
                    Monto = paqueteUsuario.Precio,
                    Descripcion = paqueteUsuario.Descripcion,
                    IdTipoUsuario = usuario.IdTipoUsuario,
                    Beneficios = incluyeUsuario,
                    IdUsuario = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto,
                    Telefono = usuario.Telefono,
                    Correo = usuario.Correo,
                    Edad = usuario.Edad,
                    CompraConfirmada = reserva.ConfirmarCompra,
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

        public async Task<object> ObtenerPaqueteGeneral()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var reservas = await _reservarEntityRepository.GetAll(_logger);
                reservas = reservas.Where(x => x.Activo == true).ToList();
                var usuarios = await _usuariosRepository.GetAll(_logger);
                usuarios = usuarios.Where(x => x.IdTipoUsuario == 3).ToList();
                var incluyeGeneral = await _incluyePaqueteRepository.GetAll(_logger);
                incluyeGeneral = incluyeGeneral.Where(x => x.Activo == true).ToList();
                var paquetesGeneral = await _paqueteGeneralRepository.GetAll(_logger);
                paquetesGeneral = paquetesGeneral.Where(x => x.Activo == true).ToList();
                var pagos = await _pagosRepository.GetAll(_logger);
                pagos = pagos.Where(x => x.Activo == true).ToList();

                var response = (from reserva in reservas
                                join usuario in usuarios on reserva.IdUsuario equals usuario.Id
                                join paquete in paquetesGeneral on reserva.IdPaquete equals paquete.Id
                                join incluye in incluyeGeneral on paquete.Id equals incluye.PaqueteId into incluyeGrupo
                                join pago in pagos on reserva.Id equals pago.IdReserva
                                select new
                                {
                                    IdRegistroReserva = reserva.Id,
                                    IdUsuario = usuario.Id,
                                    NombreCompleto = usuario.NombreCompleto,
                                    Correo = usuario.Correo,
                                    Edad = usuario.Edad,
                                    Telefono = usuario.Telefono,
                                    Direccion = usuario.Ciudad,
                                    NombrePaquete = paquete.Nombre,
                                    Monto = paquete.Precio,
                                    Beneficios = incluyeGrupo,
                                    UrlBaucher = pago.BaucherPago,
                                    TipoDePago = pago.Pasarela,
                                    UrlComprobante = usuario.UrlImg,
                                    ConfirmarCompra = reserva.ConfirmarCompra
                                }).ToList();
                 
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }


        public async Task<List<ReservaVM>> ObtenerReservaCompradores()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var reservasDB = await _reservarEntityRepository
                    .GetAll(_logger, ["PaquetePatrocinadores", "PaquetePatrocinadores.Incluyes", "PaquetePatrocinadores.TipoPaquete", "Usuarios"], x => x.Activo == true && x.Usuarios.IdTipoUsuario == 3);

                var reservasVM = reservasDB.Select(x => new ReservaVM
                {
                    IdPaquete = x.PaquetePatrocinadores != null ? x.PaquetePatrocinadores.Id : 0,
                    IdTipoPaquete = x.PaquetePatrocinadores != null ? x.PaquetePatrocinadores.IdTipoPaquete : 0,
                    NombrePaquete = x.PaquetePatrocinadores != null ? x.PaquetePatrocinadores.NombrePaquete: "",
                    NombreCompleto = x.Usuarios != null ? x.Usuarios.NombreCompleto : "",
                    NombreTipoPaquete  = x.PaquetePatrocinadores != null ?  x.PaquetePatrocinadores.TipoPaquete.Nombre : "",
                    Correo = x.Usuarios != null ? x.Usuarios.Correo : "",
                    Descripcion = x.PaquetePatrocinadores != null ?  x.PaquetePatrocinadores.Descripcion : "",
                    Edad = x.Usuarios != null ? x.Usuarios.Edad : 0,
                    Monto = x.PaquetePatrocinadores != null ?  x.PaquetePatrocinadores.Precio : 0,
                    Empresa = x.Usuarios != null ? x.Usuarios.Asociacion : "",
                    Beneficios = x.PaquetePatrocinadores != null ? x.PaquetePatrocinadores.Incluyes.Select(x => new IncluyePaqueteRequest { Nombre = x.Nombre }).ToList() : []

                }).ToList();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return reservasVM;

            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }

        }

        public async Task<GenericResponse<Reservas>> ConfirmarPago(int idRegistroRerserva)
        {
            try
            {
                var response = new GenericResponse<Reservas>();
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                
                var confirmarPago = await _reservaRepository.ConfirmarPago(idRegistroRerserva, _logger);

                if (confirmarPago.Success)
                {
                    response.Data = confirmarPago.Data;
                    response.Message = "Se agrego Hotel";
                    response.Success = true;
                    response.CreatedId = confirmarPago.CreatedId;
                }
                else
                {
                    response.Data = confirmarPago.Data;
                    response.Message = "No se pudo confirmar";
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

        #endregion
    }
}
