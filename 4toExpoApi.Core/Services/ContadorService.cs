using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
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
    }
}
