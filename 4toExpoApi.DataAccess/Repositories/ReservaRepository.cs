﻿using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Azure;
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
    public class ReservaRepository : BaseRepository<Pagos> ,IReservaRepository
    {
        private readonly _4toExpoDbContext _dbContext;

        public ReservaRepository(_4toExpoDbContext context) : base(context)
        {
            _dbContext = context;

        }

        public async Task<GenericResponse<Pagos>> AgregarReserva(Pagos pagos, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Pagos>();

                var addPagos = _context.Add(pagos);

                var addResult = await _context.SaveChangesAsync();

                if (addResult > 0)
                {

                    await transaction.CommitAsync();
                    response.Success = true;
                    response.CreatedId = addPagos.Entity.Id.ToString();
                    response.Data = addPagos.Entity;

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

        public async Task<GenericResponse<Reservas>> ConfirmarPago(int id, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Reservas>();
                var findReservartion = _dbContext.Reservas.Where(x=>x.Id == id).FirstOrDefault();
                if(findReservartion != null) {
                findReservartion.ConfirmarCompra = true;
                    _dbContext.Reservas.Update(findReservartion);
                }
                var addResult = await _dbContext.SaveChangesAsync();

                if (addResult > 0)
                {
                    response.Success = true;
                    response.CreatedId = findReservartion.Id.ToString();
                    response.Data = findReservartion;

                }
                else
                {
                    
                    response.Success = false;
                    response.Message = "No se pudo confirmar";
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
    }
}
