using MailKit.Security;
using MimeKit.Utils;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using _4toExpoApi.Core.Request;
using Newtonsoft.Json;
using _4toExpoApi.DataAccess.Entities;
using _4toExpoApi.Core.Helpers;
using _4toExpoApi.Core.ViewModels;

namespace KiddyCheckApi.Core.Helpers
{
    public static class MailHelperQr
    {
        public static void EnviarEmail(string host, string port, string user, string password, UsuarioPromoRequest datos, string nombrePlantilla)
        {
            // Obtener la plantilla HTML del archivo
            string htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Recursos", nombrePlantilla);
            string htmlContent = string.Empty;

            using (StreamReader reader = new StreamReader(htmlFilePath))
            {
                htmlContent = reader.ReadToEnd();
            }

            var fecha = DateTime.Now;
            var fechaFormateada = $"{fecha.ToString("dddd")} {fecha.Day} {fecha.ToString("MMMM")} del {fecha.Year}";

            // Reemplazar los valores de la plantilla HTML
            htmlContent = htmlContent.Replace("{{ $motivo }}", "Asistencia");

            htmlContent = htmlContent.Replace("{{ $Participante }}", datos.nombreCompleto);
            htmlContent = htmlContent.Replace("{{ $fecha }}", fechaFormateada);


            // Generar el código QR en formato base64
            string qRCodeBase64 = generateqr(datos);

            // Crear el cuerpo del correo electrónico
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlContent;

            // Crear el mensaje de correo electrónico
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(user));
            email.To.Add(MailboxAddress.Parse(datos.correo));
            email.Subject = "Asistencia al evento";
            email.Body = builder.ToMessageBody();

            // Crear el MemoryStream para el archivo adjunto
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(qRCodeBase64)))
            {
                var attachment = new MimePart("image", "png")
                {
                    Content = new MimeContent(memoryStream),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = "codigo_qr.png"
                };

                // Adjuntar el archivo al cuerpo del correo electrónico
                email.Body = new Multipart("mixed") { builder.ToMessageBody(), attachment };

                // Configurar el cliente SMTP y enviar el correo electrónico
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    smtp.Connect(host, int.Parse(port), SecureSocketOptions.StartTls);
                    smtp.Authenticate(user, password);
                    smtp.Send(email);
                    smtp.Disconnect(true);
                }
            }

            
        }

        public static string generateqr(UsuarioPromoRequest datos)
        {
            string jsonString = JsonConvert.SerializeObject(datos);
            string qRCodeHelper = QRCodeHelper.GenerateQRCode(jsonString);
            return qRCodeHelper;
        }
    }
}
