
using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using AbogadosApiV1.Core.Request;
using Azure;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace _4toExpoApi.Core.Services
{
    public class PromocionService
    {
        #region <---Variables--->
        private readonly IBaseRepository<Promocion> _promocionRepository;
        private ILogger<PromocionService> _logger;
        #endregion
        #region <---Constructor--->
        public PromocionService(IBaseRepository<Promocion> promocionRepository, ILogger<PromocionService> logger)
        {
            _promocionRepository = promocionRepository;
            _logger = logger;
        }
        #endregion
        #region <---Metodos--->
        public async Task<GenericResponse<PromocionRequest>> AgregarPromocion(PromocionRequest request, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PromocionRequest>();

                var promo = AppMapper.Map<PromocionRequest, Promocion>(request);
                promo.UserAlt = userAlt;
                promo.FechaAlt = HoraHelper.GetHora("mx");
                promo.Activo = true;

                var save = await _promocionRepository.Add(promo, _logger);

                if(save.IdPromocion > 0)
                {
                    response.Message = "Se agrego la promocion correctamente";
                    response.Success = true;
                    response.Data = request;
                    response.CreatedId = save.IdPromocion.ToString();
                }
                else
                {
                    response.Message = "Ocurrio un error al agregar la promocion";
                    response.Success = false;
                    response.Data = request;
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


        public async Task<ListResponse<PromocionRequest>> ObtenerPromocionesV1Pag(PaginadoRequest pag)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var listResponse = new ListResponse<PromocionRequest>();

                Expression<Func<Promocion, bool>> query;

                if (!string.IsNullOrEmpty(pag.Buscar))
                {
                    query = x => x.Activo == true && (
                    x.Descripcion.ToLower().Contains(pag.Buscar.ToLower()) ||
                    x.Porcentage.ToString().Contains(pag.Buscar) ||
                    x.NumeroPases.ToString().Contains(pag.Buscar) ||
                    x.PasesUsados.ToString().Contains(pag.Buscar) 
                    );
                }
                else
                {
                    query = x => x.Activo == true;
                }
                IEnumerable<Promocion> lista;
                if(pag.Take > -1)
                {
                     lista = await _promocionRepository.GetAll(_logger, [], query, pag.Skip, pag.Take);
                }
                else
                {
                    lista = await _promocionRepository.GetAll(_logger, [], query);

                }


                var listaReturn = lista.Select(x => new PromocionRequest
                {
                    IdPromocion = x.IdPromocion,
                    Descripcion = x.Descripcion,
                    PasesUsados = x.PasesUsados,
                    Porcentage = x.Porcentage,
                    NumeroPases = x.NumeroPases,
                    
                }).ToList();


                listResponse.Data = listaReturn;
                listResponse.Total = listaReturn.Count();
                listResponse.Success = true;
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return listResponse;

            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> EliminarPromocion(int id)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();

                var promo = await _promocionRepository.GetById(id, _logger);
                if (promo == null)
                {
                    response.Message = "No se encontro la promocion";
                    response.Success = false;
                    return response;
                }
                promo.Activo = false;

                var save = await _promocionRepository.Update(promo, _logger);

                if(save.IdPromocion == id)
                {
                    response.Success = true;
                    response.Message = "Se elimino correctamente la promocion";
                    response.DeletedId = id.ToString();
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se pudo eliminar la promocion";
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


        public async Task<GenericResponse<PromocionRequest>> EditarPromocion(PromocionRequest? request, int userUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PromocionRequest>();

              

                var promo = await _promocionRepository.GetById(request.IdPromocion, _logger);
                if(promo == null)
                {
                    response.Message = "No se encontro la promocion";
                    response.Success = false;
                    return response;
                }
                promo.Descripcion = request.Descripcion;
                promo.Porcentage = request.Porcentage;
                promo.PasesUsados = request.PasesUsados;
                promo.NumeroPases = request.NumeroPases;
                promo.UserUpd = userUpd;
                promo.FechaUpd = HoraHelper.GetHora("mx");


                var save = await _promocionRepository.Update(promo, _logger);
                if(save.IdPromocion == request.IdPromocion)
                {
                    response.Success = true;
                    response.Message = "Se edito la promocion";
                    response.UpdatedId = save.IdPromocion.ToString();
                    response.Data = request;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se edito la promocion";
                    response.Data = request;
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


        public async Task<ResponsePromoVM> ValidarPromocion(string codigo, int costoOriginal)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new ResponsePromoVM();
                var promo = (await _promocionRepository.GetAll(_logger, [], x => x.Codigo == codigo)).FirstOrDefault();

                if(promo == null ) 
                {
                    response.Success = false;
                    response.Message = "No hay descuento para esta promocion";
                    return response;
                }

                if(promo.PasesUsados >= promo.NumeroPases)
                {
                    response.Success = false;
                    response.Message = "Los pases Usados superan al numero de pases";
                    return response;

                }

                response.Success = true;
                response.Message = "Se aplico la promocion Correctamente";

                decimal? promoDesc = (promo.Porcentage * costoOriginal) / 100;
                response.Precio = costoOriginal - promoDesc;


                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return response;

            }catch(Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }
        #endregion
    }
}
