﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
public class ProgramaActividades
    {

        [Key]

        public int IdProgramaActividades { get; set; }
        public string? Orden { get; set; }
        public string? Nombre { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? HoraInicio { get; set; }
        public TimeSpan? HoraFinal { get; set; }
        public string? Detalles { get; set; }

        public DateTime? FechaAlt { get; set; }
        public int? UserAlt { get; set; }
        public DateTime? FechaUpd { get; set; }
        public int? UserUpd { get; set; }
        public bool? Activo { get; set; }
    }
}