using _4toExpoApi.Core.Request;
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
        public static void EnviarEmail(string host, string port, string user, string password, DatosEmailRequest datos, string nombrePlantilla)
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
            htmlContent = htmlContent.Replace("{{ $motivo }}", "Asistencia del alumno");

            //htmlContent = htmlContent.Replace("{{ $tutor }}", datos.NombreTutor);
            //htmlContent = htmlContent.Replace("{{ $alumno }}", datos.NombreAlumno);
            htmlContent = htmlContent.Replace("{{ $fecha }}", fechaFormateada);
            //htmlContent = htmlContent.Replace("{{ $maestro }}", datos.NombreMaestro);

            var builder = new BodyBuilder();

            builder.HtmlBody = htmlContent;

            //Enviar correo
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(user));
            email.To.Add(MailboxAddress.Parse(datos.Correo));
            email.Subject = "Notificación de asistencia del alumno";

            email.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            smtp.Connect(host, int.Parse(port), SecureSocketOptions.StartTls);

            smtp.Authenticate(user, password);

            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
