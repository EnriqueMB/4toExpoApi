using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class IncluyePaqueteRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int PaqueteId { get; set; }
    }
}
