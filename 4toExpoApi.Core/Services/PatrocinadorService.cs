using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
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
    public class PatrocinadorService
    {
         #region <---Variables--->
      
        private readonly IBaseRepository<Patrocinadores> _patrocinadorRepository;
        private ILogger<PatrocinadorService> _logger;
        private readonly IAzureBlobStorageService _azureBlobStorageService;

        #endregion
        #region <---Constructor--->

        public PatrocinadorService(IBaseRepository<Patrocinadores> patrocinadorRepository, ILogger<PatrocinadorService> logger, IAzureBlobStorageService azureStorageBlobService) {
            _patrocinadorRepository = patrocinadorRepository;
            _logger = logger;
            _azureBlobStorageService = azureStorageBlobService;
        }
        #endregion

        #region <---Metodos--->

        public async Task<GenericResponse<PatrocinadorRequest>> AgregarPatrocinador(PatrocinadorRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PatrocinadorRequest>();

                var addPatrocinador = AppMapper.Map<PatrocinadorRequest, Patrocinadores>(request);

                if (request.UrlImg != null)
                {
                    request.UrlLogo = await this._azureBlobStorageService.UploadAsync(request.UrlImg, ContainerEnum.logo);  
                }

                addPatrocinador.FechaAlt = DateTime.Now;
                addPatrocinador.UserAlt = userAlt;
                addPatrocinador.UrlLogo = request.UrlLogo;
                addPatrocinador.Activo = true;

                var add = await _patrocinadorRepository.Add(addPatrocinador, _logger);
                if (add != null && add.Id > 0)
                {
                    response.Data = request;
                    response.Message = "Se agrego correctamente el patrocinador";
                    response.Success = true;
                    response.CreatedId = add.Id.ToString();
                }
                else
                {
                    response.Data = request;
                    response.Message = "No se pudo agregar correctamente el patrocinador";
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

        public async Task<List<PatrocinadorRequest>> ObtenerPatrocinador()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listPatrocinador = await _patrocinadorRepository.GetAll(_logger);

                if (listPatrocinador == null || listPatrocinador.Count() == 0)
                {
                    return null;
                }
                var listaPatrocinadorFiltrada = listPatrocinador.Where(x => x.Activo == true).ToList();

                var requestListPatrocinador = listaPatrocinadorFiltrada.Select(patrocinador => AppMapper.Map<Patrocinadores, PatrocinadorRequest>(patrocinador)).ToList();


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return requestListPatrocinador;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<PatrocinadorRequest>> EditarPatrocinador(PatrocinadorRequest request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PatrocinadorRequest>();
                var patrocinador = await _patrocinadorRepository.GetById(request.Id, _logger);
                if (patrocinador == null)
                {
                    response.Message = "El patrocinador no existe";
                    return response;
                }

                if (request.UrlImg != null)
                {
                    if (request.UrlLogo == "null" || request.UrlImg == null)
                        request.UrlLogo = await this._azureBlobStorageService.UploadAsync(request.UrlImg, ContainerEnum.logo);
                    else
                        request.UrlLogo = await this._azureBlobStorageService.UploadAsync(request.UrlImg, ContainerEnum.logo, patrocinador.UrlLogo);
                }

                patrocinador.NombreCompleto = request.NombreCompleto;
                patrocinador.Email = request.Email;
                patrocinador.NombreEmpresa = request.NombreEmpresa;
                patrocinador.UrlLogo = request.UrlLogo;
                patrocinador.FechaUpd = DateTime.Now;
                patrocinador.UserUpd = UserUpd;
                
                var update = await _patrocinadorRepository.Update(patrocinador, _logger);
                if (update != null)
                {
                    response.Message = "Se edito correctamente el patrocinador";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se edito correctamente el patrocinador";
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


        public async Task<GenericResponse<PatrocinadorRequest>> EliminarPatrocinador(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PatrocinadorRequest>();
                var patrocinador = await _patrocinadorRepository.GetById(id, _logger);
                if (patrocinador == null)
                {
                    response.Message = "El patrocinador no existe";
                    return response;
                }

                patrocinador.Activo = false;


                var update = await _patrocinadorRepository.Update(patrocinador, _logger);
                if (update != null)
                {
                    response.Message = "Se elimino correctamente el patrocinador";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se elimino correctamente el servicio";
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
