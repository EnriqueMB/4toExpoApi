
using QRCoder;
using System.Drawing;

namespace _4toExpoApi.Core.Helpers
{
    public static class QRCodeHelper
    {
        public static string GenerateQRCode(string socio)
        {
            string jsonString = socio;

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(jsonString, QRCodeGenerator.ECCLevel.Q);

            BitmapByteQRCode qrCodeBitmap = new BitmapByteQRCode(qrCodeData);

            byte[] qrCodeImage = qrCodeBitmap.GetGraphic(5);

            string base64String = Convert.ToBase64String(qrCodeImage);
            return base64String;
        }

    }
}
