using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class InformacionConfigService
    {
        #region <---VARIABLES--->
        private readonly IBaseRepository<InformacionConfig> _informacionRepository; 
        private ILogger<InformacionConfigService> _logger;
        #endregion

        #region <---CONSTRUCTOR--->
        public InformacionConfigService(IBaseRepository<InformacionConfig> informacionRepository, ILogger<InformacionConfigService> logger)
        {
            _informacionRepository = informacionRepository;
            _logger = logger; 
        }
        #endregion

        #region <---METODOS--->
        #endregion
    }
}
