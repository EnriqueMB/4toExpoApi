using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
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
    public class UniversidadService
    {
        #region <--Vaiables-->
        private readonly IBaseRepository<Universidad> _universidadRepository;
        private ILogger<UniversidadService> _logger;
        #endregion
        #region <--Contructor-->
        public UniversidadService(ILogger<UniversidadService> logger,IBaseRepository<Universidad> universidad)
        {
            _universidadRepository = universidad;
            _logger = logger;
        }
        #endregion

        #region <--Metodod-->
        public async Task<List<Universidad>> ObtenerUniversidades()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listaUni = await _universidadRepository.GetAll(_logger);

                var datosUni = new List<Universidad>(); 

                foreach (var item in listaUni)
                {
                    datosUni.Add(item);
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
               
                return datosUni;
            }
            catch(Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
