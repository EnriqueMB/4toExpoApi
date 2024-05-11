using _4toExpoApi.Core.Enums;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class InformacionService
    {
        #region <---Variables--->
        private readonly IBaseRepository<Informacion> _InformacionRepository;
        private ILogger<InformacionService> _logger;
        private readonly IAzureBlobStorageService _azureBlobStorageService;
        #endregion

        #region <---Constructor--->
        public InformacionService(IBaseRepository<Informacion> informacionRepository, ILogger<InformacionService> logger, IAzureBlobStorageService azureBlobStorageService)
        {
            _InformacionRepository = informacionRepository;
            _logger = logger;
            _azureBlobStorageService = azureBlobStorageService;
        }

        #endregion

        #region <---Metodos--->
        public async Task<List<InformacionRequest>> ObtenerInformacion()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var informacion = await _InformacionRepository.GetAll(_logger);

                if (informacion == null || !informacion.Any())
                {
                    return null;
                }

                //var informacionActivos = informacion.Where(x => x.Activo).ToList();
                var listaInformacion = informacion.Select(t => new InformacionRequest
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    SubTitulo = t.SubTitulo,
                    UrlImagen = t.UrlImagen,
                    Texto = t.Texto,

                }).ToList();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return listaInformacion;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task<GenericResponse<InformacionRequest>> EditarInformacion(InformacionRequest request)
        {
            var response = new GenericResponse<InformacionRequest>();
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var informacion = await _InformacionRepository.GetById(request.Id, _logger);

                if (informacion == null)
                {
                    _logger.LogError($"No se encontró la información con el ID: {request.Id}");
                    
                    return response;
                }

                informacion.Titulo = request.Titulo;
                informacion.SubTitulo = request.SubTitulo;
                informacion.UrlImagen = request.UrlImagen;
                informacion.Texto = request.Texto;

                if (request.ImagenFile != null)
                {
                    if (request.UrlImagen == "null" || request.UrlImagen == null)
                        request.UrlImagen = await _azureBlobStorageService.UploadAsync(request.ImagenFile, ContainerEnum.multimedia);
                    else
                        request.UrlImagen = await _azureBlobStorageService.UploadAsync(request.ImagenFile, ContainerEnum.multimedia, request.UrlImagen);

                    informacion.UrlImagen = request.UrlImagen;
                }

                await _InformacionRepository.Update(informacion, _logger);
                response.Data = request;
                response.Success = true;

                _logger.LogInformation($"{MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name} Completed Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                response.Success = false;
                throw;
            }
            return response;
        }
        #endregion
    }
}