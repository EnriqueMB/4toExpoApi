using _4toExpoApi.Core.Mappers;
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
    public class ContadorService
    {
        private readonly IBaseRepository<Contador> _contadorRepository;
        private readonly ILogger<ContadorService> _logger;
        public ContadorService(IBaseRepository<Contador> contadorRepository, ILogger<ContadorService> logger) { 
        
            _contadorRepository = contadorRepository;
            _logger = logger;
        }

        
        public async Task<ContadorRequets> Index()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var contador = await _contadorRepository.GetById(1, _logger);

                var datosReq = AppMapper.Map<Contador, ContadorRequets>(contador);

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return datosReq;
            }
            catch (Exception ex) {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<ContadorRequets>> EditarContador(ContadorRequets request, int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<ContadorRequets>();
                var contador = await _contadorRepository.GetById(request.Id, _logger);
               
                contador.Fecha = request.Fecha;
                contador.Descripcion = request.Descripcion;
               
                contador.UserUpd = UserUpd;
                contador.FechaUpd = DateTime.Now;
                var update = await _contadorRepository.Update(contador, _logger);
                if (update != null)
                {
                    response.Message = "Se edito correctamente el Contador";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se edito correctamente el Contador";
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
    }
}
