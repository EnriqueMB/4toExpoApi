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
    public interface IPatrocinadoresRepository : IBaseRepository<Patrocinadores>
    {
        Task<GenericResponse<Patrocinadores>> AgregarPatrocinador(Patrocinadores patrocinadores, Usuarios usuarios, ILogger logger);
        Task<GenericResponse<Patrocinadores>> EditarPatrocinador(Patrocinadores patrocinadores, Usuarios usuarios, ILogger logger);
        Task<GenericResponse<Usuarios>> ExistsByNombreUsuario(string email, ILogger logger, int Id);
    }
}
