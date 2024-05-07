

using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.Core.ViewModels;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Azure;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace _4toExpoApi.Core.Services
{
    public class PaquetePatrocinadorService
    {
        #region <---Variables--->
        private readonly IPaquetePatrocinadoresRepository _paquetePatrocinadoresRepository;
        private readonly IBaseRepository<PaquetePatrocinadores> _basePaqueteRepository;
        private readonly IBaseRepository<BeneficioPaquete> _baseBeneficioRepository;
        private readonly ILogger<PaquetePatrocinadorService> _logger;
        private readonly IBaseRepository<TipoPaquete> _baseTipoPaqueteRepository;
        #endregion

        #region <---Constructor--->
        public PaquetePatrocinadorService(IPaquetePatrocinadoresRepository paquetePatrocinadoresRepository, ILogger<PaquetePatrocinadorService> logger,
            IBaseRepository<PaquetePatrocinadores> baseRepository, IBaseRepository<BeneficioPaquete> baseBeneficioRepository,
            IBaseRepository<TipoPaquete> baseTipoPaqueteRepository)
        {
            _paquetePatrocinadoresRepository = paquetePatrocinadoresRepository;
            _logger = logger;
            _basePaqueteRepository = baseRepository;
            _baseBeneficioRepository = baseBeneficioRepository;
            _baseTipoPaqueteRepository = baseTipoPaqueteRepository;
        }
        #endregion
        #region <---Metodos--->

        public async Task<GenericResponse> AgregarPaquete(PaquetePatrocinadoresVM paqueteRequest, int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();
                var Beneficios = new List<BeneficioPaquete>();
                if (paqueteRequest.BeneficioPaquete == null)
                {
                    Beneficios = null;
                }
                else
                {
                    foreach (var paqueteBeneficio in paqueteRequest.BeneficioPaquete)
                    {
                        Beneficios.Add(AppMapper.Map<BeneficioPaqueteRequest, BeneficioPaquete>(paqueteBeneficio));

                    }

                }
                var paquetes = AppMapper.Map<PaquetePatrocinadoresRequest, PaquetePatrocinadores>(paqueteRequest.PaquetePatrocinador);
                paquetes.UserAlt = userAlt;
                paquetes.FechaAlt = DateTime.Now;
                var result = await _paquetePatrocinadoresRepository.AgregarPaquete(paquetes, Beneficios, userAlt, _logger);

                if (result.Success == true)
                {
                    response.Success = true;
                    response.Message = result.Message;
                    response.CreatedId = result.CreatedId;
                }
                else
                {
                    response.Success = false;
                    response.Message = result.Message;

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

        public async Task<List<PaqueteBeneficiosPaVM>> ObtenerPaquetes()
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listaPaquetes = await _basePaqueteRepository.GetAll(_logger);

                var listaBeneficios = await _baseBeneficioRepository.GetAll(_logger);

                var tipoPaquete = await _baseTipoPaqueteRepository.GetAll(_logger);

                listaPaquetes.Where(x => x.Activo == true).ToList();
                listaBeneficios.Where(x => x.Activo == true).ToList();
                tipoPaquete.Where(x => x.Activo == true).ToList();


                var listaVM = (from paquete in listaPaquetes
                               join beneficio in listaBeneficios on paquete.Id equals beneficio.IdPaquetePatrocinador into beneficiosDelPaquete
                               join tipo in tipoPaquete on paquete.IdTipoPaquete equals tipo.Id
                               select new PaqueteBeneficiosPaVM
                               {
                                   Id = paquete.Id,
                                   TipoPaquete = tipo.Nombre,
                                   NombrePaquete = paquete.NombrePaquete,
                                   Descripcion = paquete.Descripcion,
                                   Precio = paquete.Precio,
                                   listaIncluye = beneficiosDelPaquete.Select(x => AppMapper.Map<BeneficioPaquete, BeneficioPaqueteRequest>(x)).ToList()



                               }).ToList();




                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return listaVM;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> EditarPaquete(PaquetePatrocinadoresVM paqueteVM, int userUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse();
                var Beneficios = new List<BeneficioPaquete>();
                if (paqueteVM.BeneficioPaquete == null)
                {
                    Beneficios = null;
                }
                else
                {
                    foreach (var paqueteBeneficio in paqueteVM.BeneficioPaquete)
                    {
                        Beneficios.Add(AppMapper.Map<BeneficioPaqueteRequest, BeneficioPaquete>(paqueteBeneficio));

                    }

                }
                var paquetes = AppMapper.Map<PaquetePatrocinadoresRequest, PaquetePatrocinadores>(paqueteVM.PaquetePatrocinador);
                paquetes.UserUpd = userUpd;
                paquetes.FechaUpd = DateTime.Now;
                var result = await _paquetePatrocinadoresRepository.EditarPaquete(paquetes, Beneficios, userUpd, _logger);

                if (result.Success == true)
                {
                    response.Success = true;
                    response.Message = result.Message;
                    response.CreatedId = result.CreatedId;
                }
                else
                {
                    response.Success = false;
                    response.Message = result.Message;

                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }

            #endregion

        }
    }
}
