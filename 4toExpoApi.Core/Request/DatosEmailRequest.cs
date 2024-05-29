using _4toExpoApi.Core.ViewModels;
using Microsoft.AspNetCore.Http;
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
            //public int Tipo { get; set; }
            public string? Empresa { get; set; }
            public string Mensaje { get; set; }
            public ServicioAlquiler? Servicio { get; set; }
        }

        public class EmailProductos
        {
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            [DataType(DataType.PhoneNumber)]
            public string Telefono { get; set; }
            public string Estado { get; set; }
            public string Municipio { get; set; }
            public string CodigoPostal { get; set; }
            public int TotalArticulos { get; set; }
            public string Direccion { get; set; }
            public string DescripcionDireccion { get; set; }
        }

        public class EmailBolsaDeTrabajo
        {
            public string Nombre { get; set; }
            public int Edad { get; set; }
            [DataType(DataType.PhoneNumber)]
            public string Telefono { get; set; }
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }
            public IFormFile Cv { get; set; }
            public string Mensaje { get; set; }
            public BolsaTrabajoVM Datos { get; set; }
        }

        public class ServicioAlquiler
        {
            public int? Id { get; set; }
            public string? Nombre { get; set; }
            public string? Descripcion { get; set; }
            public string? DiasAtencion { get; set; }
            public string? Horarios { get; set; }
        }

        public class File
        {
            public string Name { get; set; }
            public long? LastModified { get; set; }
            public DateTime? LastModifiedDate { get; set; }
            public string? WebkitRelativePath { get; set; }
            public long? Size { get; set; }
            public string Type { get; set; }
        }
    }
}
