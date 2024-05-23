using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Request
{
    public class DatosEmailRequest
    {
        public class Emails
        {
            public EmailContacto? EmailContacto { get; set; }
            public EmailAlquiler? EmailAlquiler { get; set; }
            public EmailProductos? EmailProductos { get; set; }
            public EmailBolsaDeTrabajo? EmailBolsaDeTrabajo { get; set; }
        }

        public class EmailContacto
        {
            public string Nombre { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            public string Mensaje { get; set; }
        }

        public class EmailAlquiler
        {
            public string Nombre { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            public int Tipo { get; set; }
            public string Empresa { get; set; }
            public string Mensaje { get; set; }
        }

        public class EmailProductos
        {
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [DataType(DataType.PhoneNumber)]
            public int Telefono { get; set; }
            public string Estado { get; set; }
            public string Municipio { get; set; }
            public int CodigoPostal { get; set; }
            public int TotalArticulos { get; set; }
            public string Direccion { get; set; }
            public string DescripcionDireccion { get; set; }
        }

        public class EmailBolsaDeTrabajo
        {

        }
    }
}
