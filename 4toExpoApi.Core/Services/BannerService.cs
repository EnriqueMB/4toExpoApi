
using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Ocsp;
using System.IO;
using System.Reflection;

namespace _4toExpoApi.Core.Services
{
    public class BannerService
    {
        #region <---VARIABLES--->
        private readonly IBaseRepository<Banner> _bannerRepository;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        private readonly IBaseRepository<RedSocial> _redSocialRepository;
        private readonly IBaseRepository<Patrocinadores> _patrocinadoresRepository;
        private readonly IBaseRepository<RedPatrocinador> _redPatrocinadorRepository;
        private ILogger<BannerService> _logger;
        #endregion
        #region <---CONSTRUCTOR--->
        public BannerService(IBaseRepository<Banner> bannerRepository, ILogger<BannerService> logger, IAzureBlobStorageService azureBlobStorageService,
            IBaseRepository<RedSocial> redSocialRepository, IBaseRepository<Patrocinadores> patrocinadoresRepository, IBaseRepository<RedPatrocinador> redPatrocinadorRepository)
        {
            _bannerRepository = bannerRepository;
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;
            _redSocialRepository = redSocialRepository;
            _patrocinadoresRepository = patrocinadoresRepository;
            _redPatrocinadorRepository = redPatrocinadorRepository;

        }
        #endregion
        #region <---METODOS--->

