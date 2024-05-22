using _4toExpoApi.Core.Services;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.DataAccess.IRepositories;
using _4toExpoApi.DataAccess.Repositories;
using _4toExpoApi.DataAccess;
using Microsoft.EntityFrameworkCore;
using _4toExpoApi.Core.Helpers;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using _4toExpoApi.Core.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddJWTTokenServices(builder.Configuration);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

#region <-- Services -->
//builder.Services.AddScoped<AuthService>();
//builder.Services.AddScoped<PermisosService>();
builder.Services.AddScoped<ReservaService>();
builder.Services.AddScoped<ServicioService>();
builder.Services.AddScoped<TalleresService>();

builder.Services.AddScoped<ProductoServise>();
builder.Services.AddScoped<PaquetePatrocinadorService>();
builder.Services.AddScoped<ContadorService>();
builder.Services.AddScoped<BannerConfigService>();
builder.Services.AddScoped<PaqueteGeneralService>();
builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();    
builder.Services.AddScoped<InformacionService>();

builder.Services.AddScoped<MultimediaServise>();
builder.Services.AddScoped<BolsaTrabajoService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PatrocinadorService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UniversidadService>();
builder.Services.AddScoped<PreguntasService>();
//builder.Services.AddScoped<>

builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<BannerService>();
#endregion
#region <-- Repositories -->
//builder.Services.AddScoped<IBaseRepository<Permisos>, BaseRepository<Permisos>>();
builder.Services.AddScoped<IBaseRepository<Usuarios>, BaseRepository<Usuarios>>();
//builder.Services.AddScoped<IBaseRepository<UsuarioPermisos>, BaseRepository<UsuarioPermisos>>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IBaseRepository<Reservas>, BaseRepository<Reservas>>();
builder.Services.AddScoped<IBaseRepository<Servicios>, BaseRepository<Servicios>>();
builder.Services.AddScoped<IBaseRepository<Talleres>, BaseRepository<Talleres>>();

builder.Services.AddScoped<IBaseRepository<Productos>, BaseRepository<Productos>>();
builder.Services.AddScoped<IBaseRepository<Contador>, BaseRepository<Contador>>();
builder.Services.AddScoped<IBaseRepository<BannerConfig>, BaseRepository<BannerConfig>>();
builder.Services.AddScoped<IBaseRepository<Universidad>, BaseRepository<Universidad>>();
builder.Services.AddScoped<IBaseRepository<IncluyePaquete>, BaseRepository<IncluyePaquete>>();

builder.Services.AddScoped<IBaseRepository<Informacion>,BaseRepository<Informacion>>();

builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IBaseRepository<PaqueteGeneral>, BaseRepository<PaqueteGeneral>>();
builder.Services.AddScoped<IPaqueteGeneralRepository, PaqueteGeneralRepository>();
builder.Services.AddScoped<IBaseRepository<CMultimedia>, BaseRepository<CMultimedia>>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IBaseRepository<TipoPaquete>, BaseRepository<TipoPaquete>>();
builder.Services.AddScoped<IBaseRepository<BeneficioPaquete>, BaseRepository<BeneficioPaquete>>();
builder.Services.AddScoped<IBaseRepository<PaquetePatrocinadores>, BaseRepository<PaquetePatrocinadores>>();
builder.Services.AddScoped<IPaquetePatrocinadoresRepository, PaquetesPatrocinadoresRepository>();
builder.Services.AddScoped<IBaseRepository<BolsaTrabajo>, BaseRepository<BolsaTrabajo>>();
builder.Services.AddScoped<IBaseRepository<Patrocinadores>, BaseRepository<Patrocinadores>>();
builder.Services.AddScoped<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IBaseRepository<Preguntas>, BaseRepository<Preguntas>>();
builder.Services.AddScoped<IBaseRepository<Banner>, BaseRepository<Banner>>();
builder.Services.AddScoped<IBaseRepository<RedSocial>, BaseRepository<RedSocial>>();
#endregion
#region <-- Context -->

builder.Services.AddDbContext<_4toExpoDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), sqlServerOptionsAction => sqlServerOptionsAction.CommandTimeout(120));
});

#endregion

builder.Services.AddMvc();

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
#region CultureSetup
var supportedCultures = new[]
{
 new CultureInfo("es-MX")
};
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("es-MX"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

#endregion
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.EnvironmentName.Equals("QA"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
#region MIDDLEWARE FILTER
app.UseLimitMiddleware();
#endregion
app.UseAuthorization();

app.MapControllers();

app.Run();
