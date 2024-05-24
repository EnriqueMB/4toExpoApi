using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Azure.Core;
using Azure;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace _4toExpoApi.DataAccess.Repositories
{
    public class PatrocinadoresRepository : BaseRepository<Patrocinadores>, IPatrocinadoresRepository
    {
        public readonly _4toExpoDbContext _dbContext;

        public PatrocinadoresRepository(_4toExpoDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GenericResponse<Usuarios>> ExistsByNombreUsuario(string email, ILogger logger, int Id)
        {
         
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");
                var response = new GenericResponse<Usuarios>();

                var user = await _context.Usuarios.Where(x => x.Correo == email && (Id != 0? x.Id != Id && x.Activo == true : x.Activo == true) ).FirstOrDefaultAsync();
                if (user != null)
                {
                    response.Data =user;
                    response.Success = false;
                    response.Message = "Email ya existe";
                    logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                }
                else
                {
                    response.Data = new Usuarios();
                    response.Success = true;
                }
                

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<Patrocinadores>> AgregarPatrocinador(Patrocinadores patrocinadores, Usuarios usuarios, ILogger _logger)
        {
            using var trasaction = _context.Database.BeginTransaction();
            try
            {   
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Patrocinadores>();

                var patro = _context.Add(usuarios);

                var add = await _context.SaveChangesAsync();
                if (add == 1)
                {
                    patrocinadores.IdUsuario = patro.Entity.Id;
                    var usuario = _context.Patrocinadores.Add(patrocinadores);
                    var userAdd =  await _context.SaveChangesAsync();
                    if (userAdd == 1)
                    {
                        await trasaction.CommitAsync();
                        response.Message = "Se agrego correctamente el patrocinador";
                        response.Success = true;
                        response.CreatedId = patro.Entity.Id.ToString();
                    }
                    else
                    {
                        await trasaction.RollbackAsync();
                        response.Message = "No se pudo agregar correctamente el patrocinador";
                        response.Success = false;
                    }
                }
                else
                {
                    await trasaction.RollbackAsync();
                    response.Message = "No se pudo agregar correctamente el usuario del patrocinador";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return response;
            }
            catch (SqlException error)
            {
                await trasaction.RollbackAsync();
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + error.Message);
                throw;
            }
        }

        public async Task<GenericResponse<Patrocinadores>> EditarPatrocinador(Patrocinadores patrocinadores, Usuarios usuarios, ILogger _logger)
        {
            using var trasaction = _context.Database.BeginTransaction();
            try
            {
                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Patrocinadores>();

                var patro = _context.Patrocinadores.Update(patrocinadores);

                var add = await _context.SaveChangesAsync();
                if (add == 2)
                {

                    var usuario = _context.Usuarios.Update(usuarios);
                    var userAdd = await _context.SaveChangesAsync();
                    if (userAdd == 1)
                    {
                        await trasaction.CommitAsync();
                        response.Message = "Se edito correctamente el patrocinador";
                        response.Success = true;
                        response.CreatedId = patro.Entity.Id.ToString();
                    }
                    else
                    {
                        await trasaction.RollbackAsync();
                        response.Message = "No se pudo editar correctamente el usuario del patrocinador";
                        response.Success = false;
                    }
                }
                else
                {
                    await trasaction.RollbackAsync();
                    response.Message = "No se pudo editar correctamente el patrocinador";
                    response.Success = false;
                }

                _logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return response;
            }
            catch (SqlException error)
            {
                await trasaction.RollbackAsync();
                _logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + error.Message);
                throw;
            }
        }
    }
}