        public async Task<GenericResponse<BannerRequest>> AgregarBanner(BannerRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                _bannerRepository.BeginTransaction();

                var response = new GenericResponse<BannerRequest>();
               // var patron = (await _patrocinadoresRepository.GetAll(_logger, [], x => x.IdUsuario == request.IdPatrocinador && x.Activo == true)).FirstOrDefault();
                var addBanner = AppMapper.Map<BannerRequest , Banner>(request);

                if (request.VideoFile != null)
                {
                    if (request.UrlVideo == "null" || request.UrlVideo == null || request.UrlVideo == "undefined")
                        addBanner.UrlVideo = await this._azureBlobStorageService.UploadAsync(request.VideoFile, ContainerEnum.banner);
                   
                }


                //addBanner.IdPatrocinador = patron.Id;


                var add = await _bannerRepository.Add(addBanner, _logger);
                var redes = request.Redes.Select(x => new RedPatrocinador
                {
                    IdPatrocinador = addBanner.IdPatrocinador,
                    IdRedSocial = x.IdRedSocial,
                    UrlRedSocial = x.UrlRedSocial,
                    IdBanner = add.Id
                });

                var addRed = await _redPatrocinadorRepository.AddAll(redes, _logger);

                if (add.Id > 0)
                {
                    _bannerRepository.Commit();
                    response.Success = true;
                    response.Message = "Se agrego el banner del patrocinador correctamente";
                    response.Data = request;
                    response.CreatedId = add.Id.ToString();
                }
                else
                {
                    _bannerRepository.Rollback();
                    response.Success = false;
                    response.Message = "No se pudo crear el banner del patrocinador";
                    response.Data = request;
                    
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

        public async Task<GenericResponse<BannerRequest>> EditarDatos(BannerRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                _bannerRepository.BeginTransaction();

                var response = new GenericResponse<BannerRequest>();
                var bannerEdit = (await _bannerRepository.GetAll(_logger, [], x => x.IdPatrocinador == request.IdPatrocinador)).FirstOrDefault();
                if (bannerEdit == null)
                {
                    _bannerRepository.Rollback();
                    response.Message = "El banner no fue encontrado.";
                    response.Success = false;
                    return response;
                }
                var redSocial = await _redPatrocinadorRepository.GetAll(_logger, [], x => x.IdPatrocinador == request.IdPatrocinador);
               
                    foreach (var item in redSocial)
                    {
                        // Busca la red social correspondiente en el request.
                        var matchingRed = request.Redes.FirstOrDefault(x => x.IdRedSocial == item.IdRedSocial);

                        if (matchingRed != null)
                        {
                            // Si se encuentra una coincidencia, actualiza la URL.
                            item.UrlRedSocial = matchingRed.UrlRedSocial;

                        }
                    }

                // Verifica si existen redes sociales en la solicitud que no están en la lista 'redSocial'.
                var redExist = request.Redes.Any(x => !redSocial.Any(rs => rs.IdRedSocial == x.IdRedSocial));
                if (redExist)
                {
                    // Filtra las redes que no están en 'redSocial' y las prepara para ser añadidas.
                    var redesAdd = request.Redes
                        .Where(x => !redSocial.Any(rs => rs.IdRedSocial == x.IdRedSocial))
                        .Select(x => new RedPatrocinador
                        {
                            IdPatrocinador = request.IdPatrocinador,
                            IdRedSocial = x.IdRedSocial,
                            UrlRedSocial = x.UrlRedSocial,
                            IdBanner = bannerEdit.Id,
                        }).ToList();

                    // Si hay redes sociales para añadir, procede a agregarlas.
                    if (redesAdd.Any())
                    {
                        await _redPatrocinadorRepository.AddAll(redesAdd, _logger);
                    }
                }


                bannerEdit.NombreEmpresa = request.NombreEmpresa;
                bannerEdit.Descripcion = request.Descripcion;
  
                if (request.VideoFile != null)
                {
                    if (request.UrlVideo == "null" || request.UrlVideo == null || request.UrlVideo == "undefined")
                        bannerEdit.UrlVideo = await this._azureBlobStorageService.UploadAsync(request.VideoFile, ContainerEnum.banner);
                    else
                        bannerEdit.UrlVideo = await this._azureBlobStorageService.UploadAsync(request.VideoFile, ContainerEnum.banner, bannerEdit.UrlVideo);
                }

                await _redPatrocinadorRepository.UpdateAll(redSocial, _logger);
                var result = await _bannerRepository.Update(bannerEdit, _logger);
                if (result != null)
                {
                    _bannerRepository.Commit();
                    response.Message = "Se edito correctamente el banner";
                    response.UpdatedId = bannerEdit.Id.ToString();
                    response.Success = true;
                    response.Data = request;
                }
                else
                {
                    _bannerRepository.Rollback();
                    response.Message = "No se pudo editar correctamente el banner";
                    response.UpdatedId = bannerEdit.Id.ToString();
                    response.Success = false;
                    response.Data = request;
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
        public async Task<BannerVM> ObtenerBanner(int idPat)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                //var bannerList = await _bannerRepository.GetAll( _logger, [], x => x.IdPatrocinador ==idPat);

                //var redSociales = await _redSocialRepository.GetAll(_logger);

                //var banner = (from bann in bannerList
                //              join red in redSociales on bann.IdRedSocial equals red.Id
                //              select new BannerVM
                //              {
                //                  Id = bann.Id,
                //                  NombreEmpresa = bann.NombreEmpresa ?? null,
                //                  Descripcion = bann.Descripcion ?? null,
                //                  UrlVideo = bann.UrlVideo ?? null,
                //                  IdRedSocial = bann.IdRedSocial ,
                //                  NombreRedSocial = red.Nombre ?? null,
                //                  UrlRedSocial = red.UrlRedSocial ?? null,

                //              }).FirstOrDefault();


                var bannerLis = (await _bannerRepository.GetAll(_logger, ["RedPatrocinador", "RedPatrocinador.Red"], x => x.IdPatrocinador == idPat)).FirstOrDefault();
                //var patr = await _redPatrocinadorRepository.GetAll(_logger, ["Banner"], x => x.IdPatrocinador == idPat);
                if(bannerLis == null)
                {
                    return new BannerVM();
                }

                var banner = new BannerVM
                {
                    Id = bannerLis.Id,
                    NombreEmpresa = bannerLis.NombreEmpresa,
                    Descripcion = bannerLis.Descripcion,
                    UrlVideo = bannerLis.UrlVideo,
                    RedesRequest = bannerLis.RedPatrocinador.ToList(),
                    //IdRedSocial = bannerLis?.IdRedSocial,
                    //NombreRedSocial = bannerLis?.RedSocial?.Nombre,
                    //UrlRedSocial = bannerLis?.RedSocial?.UrlRedSocial
                };


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return banner;
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
