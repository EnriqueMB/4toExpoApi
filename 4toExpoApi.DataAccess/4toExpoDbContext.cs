using _4toExpoApi.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace _4toExpoApi.DataAccess
{
    public  class _4toExpoDbContext: DbContext
    {
        public _4toExpoDbContext(DbContextOptions<_4toExpoDbContext> options):base(options)
        {
            
        }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<UsuarioPermisos> UsuarioPermisos { get; set; }
        public DbSet<UsuariosRoles> UsuariosRoles { get; set; }
    }
}
