using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Response
{
    public class GenericResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string CreatedId { get; set; }
        public string UpdatedId { get; set; }
        public string DeletedId { get; set; }
    }
}
