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
        public DbSet<Reservas> Reservas { get; set; }
        public DbSet<Pagos> Pagos { get; set; }
        public DbSet<Clientes> Clientes { get; set; }
        public DbSet<Servicios> Servicios { get; set; }
        public DbSet<PaquetePatrocinadores> PaquetePatrocinadores { get; set; }
        public DbSet<TipoPaquete> TipoPaquete { get; set; }
        public DbSet<BeneficioPaquete> BeneficioPaquete { get; set; }
        public DbSet<Contador> Contador { get; set;}
        public DbSet<PaqueteGeneral> PaqueteGeneral { get; set;}
        public DbSet<IncluyePaquete> IncluyePaquete { get; set; }

        public DbSet<Hotel> Hotel { get; set; }

        public DbSet<Habitacion> Habitacion { get; set; }

        public DbSet<Distancia> Distancia { get; set; }

    }
}
