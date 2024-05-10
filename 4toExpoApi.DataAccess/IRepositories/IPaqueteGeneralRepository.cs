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
    public interface  IPaqueteGeneralRepository : IBaseRepository<PaqueteGeneral>
    {
        Task<GenericResponse<PaqueteGeneral>> AgregarPaqueteGeneral(PaqueteGeneral paqueteGeneral,List<IncluyePaquete>listaPaquete, ILogger logger);
        Task<List<IncluyePaquete>> GetByPaqueteGeneralId(int idPaquete,ILogger logger);

    }
}
