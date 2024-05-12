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
   public class BolsaTrabajoService
    {
        #region <-----Variables----->
        private readonly IBaseRepository<BolsaTrabajo> _bolsaTrabajoRepository;
        private ILogger<BolsaTrabajoService> _logger;
        #endregion

        #region <-----Constructor----->
        public BolsaTrabajoService(IBaseRepository<BolsaTrabajo> bolsaTrabajoRepository, ILogger<BolsaTrabajoService> logger)
        {
            _bolsaTrabajoRepository = bolsaTrabajoRepository;
            _logger = logger;
        }
        #endregion

        #region <-----Metodos----->
        //public async Task<GenericResponse> BolsaTrabajo(BolsaTrabajoRequest request, int idUsuario)
        //{
        //    try
        //    {
        //        _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");



        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
        //}
        #endregion


    }
}
