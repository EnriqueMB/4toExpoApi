using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbogadosApiV1.Core.Request
{
    public class PaginadoRequest
    {
        public string Buscar { get; set; }
        public int Page { get; set; }
        public int Take { get; set; }
        public int Skip
        {
            get
            {
                return (Page - 1) * Take;
            }
        }
    }
}
