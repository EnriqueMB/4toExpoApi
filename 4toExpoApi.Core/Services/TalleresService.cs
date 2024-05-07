using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using Microsoft.Extensions.Logging;
using System.Reflection;
using _4toExpoApi.DataAccess.Response;
using _4toExpoApi.Core.Mappers;

namespace _4toExpoApi.Core.Services
{
    public class TalleresService
    {
        #region <---Variables--->
        private readonly IBaseRepository<Talleres> _talleresRepository;
        private ILogger<TalleresService> _logger;
        #endregion

        #region <---Constructor--->
        public TalleresService(IBaseRepository<Talleres> talleresRepository, ILogger<TalleresService> logger)
        {
            _talleresRepository = talleresRepository;
            _logger = logger;
        }
        #endregion

        #region <---Metodos--->

        public async Task<GenericResponse<TalleresRequest>> AgregarTaller(TalleresRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = new GenericResponse<TalleresRequest>();

                var taller = AppMapper.Map<TalleresRequest, Talleres>(request);

                taller.FechaAlt = DateTime.Now;
                taller.UserAlt = userAlt;
                taller.Activo = true;
                var result = await _talleresRepository.Add(taller, _logger);
                if (result != null && result.Id > 0)
                {
                    response.Data = request;
                    response.Message = "Taller agregado correctamente";
                    response.Success = true;
                    response.CreatedId = result.Id.ToString();
                }
                else
                {
                    response.Message = "Error al agregar el taller";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<List<TalleresRequest>> ObtenerTalleres()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var talleres = await _talleresRepository.GetAll(_logger);

                if (talleres == null || !talleres.Any())
                {
                    return null;
                }

                var talleresActivos = talleres.Where(x => x.Activo).ToList();
                var listaTalleres = talleresActivos.Select(t => new TalleresRequest
                {
                    Id = t.Id,
                    Nombre = t.Nombre,
                    Descripcion = t.Descripcion,
                    Fecha = t.Fecha,
                    Hora = t.Hora,
                }).ToList();

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return listaTalleres;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<TalleresRequest>> EliminarTalleres(int id)
        {
            var response = new GenericResponse<TalleresRequest>();
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var taller = await _talleresRepository.GetById(id, _logger);
                if (taller == null)
                {
                    response.Message = "El taller no existe";
                    response.Success = false;
                    return response;
                }

                taller.Activo = false;
                var result = await _talleresRepository.Update(taller, _logger);
                if (result != null)
                {
                    response.Message = "Taller eliminado correctamente";
                    response.Success = true;
                }
                else
                {
                    response.Message = "Error al eliminar el taller";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }
        public async Task<GenericResponse<TalleresRequest>> EditarTalleres(TalleresRequest request)
        {
            var response = new GenericResponse<TalleresRequest>();
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var taller = await _talleresRepository.GetById(request.Id, _logger);
                if (taller == null)
                {
                    response.Message = "El taller no existe";
                    response.Success = false;
                    return response;
                }

                taller.Nombre = request.Nombre;
                taller.Descripcion = request.Descripcion;
                taller.Fecha = request.Fecha;
                taller.Hora = request.Hora;
                taller.FechaUpd = DateTime.Now;

                var result = await _talleresRepository.Update(taller, _logger);
                if (result != null)
                {
                    response.Data = request;
                    response.Message = "Taller actualizado correctamente";
                    response.Success = true;
                }
                else
                {
                    response.Message = "Error al actualizar el taller";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " " + ex.Message);
                throw;
            }
        }

        #endregion

    }
}
