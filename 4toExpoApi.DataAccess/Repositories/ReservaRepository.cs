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
    public class ReservaRepository : BaseRepository<Reservas> ,IReservaRepository
    {
        private readonly _4toExpoDbContext _dbContext;

        public ReservaRepository(_4toExpoDbContext context) : base(context)
        {
            _dbContext = context;

        }

        public async Task<GenericResponse<Reservas>> AgregarReserva(Reservas Reserva,Pagos pagos,Clientes clientes, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Reservas>();

                var addReserva = _context.Add(Reserva);

                var addResult = await _context.SaveChangesAsync();

                if (addResult > 0)
                {

                    var reservaId = addReserva.Entity.Id;
                    pagos.IdReserva = reservaId;

                    var addPagos = _context.Add(pagos);
                    var addResultPagos = await _context.SaveChangesAsync();

                    if(addResultPagos > 0)
                    {
                        clientes.IdReserva = reservaId;

                        var addCliente = _context.Add(clientes);
                        var addResultCliente = await _context.SaveChangesAsync();

                        if(addResultCliente > 0)
                        {
                            await transaction.CommitAsync();
                            response.Success = true;
                            response.CreatedId = addReserva.Entity.Id.ToString();
                            response.Data = addReserva.Entity;
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            response.Success = false;
                            response.Message = "No se pudo agregar el cliente";
                        }

                        
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se puso agregar el pago";
                    }

                    

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
