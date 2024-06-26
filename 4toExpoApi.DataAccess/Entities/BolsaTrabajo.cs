﻿using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class BolsaTrabajo
    {
        public int IdBolsaTrabajo { get; set; }
        public string Puesto { get; set; }
        public string Tipo { get; set; }
        public string Requisitos { get; set; }
        public string Descripcion { get; set; }
        public DateTime DiasLaborales { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinal { get; set; }
        public string Ciudad { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaAlt { get; set; }
        public string UserAlt { get; set; }
        public DateTime FechaUpd { get; set; }
        public string UserUpd { get; set; }
        public bool Activo { get; set; }

    }
}
