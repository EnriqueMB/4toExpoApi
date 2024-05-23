using _4toExpoApi.Core.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Helpers
{
    public static class JwtHelper
    {
        #region <-- Fields -->
        public enum Duration { Seconds = 1, Minutes = 2, Hours = 3, Days = 4 };
        public enum Roles
        {
            Admin = 1, SuperAdmin = 4, User = 5
        };

        #endregion

        #region <-- Constructor -->

        #endregion

        #region <-- Methods -->
        public static List<Claim> GetClaims(this UserToken userAccounts, Guid Id, IConfiguration configuration)
        {
            //var rol = TranslateRoles(configuration, userAccounts.PerfilId);

            List<Claim> claims = new List<Claim> {
                new Claim("Id", userAccounts.Id.ToString()),
                new Claim("PerfilId", userAccounts.PerfilId.ToString()),

            };

            foreach (var rol in userAccounts.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol.ToString()));
            }
            //foreach (var permiso in userAccounts.Permisos)
            //{
             //   claims.Add(new Claim("Permisos", permiso.ToString()));
            //}

            return claims;
        }
        public static UserToken GenTokenkey(UserToken model, JwtSettings jwtSettings, IConfiguration configuration)
        {
            try
            {
                var UserToken = new UserToken();

                if (model == null) throw new ArgumentException(nameof(model));

                // Get secret key
                var key = System.Text.Encoding.ASCII.GetBytes(jwtSettings.IssuerSigningKey);
                Guid Id = Guid.Empty;

                //DateTime expireTime = DateTime.UtcNow.AddDays(1);
                DateTime expireTime = DeterminarDuracion(configuration);

                var JWToken = new JwtSecurityToken
                    (
                        issuer: jwtSettings.ValidIssuer,
                        audience: jwtSettings.ValidAudience,
                        claims: GetClaims(model, Guid.NewGuid(), configuration),
                        notBefore: new DateTimeOffset(DateTime.Now).DateTime,
                        //expires: DateTime.Now.AddDays(2),
                        expires: expireTime,//DeterminarDuracion(configuration),
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha512Signature)
                    );

                UserToken.Token = new JwtSecurityTokenHandler().WriteToken(JWToken);
                UserToken.UserName = model.UserName;
                UserToken.Id = model.Id;
                UserToken.PerfilId = model.PerfilId;
                UserToken.Permisos = model.Permisos;
                UserToken.Roles = model.Roles;

                return UserToken;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static DateTime DeterminarDuracion(IConfiguration configuration)
        {
            Duration DurationType = (Duration)configuration.GetValue<int>("JsonWebTokenKeys:ConfiguracionExtra:TipoExpiracion");
            int DutarionTime = configuration.GetValue<int>("JsonWebTokenKeys:ConfiguracionExtra:TiempoExpiracion");
            DateTime duration;
            duration = DurationType switch
            {
                Duration.Seconds => DateTime.Now.AddSeconds(DutarionTime),
                Duration.Minutes => DateTime.Now.AddMinutes(DutarionTime),
                Duration.Hours => DateTime.Now.AddHours(DutarionTime),
                Duration.Days => DateTime.Now.AddDays(DutarionTime),
            };
            return duration;
        }

        #endregion
    }
}
