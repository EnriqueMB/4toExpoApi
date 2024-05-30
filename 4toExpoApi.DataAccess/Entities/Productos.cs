using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.DataAccess.Entities
{
    public class Productos
    {
        public int Id { get; set; }
        public string Nombre { get; set; } 
        public string Marca {  get; set; }
        public string Caracteristica { get; set; }
        public int Precio { get; set; }
        public string Descripcion {  get; set; }
        public int TotalArticulo { get; set; }
        public int IdPatrocinador {  get; set; }
        public int? UserAlt {  get; set; }
        public DateTime? FechaAlt { get; set; }
        public int? UserUpd { get; set; }
        public DateTime? FechaUpd { get; set; }
        public bool Activo {  get; set; }
    }
}
