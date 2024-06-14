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
    public interface IUsuarioRepository : IBaseRepository<Usuarios>
    {
        Task<Usuarios> ExistsByNombreUsuario(string correo, ILogger logger);
        Task<UsuariosPromocion> ExistsByNombreUsuarioPromo(string correo, ILogger logger);
        Task<GenericResponse<Usuarios>> AgregarUsuario(Usuarios usuario,Reservas reserva, ILogger logger);
        Task<GenericResponse<Usuarios>> ActualizarUsuario(Usuarios usuario, List<UsuarioPermisos> permisos, ILogger logger);
        Task<GenericResponse<List<int>>> ObtenerRolesUsuario(int id, ILogger logger);
        Task<GenericResponse<List<Usuarios>>> ObtenerUsuarios(ILogger logger);
        Task<GenericResponse<Usuarios>> ObtenerUsuarioPorId(int id, ILogger logger);
        Task<GenericResponse<Usuarios>> ExistsNombreUsuario(string nombreUsuario, int idUsuario, ILogger logger);
        Task<GenericResponse<Usuarios>> EliminarUsuario(int id, int userMod, ILogger logger);
    }
}
