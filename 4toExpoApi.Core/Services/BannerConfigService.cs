﻿using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
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
        #endregion
        #region <---Constructor--->
        public BannerConfigService(IBaseRepository<BannerConfig> bannerConfigRepository, ILogger<BannerConfigService> logger)
        {
            _bannerConfigRepository = bannerConfigRepository;
            _logger = logger;
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

        public async Task<GenericResponse<BannerConfigRequest>> EditarBannerConfig(BannerConfigRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<BannerConfigRequest>();
                var bannerConfig = await _bannerConfigRepository.GetById(request.Id, _logger);
                if (bannerConfig == null)
                {
                    response.Message = "El Banner no existe";
                    return response;
                }
                bannerConfig.Titulo = request.Titulo;
                bannerConfig.SubTitulo = request.SubTitulo;
                bannerConfig.Descripcion = request.Descripcion;
                bannerConfig.Imagen = request.Imagen;
                bannerConfig.CantidadExpositores = request.CantidadExpositores;
                bannerConfig.CantidadParticipantes = request.CantidadParticipantes;
                bannerConfig.Orden = request.Orden;
                bannerConfig.UserUpd = UserUpd;
                bannerConfig.FechaUpd = DateTime.Now;
                var update = await _bannerConfigRepository.Update(bannerConfig, _logger);
                if (update != null)
                {
                    response.Message = "Se edito correctamente el Banner";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se edito correctamente el Banner";
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

        public async Task<GenericResponse<BannerConfigRequest>> EliminarBannerConfig(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<BannerConfigRequest>();
                var bannerConfig = await _bannerConfigRepository.GetById(id, _logger);
                if (bannerConfig == null)
                {
                    response.Message = "El Banner no existe";
                    return response;
                }

                bannerConfig.Id = 0;


                var update = await _bannerConfigRepository.Update(bannerConfig, _logger);
                if (update != null)
                {
                    response.Message = "Se elimino correctamente el Banner";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se elimino correctamente el Banner";
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

        public async Task<object?> EditarBannerConfig(ServicioRequest request, int v)
        {
            throw new NotImplementedException();
        }

        #endregion




    }
}
