
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace _4toExpoApi.DataAccess.Repositories
{

    public class PaquetesPatrocinadoresRepository : BaseRepository<PaquetePatrocinadores>, IPaquetePatrocinadoresRepository
    {
        private readonly _4toExpoDbContext _context;
  
        public PaquetesPatrocinadoresRepository(_4toExpoDbContext context) : base(context) 
        {
            _context = context;
        }
        public async Task<GenericResponse<PaquetePatrocinadores>> AgregarPaquete(PaquetePatrocinadores paquete, List<BeneficioPaquete>? beneficio, int userAlt, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PaquetePatrocinadores>();

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    var paquetes = await _context.PaquetePatrocinadores.AddAsync(paquete);

             
                    var save = await _context.SaveChangesAsync();

                    if (beneficio != null)
                    {
                        foreach (var item in beneficio)
                        {
                            item.IdPaquetePatrocinador = paquetes.Entity.Id;
                            item.Activo = true;
                            item.UserAlt = userAlt;
                            item.FechaAlt = DateTime.Now;
                        }
                        await _context.BeneficioPaquete.AddRangeAsync(beneficio);
                        save = await _context.SaveChangesAsync();
                    }
                    
                    if (save > 0)
                    {
                        response.Data = paquete;
                        response.Success = true;
                        response.Message = "Se agrego correctamente el paquete";
                        response.CreatedId = paquetes.Entity.Id.ToString();
                     
                        await transaction.CommitAsync();
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "No se pudo agregar correctamente el paquete";
                    }

                    logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                    throw;
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<PaquetePatrocinadores>> EditarPaquete(PaquetePatrocinadores paquete, List<BeneficioPaquete>? beneficio, int userUpd, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<PaquetePatrocinadores>();

                await using var transaction = await _context.Database.BeginTransactionAsync();

                try
                {
                    if(paquete != null)
                    {
                        
                        var paqueteEdit = await _context.PaquetePatrocinadores.FindAsync(paquete.Id);
                        if(paqueteEdit == null)
                        {
                            response.Message = "No se encontro el paquete";
                            return response;
                        }
                        paqueteEdit.Descripcion = paquete.Descripcion;
                        paqueteEdit.NombrePaquete = paquete.NombrePaquete;
                        paqueteEdit.IdTipoPaquete = paquete.IdTipoPaquete;
                        paqueteEdit.Precio = paquete.Precio;
                        paqueteEdit.UserUpd = paquete.UserUpd;
                        paqueteEdit.FechaUpd = paquete.FechaUpd;

                       _context.PaquetePatrocinadores.Update(paqueteEdit);

                        var beneficiosUpdate = new List<BeneficioPaquete>();
                        var beneficiosNuevos = new List<BeneficioPaquete>();
                        if (beneficio != null)
                        {

                            foreach (var item in beneficio)
                            {
                                if (item.Id == 0)
                                {
                                    item.IdPaquetePatrocinador = paquete.Id;

                                    item.UserAlt = userUpd;
                                    item.FechaAlt = DateTime.Now;
                                    beneficiosNuevos.Add(item);
                                }
                                else if (item.Id > 0)
                                {
                                    var beneficioSelect = await _context.BeneficioPaquete.FindAsync(item.Id);
                                    if (beneficioSelect != null)
                                    {
                                        beneficioSelect.Descripcion = item.Descripcion;
                                        beneficioSelect.TipoBeneficio = item.TipoBeneficio;
                                        beneficioSelect.UserUpd = userUpd;
                                        beneficioSelect.FechaUpd = DateTime.Now;
                                        beneficiosUpdate.Add(beneficioSelect);
                                    }

                                }

                            }
                            if (beneficiosNuevos.Count() > 0)
                            {
                                await _context.BeneficioPaquete.AddRangeAsync(beneficiosNuevos);
                            }
                            if (beneficiosUpdate.Count() > 0)
                            {
                                _context.BeneficioPaquete.UpdateRange(beneficiosUpdate);
                            }

                        }

                        var save = await _context.SaveChangesAsync();
                        if (save > 0)
                        {
                            response.Data = paquete;
                            response.Success = true;
                            response.Message = "Se edito correctamente el paquete";
                            response.CreatedId = paqueteEdit.Id.ToString();

                            await transaction.CommitAsync();
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "No se pudo editar correctamente el paquete";
                        }

                        logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                    }
                 


                  
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();

                    logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                    throw;
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

    }
}
