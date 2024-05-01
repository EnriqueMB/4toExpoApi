using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Repositories
{
    public class ReservaRepository : BaseRepository<Reserva> ,IReservaRepository
    {
        private readonly _4toExpoDbContext _dbContext;

        public ReservaRepository(_4toExpoDbContext context) : base(context)
        {
            _dbContext = context;

        }

        public async Task<GenericResponse<Reserva>> AgregarReserva(Reserva persona, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Reserva>();

                var add = _context.Add(persona);

                var addResult = await _context.SaveChangesAsync();

                if (addResult > 0)
                {

                    await transaction.CommitAsync();
                    response.Success = true;
                    response.CreatedId = add.Entity.Id.ToString();
                    response.Data = add.Entity;

                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se pudo agregar la reserva";
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
    }
}
