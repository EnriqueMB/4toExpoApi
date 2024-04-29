using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
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
    public class PermisosService
    {
        #region <--- Variables --->
        private readonly IBaseRepository<Permisos> _repository;
        private ILogger<PermisosService> _logger;
        #endregion

        #region <--- Constructor --->
        public PermisosService(IBaseRepository<Permisos> repository, ILogger<PermisosService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        #endregion

        #region <--- Metodos --->
        public async Task<ListResponse<PermisosVM>> ObtenerPermisos()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new ListResponse<PermisosVM>();

                var permisos = await _repository.GetAll(_logger);

                var permisosVM = AppMapper.Map<List<Permisos>, List<PermisosVM>>(permisos.ToList());

                response.Data = permisosVM;

                response.Total = permisosVM.Count;

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Obtener permisos por tipo permiso
        public async Task<ListResponse<PermisosVM>> ObtenerPermisosPorTipoPermiso(int tipoPermiso)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new ListResponse<PermisosVM>();

                var permisos = await _repository.GetAll(_logger);

                permisos = permisos.Where(x => x.TipoPermiso == tipoPermiso);

                var permisosVM = AppMapper.Map<List<Permisos>, List<PermisosVM>>(permisos.ToList());

                response.Data = permisosVM;

                response.Total = permisosVM.Count;

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
