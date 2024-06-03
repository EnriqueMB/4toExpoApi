
using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private ILogger<BannerService> _logger;
        #endregion
        #region <---CONSTRUCTOR--->
        public BannerService(IBaseRepository<Banner> bannerRepository, ILogger<BannerService> logger, IAzureBlobStorageService azureBlobStorageService,
            IBaseRepository<RedSocial> redSocialRepository)
        {
            _bannerRepository = bannerRepository;
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;
            _redSocialRepository = redSocialRepository;

        }
        #endregion
        #region <---METODOS--->

        public async Task<GenericResponse<BannerRequest>> EditarDatos(BannerRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
               
                var response = new GenericResponse<BannerRequest>();
                var bannerEdit = await _bannerRepository.GetById(request.Id, _logger);
                var redSocial = await _redSocialRepository.GetById(request.IdRedSocial, _logger);
                bannerEdit.NombreEmpresa = request.NombreEmpresa;
                bannerEdit.Descripcion = request.Descripcion;
                redSocial.UrlRedSocial = request.UrlRedSocial;
                bannerEdit.IdRedSocial = request.IdRedSocial;
                if (request.VideoFile != null)
                {
                    if (request.UrlVideo == "null" || request.UrlVideo == null)
                        bannerEdit.UrlVideo = await this._azureBlobStorageService.UploadAsync(request.VideoFile, ContainerEnum.banner);
                    else
                        bannerEdit.UrlVideo = await this._azureBlobStorageService.UploadAsync(request.VideoFile, ContainerEnum.banner, bannerEdit.UrlVideo);
                }
                
                await _redSocialRepository.Update(redSocial, _logger);
                var result = await _bannerRepository.Update(bannerEdit, _logger);
                if(result != null)
                {
                    response.Message = "Se edito correctamente el banner";
                    response.UpdatedId = bannerEdit.Id.ToString();
                    response.Success = true;
                    response.Data = request;
                }
                else
                {
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
        public async Task<BannerVM> ObtenerBanner(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var bannerList = await _bannerRepository.GetAll( _logger);

                var redSociales = await _redSocialRepository.GetAll(_logger);

                var banner = (from bann in bannerList
                              join red in redSociales on bann.IdRedSocial equals red.Id
                              select new BannerVM
                              {
                                  Id = bann.Id,
                                  NombreEmpresa = bann.NombreEmpresa ?? null,
                                  Descripcion = bann.Descripcion ?? null,
                                  UrlVideo = bann.UrlVideo ?? null,
                                  IdRedSocial = bann.IdRedSocial,
                                  NombreRedSocial = red.Nombre ?? null,
                                  UrlRedSocial = red.UrlRedSocial ?? null,


                              }).Where(x => x.Activo == true && x.IdPatrocinador == id).FirstOrDefault();


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return (banner);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        private void FirstOrDefault()
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}
