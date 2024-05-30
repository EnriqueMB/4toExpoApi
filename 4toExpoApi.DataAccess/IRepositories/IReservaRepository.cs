using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.IRepositories
{
    public interface IReservaRepository : IBaseRepository<Pagos>
    {
        Task<GenericResponse<Pagos>> AgregarReserva(Pagos pagos, ILogger logger);
        Task<GenericResponse<Reservas>> ConfirmarPago(int id, ILogger logger);
    }
}
