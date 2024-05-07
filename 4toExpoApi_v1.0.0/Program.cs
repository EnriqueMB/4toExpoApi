using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Repositories;
using _4toExpoApi.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

#region <-- Services -->
//builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<PermisosService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<ServicioService>();
builder.Services.AddScoped<PaquetePatrocinadorService>();

#endregion
#region <-- Repositories -->
//builder.Services.AddScoped<IBaseRepository<Permisos>, BaseRepository<Permisos>>();
//builder.Services.AddScoped<IBaseRepository<Usuarios>, BaseRepository<Usuarios>>();
//builder.Services.AddScoped<IBaseRepository<UsuarioPermisos>, BaseRepository<UsuarioPermisos>>();
//builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IBaseRepository<Reserva>, BaseRepository<Reserva>>();
builder.Services.AddScoped<IBaseRepository<Servicios>, BaseRepository<Servicios>>();
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IBaseRepository<TipoPaquete>, BaseRepository<TipoPaquete>>();
builder.Services.AddScoped<IBaseRepository<BeneficioPaquete>, BaseRepository<BeneficioPaquete>>();
builder.Services.AddScoped<IBaseRepository<PaquetePatrocinadores>, BaseRepository<PaquetePatrocinadores>>();
builder.Services.AddScoped<IPaquetePatrocinadoresRepository, PaquetesPatrocinadoresRepository>();
#endregion
#region <-- Context -->

builder.Services.AddDbContext<_4toExpoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(120));
});

#endregion

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("*");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
