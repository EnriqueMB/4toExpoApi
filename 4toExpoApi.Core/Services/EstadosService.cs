using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using AbogadosApiV1.Core.Request;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static _4toExpoApi.Core.Helpers.JwtHelper;

namespace _4toExpoApi.Core.Services
{
    public class EstadosService
    {
        #region <---Variables--->
        private readonly IBaseRepository<Estados> _baseRepository;
        private ILogger<EstadosService> _logger;

        #endregion

        #region <---Constructor--->
        public EstadosService(IBaseRepository<Estados> baseRepository, ILogger<EstadosService> logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }
        #endregion

        #region <---Metodos--->

        public async Task<ListResponse<EstadosVM>> Estado(PaginadoRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.Name + " - Started Success");

                var response = new ListResponse<EstadosVM>();

                Expression<Func<Estados, bool>> query = x => true;
                Expression<Func<Estados, bool>> search = null;
                IEnumerable<Estados> estado;

                if (!string.IsNullOrEmpty(request.Buscar))
                {
                    search = x => (x.Estado.ToLower().Contains(request.Buscar.ToLower()));
                }
                if (request.Take == -1)
                {
                    estado = await _baseRepository.GetAll(_logger, [], search == null ? query : search);
                }
                else
                {
                    estado = await _baseRepository.GetAll(_logger, [], search == null ? query : search, request.Skip, request.Take);
                }
                var obtenerEstado = estado.Select(estado => new EstadosVM
                {
                    Clave= estado.Clave,
                    Estado = estado.Estado,
                    Abreviatura = estado.Abreviatura,
                }).ToList();
                response.Total = estado.Count();
                response.Data = obtenerEstado;

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.Name + " - Ended Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.Name + " - Error: " + ex.Message);
                return new ListResponse<EstadosVM>();
            }
        }
        public async Task<ListResponse<EstadosVM>> ObtenerEstados( )
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new ListResponse<EstadosVM>();

               

                var result = await _baseRepository.GetAll(_logger, []);


                response.Total = result.Count();
                response.Data = result.Select(x => new EstadosVM
                {
                    Clave = x.Clave,
                    Estado = x.Estado,
                    Abreviatura = x.Abreviatura,
                   

                }).ToList();





                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                return response;
            }
            catch (Exception ex)
            {

                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Error: " + ex.Message);
                throw;
            }


        }
        #endregion
    }
}
