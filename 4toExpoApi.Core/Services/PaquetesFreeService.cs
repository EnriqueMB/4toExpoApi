using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Response;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using AbogadosApiV1.Core.Request;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class PaquetesFreeService
    {
        #region VARIABLES
        private readonly IBaseRepository<Reservas> _baseRepository;
        private readonly ILogger<PaquetesFreeService> _logger;
        #endregion

        #region CONSTRUCTOR
        public PaquetesFreeService(IBaseRepository<Reservas> baseRepository, ILogger<PaquetesFreeService> logger)
        {
            _baseRepository = baseRepository;
            _logger = logger;
        }
        #endregion

        #region MÉTODOS
        public async Task<ListResponse<Reservas>> GetFreePackageUsers(PaginadoRequest request)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = new ListResponse<Reservas>();

                Expression<Func<Reservas, bool>> query;

                if (!string.IsNullOrEmpty(request.Buscar))
                {
                    query = x => x.Activo == true && x.Usuarios!.Activo == true && x.Usuarios!.PaqueteGratis == true && ( x.PaqueteGeneral!.Nombre!.Contains(request.Buscar) 
                    || x.Usuarios.NombreCompleto!.Contains(request.Buscar) || x.Usuarios.Correo!.Contains(request.Buscar) || x.Usuarios.Estado!.Contains(request.Buscar) 
                    || x.Usuarios.Ciudad!.Contains(request.Buscar) || x.Usuarios.Telefono!.Contains(request.Buscar));
                }else
                    query = x => x.Activo == true && x.Usuarios!.Activo == true && x.Usuarios!.PaqueteGratis == true;

                var result = await _baseRepository.GetAll(_logger, ["Usuarios", "PaqueteGeneral"], query, request.Skip, request.Take);
                var total = await _baseRepository.GetCount(_logger, query);

                response.Data = result.ToList();
                response.Total = total;
                response.Success = true;

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                throw;
            }
        }

        public async Task<GenericResponse> ConfirmarCompra(int id, int user)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + " Started Success");

                var response = new GenericResponse();

                var data = await _baseRepository.GetById(id, _logger);

                if (data == null)
                {
                    response.Message = "La reserva no existe.";
                    response.Success = false;
                }
                else
                {
                    data.ConfirmarCompra = true;
                    data.UserUpd = user;
                    data.FechaUpd = HoraHelper.GetHora("mx");

                    var res = await _baseRepository.Update(data, _logger);
                    if (res != null)
                    {
                        response.Message = "Validado correctamente.";
                        response.Success = true;
                    }
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception)
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                throw;
            }
        }
        #endregion
    }
}
