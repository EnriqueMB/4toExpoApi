using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.Mappers;
using _4toExpoApi.Core.Request;
using _4toExpoApi.Core.Response;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Services
{
    public class PaqueteGeneralService
    {
        #region <--Variables -->
        private readonly IPaqueteGeneralRepository _paqueteGeneralRepository;
        private readonly IBaseRepository<IncluyePaquete> _incluyePaqueteRepository;
        private ILogger<PaqueteGeneralService> _logger;
        #endregion

        #region <-- Constructor-->
        public PaqueteGeneralService(IPaqueteGeneralRepository paqueteGeneralRepository,
            ILogger<PaqueteGeneralService> logger,IBaseRepository<IncluyePaquete> incluyePaquete) 
        { 
            _paqueteGeneralRepository = paqueteGeneralRepository;
            _logger = logger;
            _incluyePaqueteRepository = incluyePaquete;
        }
        #endregion

        #region
        public async Task<GenericResponse> AgregarPaquetesGeneral(PaqueteGeneralRequest request,int userAlt)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse();

                var paqueteGeneral = AppMapper.Map<PaqueteGeneralRequest, PaqueteGeneral>(request);

                paqueteGeneral.FechaAlt = DateTime.Now;
                paqueteGeneral.UserAlt = userAlt;
                paqueteGeneral.Activo = true;

                var listaPaquetes = AppMapper.Map<List<IncluyePaqueteRequest>, List<IncluyePaquete>>(request.ListaPaquetes);

                foreach( var item in listaPaquetes)
                {
                    item.UserAlt = userAlt;
                    item.FechaAlt = DateTime.Now;
                    item.Activo = true;
                }

                var result = await _paqueteGeneralRepository.AgregarPaqueteGeneral(paqueteGeneral, listaPaquetes,_logger);

                if (result.Success)
                {
                    response.Message = "Paquete agregado correctamente";
                    response.Success = true;
                    response.CreatedId = result.CreatedId;
                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch(Exception ex) 
            {
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse> EditarPaqueteGeneral(PaqueteGeneralRequest request,int UserUpd)
        {
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                
                var response = new GenericResponse();
                var paqueteGeneral = await _paqueteGeneralRepository.GetById(request.Id, _logger);

                if(paqueteGeneral == null)
                {
                    response.Message = "El paquete no existe";
                    return response;
                }

                paqueteGeneral.Nombre = request.Nombre;
                paqueteGeneral.Descripcion = request.Nombre;
                paqueteGeneral.Precio = request.Precio;
                paqueteGeneral.UserUpd = UserUpd;
                paqueteGeneral.FechaUpd = DateTime.Now;

                var listaPaquetesDb = await _paqueteGeneralRepository.GetByPaqueteGeneralId(request.Id, _logger);

                if (listaPaquetesDb != null)
                {
                    foreach (var item in request.ListaPaquetes)
                    {
                        // Verifica si el elemento ya existe en la db
                        var listadbExiste = listaPaquetesDb.FirstOrDefault(x => x.Id == item.Id);

                        if (listadbExiste != null)
                        {
                            listadbExiste.Nombre = item.Nombre;
                            listadbExiste.FechaUpd = DateTime.Now;
                            listadbExiste.UserUpd = UserUpd;

                            await _incluyePaqueteRepository.Update(listadbExiste, _logger);
                        }
                        else
                        {
                            //si no existe se Agrega el nuevo elemento a la base de datos
                            var nuevoElementoPaquete = AppMapper.Map<IncluyePaqueteRequest, IncluyePaquete>(item);

                            nuevoElementoPaquete.PaqueteId = paqueteGeneral.Id;
                            nuevoElementoPaquete.FechaAlt = DateTime.Now;
                            nuevoElementoPaquete.UserAlt = UserUpd;

                            await _incluyePaqueteRepository.Add(nuevoElementoPaquete, _logger);
                        }
                    }
                }

                var update = await _paqueteGeneralRepository.Update(paqueteGeneral, _logger);

                if (update != null )
                {
                  

                    response.Message = "Se edito correctamente el paquete";
                    response.Success = true;
                    response.UpdatedId = update.Id.ToString();
                }
                else
                {
                    response.Message = "No se edito el paquete";
                    response.Success = false;

                }
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;


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
