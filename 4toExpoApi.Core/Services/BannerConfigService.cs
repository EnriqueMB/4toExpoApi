using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class BannerConfigService
    {

        #region <---Variables--->
        private readonly IBaseRepository<BannerConfig> _bannerConfigRepository;
        private ILogger<BannerConfigService> _logger;
       // private readonly IAzureBlobStorageService _azureBlobStorageService;
        #endregion
        #region <---Constructor--->
        public BannerConfigService(IBaseRepository<BannerConfig> bannerConfigRepository, ILogger<BannerConfigService> logger)
        {
            _bannerConfigRepository = bannerConfigRepository;
            _logger = logger;
           // _azureBlobStorageService = azureStorageBlobService;
        }
        #endregion

        #region <--- Metodos --->
        public async Task<List<BannerConfigRequest>> ObtenerBannerConfig(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listBanner = await _bannerConfigRepository.GetAll(_logger);

                if (listBanner == null || listBanner.Count() == 0)
                {
                    return null;
                }

                var requestListBannnerConfig = listBanner
                    .Where(bannerConfig => bannerConfig.Id == id)
                    .Select(bannerConfig => AppMapper.Map<BannerConfig, BannerConfigRequest>(bannerConfig))
                    .ToList();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return requestListBannnerConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        public async Task<List<BannerConfigRequest>> ObtenerTodosLosBanners()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listBanner = await _bannerConfigRepository.GetAll(_logger);

                if (listBanner == null || listBanner.Count() == 0)
                {
                    return null;
                }

                var requestListBannerConfig = listBanner
                    .Select(bannerConfig => AppMapper.Map<BannerConfig, BannerConfigRequest>(bannerConfig))
                    .ToList();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return requestListBannerConfig;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<BannerConfigRequest>> EditarBannerConfig(BannerConfigRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = new GenericResponse<BannerConfigRequest>();
                var bannerConfig = await _bannerConfigRepository.GetById(request.Id, _logger);
                if (bannerConfig == null)
                {
                    response.Message = "El Banner no existe";
                    return response;
                }


                //if (request.ImagenFile != null)
                //{
                //    if (request.UrlImg == "null" || request.UrlImg == null)
                //        request.UrlImg = await this._azureBlobStorageService.UploadAsync(request.ImagenFile, ContainerEnum.INBOXACONTECIMIENTO);
                //    else
                //        request.UrlImg = await this._azureBlobStorageService.UploadAsync(request.ImagenFile, ContainerEnum.INBOXACONTECIMIENTO, bannerConfig.Imagen);
                //}

                bannerConfig.Titulo = request.Titulo;
                bannerConfig.SubTitulo = request.SubTitulo;
                bannerConfig.Descripcion = request.Descripcion;
                bannerConfig.Imagen = request.Imagen;
                bannerConfig.CantidadExpositores = request.CantidadExpositores;
                bannerConfig.CantidadParticipantes = request.CantidadParticipantes;
                bannerConfig.Orden = request.Orden;
                bannerConfig.Dias = request.Dias;
                bannerConfig.CantidadConstructoras = request.CantidadConstructoras;
                bannerConfig.UserUpd = UserUpd;
                bannerConfig.FechaUpd = DateTime.Now;
              
                var update = await _bannerConfigRepository.Update(bannerConfig, _logger);
                if (update != null)
                {
                    response.Message = "Se editó correctamente el Banner";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se editó correctamente el Banner";
                    response.Success = false;
                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }


        public async Task<GenericResponse<BannerConfig>> EliminarBannerConfig(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<BannerConfig>();
                var banner = await _bannerConfigRepository.GetById(id, _logger);
                if (banner == null)
                {
                    response.Message = "El banner no existe";
                    return response;
                }

                banner.Activo = false;

                var update = await _bannerConfigRepository.Update(banner, _logger);
                if (update != null)
                {
                    response.Message = "Se eliminó correctamente el banner";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se eliminó correctamente el banner";
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
