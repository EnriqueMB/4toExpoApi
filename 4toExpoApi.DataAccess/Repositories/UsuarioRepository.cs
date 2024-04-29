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
    public class UsuarioRepository : BaseRepository<Usuarios>, IUsuarioRepository
    {
        private readonly _4toExpoDbContext _dbContext;

        public UsuarioRepository(_4toExpoDbContext context) :  base(context)
        {
            _dbContext = context;
        }
        public async Task<Usuarios> ExistsByNombreUsuario(string nombreUsuario, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var user = EntitySet.AsNoTracking().Where(x => x.NombreUsuario == nombreUsuario && x.Activo == true).FirstOrDefault();

                if (user != null)
                {
                    logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                    return user;
                }

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");
                return null;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<Usuarios>> AgregarUsuario(Usuarios usuario, List<UsuarioPermisos> permisos, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Usuarios>();

                var add = _context.Add(usuario);

                var addResult = await _context.SaveChangesAsync();

                if (addResult > 0)
                {
                    foreach (var item in permisos)
                    {
                        item.IdUsuario = add.Entity.Id;
                    }

                    _context.AddRange(permisos);

                    var addPermisosResult = await _context.SaveChangesAsync();

                    if (addPermisosResult == permisos.Count)
                    {
                        await transaction.CommitAsync();
                        response.Success = true;
                        response.CreatedId = add.Entity.Id.ToString();
                        response.Data = add.Entity;
                        response.Message = "Se agrego el usuario correctamente";
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo agregar los permisos del usuario";
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se pudo agregar el usuario";
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

        public async Task<GenericResponse<Usuarios>> ActualizarUsuario(Usuarios usuario, List<UsuarioPermisos> permisos, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Usuarios>();

                var usr = await base.GetById(usuario.Id, logger);

                if (usr != null)
                {
                    usr.NombreCompleto = usuario.NombreCompleto;
                    usr.NombreUsuario = usuario.NombreUsuario;
                    usr.PasswordHash = usuario.PasswordHash;
                    usr.PasswordSalt = usuario.PasswordSalt;
                    usr.Correo = usuario.Correo;
                    usr.UserUpd = usuario.UserUpd;
                    usr.FechaUpd = usuario.FechaUpd;
                    usr.Activo = usuario.Activo;

                    var update = _context.Update(usr);

                    var updateResult = await _context.SaveChangesAsync();

                    if (updateResult == 1)
                    {
                        var permisosAnteriores = _context.UsuarioPermisos.Where(x => x.IdUsuario == usuario.Id).ToList();

                        _context.RemoveRange(permisosAnteriores);

                        var removeResult = await _context.SaveChangesAsync();

                        if (removeResult == permisosAnteriores.Count)
                        {
                            foreach (var item in permisos)
                            {
                                item.IdUsuario = usuario.Id;

                                _context.Add(item);
                            }

                            var addResult = await _context.SaveChangesAsync();

                            if (addResult == permisos.Count)
                            {
                                await transaction.CommitAsync();
                                response.Success = true;
                                response.UpdatedId = usr.Id.ToString();
                                response.Data = usr;
                                response.Message = "Se modifico el usuario correctamente";
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                response.Success = false;
                                response.Message = "No se pudo agregar el permiso del usuario";
                            }
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            response.Success = false;
                            response.Message = "No se pudo eliminar los permisos anteriores del usuario";
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo modificar el usuario";
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se encontro el usuario";
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

        public async Task<GenericResponse<Usuarios>> ActualizarUltimoAcceso(int id, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Usuarios>();

                var usr = await base.GetById(id, logger);

                if (usr != null)
                {
                    usr.UltimoAcceso = DateTime.UtcNow.AddHours(-6);

                    var update = _context.Update(usr);

                    var updateResult = await _context.SaveChangesAsync();

                    if (updateResult == 1)
                    {
                        response.Success = true;
                        response.UpdatedId = usr.Id.ToString();
                        response.Data = usr;
                        response.Message = "Se modifico el ultimo acceso del usuario correctamente";
                    }
                    else
                    {
                        response.Success = false;
                        response.Message = "No se pudo modificar el ultimo acceso del usuario";
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se encontro el usuario";
                }

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<List<long>>> ObtenerPermisosUsuario(int id, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<long>>();

                var permisos = await _context.UsuarioPermisos.Where(x => x.IdUsuario == id).Select(x => x.IdPermiso).ToListAsync();

                response.Success = true;
                response.Data = permisos;

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        public async Task<GenericResponse<List<int>>> ObtenerRolesUsuario(int id, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<int>>();

                var permisos = await _context.UsuariosRoles.Where(x => x.IdUsuario == id).Select(x => x.IdRol).ToListAsync();

                response.Success = true;
                response.Data = permisos;

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Obtener todos los usuarios
        public async Task<GenericResponse<List<Usuarios>>> ObtenerUsuarios(ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<Usuarios>>();

                var usuarios = await EntitySet.AsNoTracking().Where(x => x.Activo == true).ToListAsync();

                response.Success = true;
                response.Data = usuarios;

                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Finished Success");

                return response;
            }
            catch (SqlException ex)
            {
                logger.LogError(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + ex.Message);
                throw;
            }
        }

        //Obtener usuario por id
        public async Task<GenericResponse<Usuarios>> ObtenerUsuarioPorId(int id, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Usuarios>();

                var usuario = await EntitySet.AsNoTracking().Where(x => x.Id == id && x.Activo == true).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    response.Success = true;
                    response.Data = usuario;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se encontro el usuario";
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

        //Obtener usuario por nombre de usuario
        public async Task<GenericResponse<Usuarios>> ExistsNombreUsuario(string nombreUsuario, int idUsuario, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Usuarios>();

                var usuario = await EntitySet.AsNoTracking().Where(x => x.NombreUsuario == nombreUsuario && x.Id != idUsuario && x.Activo == true).FirstOrDefaultAsync();

                if (usuario != null)
                {
                    response.Success = true;
                    response.Data = usuario;
                }
                else
                {
                    response.Success = false;
                    response.Message = "No se encontro el usuario";
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

        //Eliminar usuario
        public async Task<GenericResponse<Usuarios>> EliminarUsuario(int id, int userMod, ILogger logger)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<Usuarios>();

                var usr = await base.GetById(id, logger);

                if (usr != null)
                {
                    usr.Activo = false;
                    usr.FechaUpd = DateTime.UtcNow.AddHours(-6);
                    usr.UserUpd = userMod;

                    var update = _context.Update(usr);

                    var updateResult = await _context.SaveChangesAsync();

                    if (updateResult == 1)
                    {
                        var permisosAnteriores = _context.UsuarioPermisos.Where(x => x.IdUsuario == id).ToList();

                        _context.RemoveRange(permisosAnteriores);

                        var removeResult = await _context.SaveChangesAsync();

                        if (removeResult == permisosAnteriores.Count)
                        {
                            await transaction.CommitAsync();
                            response.Success = true;
                            response.UpdatedId = usr.Id.ToString();
                            response.Data = usr;
                            response.Message = "Se elimino el usuario correctamente";
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            response.Success = false;
                            response.Message = "No se pudo eliminar los permisos anteriores del usuario";
                        }
                    }
                    else
                    {
                        await transaction.RollbackAsync();
                        response.Success = false;
                        response.Message = "No se pudo eliminar el usuario";
                    }
                }
                else
                {
                    await transaction.RollbackAsync();
                    response.Success = false;
                    response.Message = "No se encontro el usuario";
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

        //Obtener permisos por tipo permiso
        public async Task<GenericResponse<List<Permisos>>> ObtenerPermisosPorTipoPermiso(int tipoPermiso, ILogger logger)
        {
            try
            {
                logger.LogInformation(MethodBase.GetCurrentMethod().DeclaringType.DeclaringType.Name + "Started Success");

                var response = new GenericResponse<List<Permisos>>();

                var permisos = await _context.Permisos.Where(x => x.TipoPermiso == tipoPermiso).ToListAsync();

                response.Success = true;
                response.Data = permisos;

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
