using _4toExpoApi.Core.Request;
using _4toExpoApi.DataAccess.Entities;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4toExpoApi.Core.Helpers
{
    public static class MailHelper
    {
        public static void EnviarEmail(string host, string port, string user, string password, DatosEmailRequest.Emails datos, string nombrePlantilla, string correoEnviar)
        {
            //Obtener plantilla de html de la carpeta de recursos
            string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Recursos", nombrePlantilla);

            string htmlContent = string.Empty;

            using (StreamReader reader = new StreamReader(htmlFilePath))
            {
                htmlContent = reader.ReadToEnd();
            }

            var fecha = DateTime.Now;
            var fechaFormateada = (fecha.ToString("dddd") + " " + fecha.Day + " " + fecha.ToString("MMMM") + " del " + fecha.Year);

            //Reemplazar los valores de la plantilla
            //htmlContent = htmlContent.Replace("{{ $motivo }}", "Asistencia del alumno");

            //htmlContent = htmlContent.Replace("{{ $tutor }}", datos.NombreTutor);
            //htmlContent = htmlContent.Replace("{{ $alumno }}", datos.NombreAlumno);
            //htmlContent = htmlContent.Replace("{{ $fecha }}", fechaFormateada);
            //htmlContent = htmlContent.Replace("{{ $maestro }}", datos.NombreMaestro);

            var builder = new BodyBuilder();


            //Enviar correo
            var email = new MimeMessage();

            if (datos.EmailContacto != null)
            {
                htmlContent = htmlContent.Replace("{{ $motivo }}", "Contactanos eventos");
                htmlContent = htmlContent.Replace("{{ $nombre }}", datos.EmailContacto.Nombre);
                htmlContent = htmlContent.Replace("{{ $email }}", datos.EmailContacto.Email);
                htmlContent = htmlContent.Replace("{{ $mensaje }}", datos.EmailContacto.Mensaje);

                email.Subject = "Notificación de contacto";
            }

            if (datos.EmailAlquiler != null)
            {
                htmlContent = htmlContent.Replace("{{ $motivo }}", "Alquieler eventos");
                htmlContent = htmlContent.Replace("{{ $nombre }}", datos.EmailAlquiler.Nombre);
                htmlContent = htmlContent.Replace("{{ $email }}", datos.EmailAlquiler.Email);
                htmlContent = htmlContent.Replace("{{ $mensaje }}", datos.EmailAlquiler.Mensaje);

                if(datos.EmailAlquiler.Empresa != null)
                    htmlContent = htmlContent.Replace("{{ $empresa }}", datos.EmailAlquiler.Empresa);
                else
                    htmlContent = htmlContent.Replace("{{ $empresa }}", " ");

                if (datos.EmailAlquiler.Servicio != null)
                {
                    htmlContent = htmlContent.Replace("{{ $nombreServicio }}", datos.EmailAlquiler!.Servicio!.Nombre);
                    htmlContent = htmlContent.Replace("{{ $descripcion }}", datos.EmailAlquiler!.Servicio!.Descripcion);
                    htmlContent = htmlContent.Replace("{{ $diasAtencion }}", datos.EmailAlquiler!.Servicio!.DiasAtencion);
                    htmlContent = htmlContent.Replace("{{ $horarios }}", datos.EmailAlquiler!.Servicio!.Horarios);
                    //htmlContent = htmlContent.Replace("{{ $servicio }}",
                    //    datos.EmailAlquiler!.Servicio!.Nombre
                    //    + "<p>" + datos.EmailAlquiler!.Servicio!.Descripcion + "</p>"
                    //    + "<p>" + datos.EmailAlquiler!.Servicio!.DiasAtencion + "</p>"
                    //    + "<p>" + datos.EmailAlquiler!.Servicio!.Horarios + "</p>");
                }

                email.Subject = "Notificación de alquiler";
            }

            if (datos.EmailProductos != null)
            {
                htmlContent = htmlContent.Replace("{{ $motivo }}", "Productos eventos");
                htmlContent = htmlContent.Replace("{{ $nombre }}", datos.EmailProductos.Nombres);
                htmlContent = htmlContent.Replace("{{ $apellido }}", datos.EmailProductos.Apellidos);
                htmlContent = htmlContent.Replace("{{ $email }}", datos.EmailProductos.Email);
                htmlContent = htmlContent.Replace("{{ $telefono }}", datos.EmailProductos.Telefono);
                htmlContent = htmlContent.Replace("{{ $estado }}", datos.EmailProductos.Estado);
                htmlContent = htmlContent.Replace("{{ $municipio }}", datos.EmailProductos.Municipio);
                htmlContent = htmlContent.Replace("{{ $codigoPostal }}", datos.EmailProductos.CodigoPostal);
                htmlContent = htmlContent.Replace("{{ $totalArticulos }}", datos.EmailProductos.TotalArticulos.ToString());
                htmlContent = htmlContent.Replace("{{ $direccion }}", datos.EmailProductos.Direccion);
                htmlContent = htmlContent.Replace("{{ $descripcionDireccion }}", datos.EmailProductos.DescripcionDireccion);

                email.Subject = "Notificación de producto";
            }

            if (datos.EmailBolsaDeTrabajo != null)
            {
                if (datos.EmailBolsaDeTrabajo.Cv != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        datos.EmailBolsaDeTrabajo.Cv.CopyTo(ms);
                        var cvAttachment = ms.ToArray();
                        builder.Attachments.Add(datos.EmailBolsaDeTrabajo.Cv.FileName, cvAttachment, ContentType.Parse(datos.EmailBolsaDeTrabajo.Cv.ContentType));
                    }
                }

                htmlContent = htmlContent.Replace("{{ $motivo }}", "Bolsa de trabajo eventos");

                htmlContent = htmlContent.Replace("{{ $nombre }}", datos.EmailBolsaDeTrabajo.Nombre);
                htmlContent = htmlContent.Replace("{{ $edad }}", datos.EmailBolsaDeTrabajo.Edad.ToString());
                htmlContent = htmlContent.Replace("{{ $telefono }}", datos.EmailBolsaDeTrabajo.Telefono);
                htmlContent = htmlContent.Replace("{{ $email }}", datos.EmailBolsaDeTrabajo.Email);
                htmlContent = htmlContent.Replace("{{ $mensaje }}", datos.EmailBolsaDeTrabajo.Mensaje);
                htmlContent = htmlContent.Replace("{{ $tipo }}", datos.EmailBolsaDeTrabajo.Datos.Tipo);
                htmlContent = htmlContent.Replace("{{ $puesto }}", datos.EmailBolsaDeTrabajo.Datos.Puesto);
                htmlContent = htmlContent.Replace("{{ $descripcion }}", datos.EmailBolsaDeTrabajo.Datos.Descripcion);
                htmlContent = htmlContent.Replace("{{ $ciudad }}", datos.EmailBolsaDeTrabajo.Datos.Ciudad);
                htmlContent = htmlContent.Replace("{{ $direccion }}", datos.EmailBolsaDeTrabajo.Datos.Direccion);
                htmlContent = htmlContent.Replace("{{ $diaslaborales }}", datos.EmailBolsaDeTrabajo.Datos.DiasLaborales);
                htmlContent = htmlContent.Replace("{{ $horario }}", datos.EmailBolsaDeTrabajo.Datos.HoraInicio + " - " + datos.EmailBolsaDeTrabajo.Datos.HoraFinal);
                htmlContent = htmlContent.Replace("{{ $requisitos }}", datos.EmailBolsaDeTrabajo.Datos.Requisitos);

                email.Subject = "Notificación bolsa de trabajo";

            }
            builder.HtmlBody = htmlContent;


            email.From.Add(MailboxAddress.Parse(user));
            email.To.Add(MailboxAddress.Parse(correoEnviar));


            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(host, int.Parse(port), SecureSocketOptions.StartTls);

            smtp.Authenticate(user, password);

            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public static void EnviarEmailPago(string host, string port, string user, string password, Pagos pago, string nombrePlantilla)
        {
            //Obtener plantilla de html de la carpeta de recursos
            string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Recursos", nombrePlantilla);

            string htmlContent = string.Empty;

            using (StreamReader reader = new StreamReader(htmlFilePath))
            {
                htmlContent = reader.ReadToEnd();
            }

            var fecha = DateTime.Now;
            var fechaFormateada = (fecha.ToString("dddd") + " " + fecha.Day + " " + fecha.ToString("MMMM") + " del " + fecha.Year);

            //Reemplazar los valores de la plantilla
            htmlContent = htmlContent.Replace("{{ $motivo }}", "Baucher de pago");

            htmlContent = htmlContent.Replace("{{ $NombreCliente }}", pago.TitularTarjeta);
            htmlContent = htmlContent.Replace("{{ $ReferenciaPago }}", pago.IdTransaccion);
            htmlContent = htmlContent.Replace("{{ $Monto }}", pago.Monto.ToString());

            var builder = new BodyBuilder();

            builder.HtmlBody = htmlContent;

            //Enviar correo
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(user));
            email.To.Add(MailboxAddress.Parse(pago.EmailTarjeta));
            email.Subject = "Baucher de pago";

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(host, int.Parse(port), SecureSocketOptions.StartTls);

            smtp.Authenticate(user, password);

            smtp.Send(email);
            smtp.Disconnect(true);
        }


    }
}
