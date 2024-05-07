using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Repositories
{
    public class HotelRepository : BaseRepository<Hotel>, IHotelRepository
    {
        private readonly _4toExpoDbContext _dbContext;

        public HotelRepository(_4toExpoDbContext context) : base(context)
        {
            _dbContext = context;
        }

        public async Task<GenericResponse<Hotel>> AgregarHotel(Hotel hotel, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Hotel>();

                var addHotel = _dbContext.Add(hotel);

                var addResult = await _dbContext.SaveChangesAsync();

                response.Success = true;
                response.CreatedId = addHotel.Entity.Id.ToString();
                response.Data = addHotel.Entity;

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
