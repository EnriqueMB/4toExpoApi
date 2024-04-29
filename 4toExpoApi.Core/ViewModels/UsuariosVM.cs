using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class UsuariosVM
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string NombreUsuario { get; set; }
        public int TipoUsuario { get; set; }
        //public string TipoUsuarioString
        //{
        //    get
        //    {
        //        return TipoUsuarioHelper.GetTipoUsuario(TipoUsuario);
        //    }
        //}
        public DateTime UltimoAcceso { get; set; }

        public string UltimoAccesoString
        {
            get
            {
                return UltimoAcceso.ToString("dd/MM/yyyy HH:mm:ss");
            }
        }
        public bool? Activo { get; set; }
    }
}
