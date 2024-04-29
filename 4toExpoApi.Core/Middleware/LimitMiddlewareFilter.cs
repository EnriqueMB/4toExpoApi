using _4toExpoApi.Core.Helpers;
using _4toExpoApi.DataAccess;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace _4toExpoApi.Core.Middleware
{
    public static class LimitMiddlewareFilter
    {
        public static IApplicationBuilder UseLimitMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LimitMiddlewareRequest>();
        }
    }
    public class LimitMiddlewareRequest
    {
        private readonly RequestDelegate siguiente;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LimitMiddlewareRequest> _logger;
        private readonly JwtSettings jwtSettings;

        public LimitMiddlewareRequest(RequestDelegate siguiente, IConfiguration configuration, ILogger<LimitMiddlewareRequest> logger, JwtSettings jwtSettings)
        {
            this.siguiente = siguiente;
            _configuration = configuration;
            _logger = logger;
            this.jwtSettings = jwtSettings;
        }
        public async Task InvokeAsync(HttpContext httpContext, _4toExpoDbContext _dbContext)
        {

            try
            {
                #region TokenValidation          
                //Token needs to be save in the request authorization HEADERS **** ==>>>
                var token = httpContext.Request.Headers["Authorization"].ToString();
                if (!String.IsNullOrEmpty(token))
                {
                    await siguiente(httpContext);
                    return;
                }
                #endregion
            }
            catch (ApplicationException ex)
            {
                //https://docs.microsoft.com/es-es/dotnet/api/system.applicationexception?view=net-6.0
                //este error ya se grabo en el Log desde el Filtro general de excepciones
                if (ex.Message != "Error no controlado en la API, ya se capturo en el archivo LOG.")
                    this._logger.LogError($"Error en el middleware de validacion de la APIKey, ApplicationException: {ex.Message}");

                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Ocurrió un error interno en la API." + ex.Message);
                return;
            }
            catch (InvalidOperationException ex)
            {
                //throw new InvalidOperationException(ex.Message);
                this._logger.LogError($"Error en el middleware de validacion de la APIKey, InvalidOperationException: {ex.Message}");
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync("Ocurrió un error interno en la API." + ex.Message);
                return;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error en el middleware de validacion de la APIKey: {ex.Message}");

                httpContext.Response.StatusCode = 400;
                await httpContext.Response.StartAsync();
                await httpContext.Response.WriteAsync(ex.Message);
                return;
            }
            await this.siguiente(httpContext);
        }
    }
}
