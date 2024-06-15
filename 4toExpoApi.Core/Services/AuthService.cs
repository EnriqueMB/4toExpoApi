using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using AutoMapper.Configuration.Annotations;
using KiddyCheckApi.Core.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _4toExpoApi.Core.Services
{
    public class AuthService
    {
        #region <-- Variables -->
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IBaseRepository<UsuariosPromocion> _promocionRepository;
        private ILogger<AuthService> _logger;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly JwtSettings _jwtSettings;
        private IConfiguration _configuration;
        #endregion

        #region <-- Constructor -->
        public AuthService(IUsuarioRepository usuarioRepository,
            ILogger<AuthService> logger, JwtSettings jwtSettings,
            IConfiguration configuration, IAzureBlobStorageService azureBlobStorageService, IBaseRepository<UsuariosPromocion> promocionRepository)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
            _jwtSettings = jwtSettings;
            _configuration = configuration;
            _azureBlobStorageService = azureBlobStorageService;
            _promocionRepository = promocionRepository;
        }
        #endregion

        #region <-- Metodos -->

        public async Task<GenericResponse> AgregarUsuario(UsuarioRequest request, int usrAlta)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var userDb = await _usuarioRepository.ExistsByNombreUsuario(request.correo, _logger);

                if (userDb != null)
                {
                    response.Message = "El nombre de usuario ya existe";
                    response.Success = false;

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }


                var usuario = AppMapper.Map<UsuarioRequest, Usuarios>(request);

                if(request.ImagenFile != null)
                {
                    request.urlImg = await this._azureBlobStorageService.UploadAsync(request.ImagenFile, ContainerEnum.multimedia);
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                usuario.UrlImg = request.urlImg;
                usuario.PasswordHash = passwordHash;
                usuario.PasswordSalt = passwordSalt;
                usuario.FechaAlt = DateTime.Now;
                usuario.UserAlt = usrAlta;
                usuario.Activo = true;

                var reserva = new Reservas
                {
                    Producto = request.producto,
                    IdPaquete = request.idPaquete,
                    Monto = request.monto,
                    NombreCompleto = request.NombreCompleto,
                    FechaAlt = DateTime.Now,
                    UserAlt = usrAlta,
                    Activo = true
                };

                var result = await _usuarioRepository.AgregarUsuario(usuario, reserva,  _logger);

                if (result.Success)
                {
                    response.Message = "Usuario agregado correctamente";
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

        public async Task<GenericResponse> AgregarUsuarioPromocion(UsuarioPromoRequest request, int usrAlta)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var userDb = await _usuarioRepository.ExistsByNombreUsuarioPromo(request.correo, _logger);

                if (userDb != null)
                {
                    response.Message = "El nombre de usuario ya existe";
                    response.Success = false;

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }


                var usuario = AppMapper.Map<UsuarioPromoRequest, UsuariosPromocion>(request);


                usuario.FechaAlt = DateTime.Now;
                usuario.UserAlt = usrAlta;
                usuario.Activo = true;


                var agregar = await _promocionRepository.Add(usuario, _logger);

                if (agregar != null)
                {
                    var enviarCorroQr = await enviarCorreo(request);

                    if (enviarCorroQr.Success)
                    {
                        response.Message = "Participante agregado correctamente y QR enviado al correo";
                    }
                    else
                    {
                        response.Message = "Participante agregado correctamente pero no se envió el correo";
                    }
                    response.Success = true;
                    response.CreatedId = agregar.Id.ToString();
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


        public async Task<GenericResponse> enviarCorreo(UsuarioPromoRequest usuarioDatos)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

              

                if (usuarioDatos != null)
                {
                    // Obtener credenciales de correo
                    var host = _configuration["EmailSettings:Host"];
                    var port = _configuration["EmailSettings:Port"];
                    var usuario = _configuration["EmailSettings:UserName"];
                    var contraseña = _configuration["EmailSettings:Password"];
                    string nombrePlantilla = "plantilla_correoQr.html";

                    // Enviar correo
                    MailHelperQr.EnviarEmail(host, port, usuario, contraseña, usuarioDatos, nombrePlantilla);

                    response.Message = "Correo enviado correctamente";
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

        public async Task<AuthResponse> Login(AuthRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new AuthResponse();

                var usuario = await _usuarioRepository.ExistsByNombreUsuario(request.UserName, _logger);

                if (usuario == null)
                {
                    response.Message = "El usuario no existe";
                    response.Success = false;

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }

                if (!VerifyPasswordHash(request.Password, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    response.Message = "La contraseña es incorrecta";
                    response.Success = false;

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
                }

           
                //var roles = await _usuarioRepository.ObtenerRolesUsuario(usuario.Id, _logger);

                var token = JwtHelper.GenTokenkey(new UserToken
                {
                    UserName = usuario.Correo,
                    Id = usuario.Id,
                    Roles = new List<int>()//roles.Data
                }, _jwtSettings, _configuration);

                response.Token = token.Token;
                response.UserName = usuario.Correo;
                response.UserId = usuario.Id.ToString();
                response.Permisos = new List<long>();
                response.IdTipoUsuario = usuario.IdTipoUsuario;
                response.Roles = new List<int>();


                response.Message = "Login correcto";
                response.Success = true;

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Obtener todos los usuarios
        public async Task<ListResponse<UsuariosVM>> ObtenerUsuarios()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _usuarioRepository.ObtenerUsuarios(_logger);

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    var list = AppMapper.Map<List<Usuarios>, List<UsuariosVM>>(response.Data);

                    return new ListResponse<UsuariosVM>
                    {
                        Data = list,
                        Total = list.Count
                    };
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return new ListResponse<UsuariosVM>();
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<ListResponse<UsuariosPromoVM>> ObtenerUsuariosPromo()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new ListResponse<UsuariosPromoVM>();

                string[] include = new string[] { "Usuarios" };
                Expression<Func<UsuariosPromocion, bool>> query = u => u.Activo == true;

                var usuariosPromo = await _promocionRepository.GetAll(_logger, include, query);

                if (usuariosPromo != null)
                {

                    var listaUusarios = usuariosPromo.Select(usuariosPromo => new UsuariosPromoVM
                    {
                        Id = usuariosPromo.Id,
                        NombreCompleto = usuariosPromo.NombreCompleto,
                        Correo = usuariosPromo.Correo,
                        Telefono = usuariosPromo.Telefono,
                        Asociacion = usuariosPromo.Asociacion,
                        Estado = usuariosPromo.Estado,
                        NombreRepresentante = usuariosPromo.Usuarios.NombreCompleto,
                        Ciudad = usuariosPromo.Ciudad,
                    }).ToList();


                    response.Data = listaUusarios;
                    response.Total = listaUusarios.Count();

                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                    return response;

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
        //Obtener usuario por id
        public async Task<UsuariosResponse> ObtenerUsuarioPorId(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _usuarioRepository.ObtenerUsuarioPorId(id, _logger);

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    var usuario = AppMapper.Map<Usuarios, UsuariosResponse>(response.Data);

                    return usuario;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Obtener usuario por nombre de usuario
        public async Task<UsuariosResponse> ExistsNombreUsuario(string nombreUsuario, int idUsuario)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _usuarioRepository.ExistsNombreUsuario(nombreUsuario, idUsuario, _logger);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    var usuario = AppMapper.Map<Usuarios, UsuariosResponse>(response.Data);

                    return usuario;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Eliminar usuario
        public async Task<GenericResponse> EliminarUsuario(int id, int usrMod)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var result = await _usuarioRepository.EliminarUsuario(id, usrMod, _logger);

                if (result.Success)
                {
                    response.Message = result.Message;
                    response.Success = true;
                    response.UpdatedId = result.UpdatedId;
                }
                else
                {
                    response.Message = result.Message;
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


        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        #endregion
    }
}
