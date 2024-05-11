using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Repositories
{
    public class PaqueteGeneralRepository : BaseRepository<PaqueteGeneral> , IPaqueteGeneralRepository
    {
        private readonly _4toExpoDbContext _dbContext;

        public PaqueteGeneralRepository(_4toExpoDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<GenericResponse<PaqueteGeneral>> AgregarPaqueteGeneral(PaqueteGeneral paqueteGeneral, List<IncluyePaquete> listaPaquete, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse<PaqueteGeneral>();

                var addPaquete = _context.Add(paqueteGeneral);
                var resultAdd = await _context.SaveChangesAsync();
                
                if(resultAdd > 0)
                {
                    var paqueteId = addPaquete.Entity.Id;

                    foreach(var item in listaPaquete)
                    {
                        item.PaqueteId = paqueteId;
                    }
                    await _context.AddRangeAsync(listaPaquete);
                    var resultIncluye = await _context.SaveChangesAsync();
                    
                    if(resultIncluye > 0)
                    {
                        await transaction.CommitAsync();
                        response.Success = true;
                        response.CreatedId = addPaquete.Entity.Id.ToString();
                        response.Data = addPaquete.Entity;
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo agregar la lista de lo que incluye";
                    }
                   
                }
                else
                {
                    await transaction.RollbackAsync(); 
                    response.Success = false;
                    response.Message = "No se pudo agregar el paquete";
                }

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;

            }
            catch (SqlException ex)
            {
                await transaction.RollbackAsync();
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<List<IncluyePaquete>> GetByPaqueteGeneralId(int idPaquete, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var listaPaquete = await _context.IncluyePaquete
                                    .Where(ip => ip.PaqueteId == idPaquete && ip.Activo == true).ToListAsync();

                return listaPaquete;

            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }

        }
    }
}
