using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class MultimediaServise
    {
        private readonly IBaseRepository<CMultimedia> _MultimediaRepository;
        private ILogger<MultimediaServise> _logger;


        public MultimediaServise(IBaseRepository<CMultimedia> mulatimediaRepository, ILogger<MultimediaServise> logger)
        {
            _MultimediaRepository = mulatimediaRepository;
            _logger = logger;
        }
        public async Task<GenericResponse<CMultimedia>> UpdateMultimedia(CMultimedia request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<CMultimedia>();
                var multimedia = await _MultimediaRepository.GetById(request.Id, _logger);
                if (multimedia == null)
                {
                    response.Message = "No existe esa multimedia";
                    return response;
                }

                multimedia.Collage = request.Collage;
                multimedia.Mapa = request.Mapa;
                multimedia.Multiple = request.Multiple;

                var update = await _MultimediaRepository.Update(multimedia, _logger);
                if (update != null)
                {
                    response.Message = "Se pudo modificar exitosamente la Multimedia";
                    response.Success = true;
                }
                else
                {
                    response.Message = "No se puedo modificar la Multimedia";
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
        public async Task<CMultimedia> ObtenerMultimedia()
        {

            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var listMultimedia = await _MultimediaRepository.GetById(1, _logger);
                if(listMultimedia != null)
                {
                    return listMultimedia;
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
        
    }
}
