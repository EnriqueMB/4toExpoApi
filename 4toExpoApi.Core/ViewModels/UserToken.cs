using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.ViewModels
{
    public class UserToken
    {
        public string Token { get; set; }
        public string UserName { get; set; }
        public int Id { get; set; }
        public int PerfilId { get; set; }
        public List<long> Permisos { get; set; }
        public List<int> Roles { get; set; }
    }
}
