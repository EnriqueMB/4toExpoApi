﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class PaymentRequest
    {
        public string Producto { get; set; }
        public int Monto { get; set; }
        public int Cantidad { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string CardToken { get; set; } // Token de la tarjeta
    }
}