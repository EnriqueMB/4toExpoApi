using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Response
{
    public class GenericResponse<T> where T : class
    {
        public T Data { get; set; }
        public string CreatedId { get; set; }
        public string UpdatedId { get; set; }
        public string DeletedId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
