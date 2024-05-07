﻿using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Reflection;

namespace _4toExpoApi_v1._0._0.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class BannerConfigController : ControllerBase
    {
        #region <---Variables--->
        private readonly BannerConfigService _bannerConfigService;
        private readonly ILogger<BannerConfigController> _logger;
        #endregion
        #region <---Constructor--->
        public BannerConfigController(BannerConfigService bannerConfigService, ILogger<BannerConfigController> logger)
        {
            _bannerConfigService = bannerConfigService;
            _logger = logger;
        }
        [HttpGet("ObtenerBannerConfig/{id}")]
        public async Task<IEnumerable> ObtenerBannerConfig(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _bannerConfigService.ObtenerBannerConfig(id);

                if (response != null)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return response;
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



        [HttpPut("EditarBannerConfig")]
        public async Task<IActionResult> EditarBannerConfig(BannerConfigRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = await _bannerConfigService.EditarBannerConfig(request, 1);

                if (response.Success)
                {
                    _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                    return BadRequest(response);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }




        [HttpDelete("EliminarBannerConfig")]
    public async Task<IActionResult> EliminarBannerConfig(int id)
    {
        try
        {
            _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");


            var response = await _bannerConfigService.EliminarBannerConfig(id);

            if (response.Success)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return Ok(response);
            }

            _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

            return BadRequest(response);
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