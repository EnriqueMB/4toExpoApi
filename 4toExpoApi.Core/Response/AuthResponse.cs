using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Response
{
    public class AuthResponse
    {
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string Token { get; set; }
        public List<long> Permisos { get; set; }
        public List<int> Roles { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
