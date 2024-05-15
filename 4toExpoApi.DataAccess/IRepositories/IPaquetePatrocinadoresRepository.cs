
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.Response;
using Microsoft.Extensions.Logging;
namespace _4toExpoApi.DataAccess.IRepositories
{
    public interface IPaquetePatrocinadoresRepository : IBaseRepository<PaquetePatrocinadores>
    {
        Task<GenericResponse<PaquetePatrocinadores>> AgregarPaquete(PaquetePatrocinadores paquete, List<BeneficioPaquete>? beneficio, int userAlt, ILogger logger);
        Task<GenericResponse<PaquetePatrocinadores>> EditarPaquete(PaquetePatrocinadores paquete, List<BeneficioPaquete>? beneficio, int userUpd, ILogger logger);


    }
}
