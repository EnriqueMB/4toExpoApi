using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.Response;
using _4toExpoApi.DataAccess.Response.Hotel;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.IRepositories
{
    public interface IHotelRepository : IBaseRepository<Hotel>
    {
        Task<GenericResponse<Hotel>> AgregarHotel(Hotel hotel, List<Habitacion> listHabitacion, List<Distancia>listaDistancia, int userAlt, ILogger logger);

        Task<GenericResponse<List<HotelResponse>>> ListaHotel(ILogger logger);
    }
}
