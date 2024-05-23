using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class PagoRequest
    {
        public string? id { get; set; }
        public string? TitularTarjeta { get; set; }
        public string? EmailTajeta { get; set; }
        public int? amount { get; set; }
        public string? payment_status { get; set; }
        public string? RefBancaria { get; set; }
        public string? Pasarela { get; set; }
        public CustomerInfo customer_info { get; set; }


    }


    public class CustomerInfo
    {
        public string email { get; set; }
        public string  phone { get; set; }
        public string name { get; set; }

    }
}
