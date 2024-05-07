using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    internal class InformacionConfigRequest
    {
        public int id { get; set; }
        public string Titulo { get; set; }
        public string Subtitulo { get; set; }
        public string Multimedia { get; set; }
        public string Texto { get; set; }
    }
}
