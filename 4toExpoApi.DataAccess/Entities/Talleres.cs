﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Talleres
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan Hora { get; set; }
        public int UserAlt { get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool Activo { get; set; }
    }
}