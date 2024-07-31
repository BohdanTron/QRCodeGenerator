using Microsoft.AspNetCore.Mvc;
using QRCoder;

namespace QRCodeGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QRCodeController : ControllerBase
    {
        [HttpGet("html")]
        public ContentResult GenerateInHtml([FromQuery] string text)
        {
            var base64 = GenerateQrCodeInBase64(text);

            var htmlTemplate =
                $"""
                     <!DOCTYPE html>
                     <html lang='en'>
                     <head>
                         <meta charset='UTF-8'>
                         <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                         <title>QR Code</title>
                     </head>
                     <body>
                         <h1>QR Code</h1>
                         <img src='data:image/png;base64,{base64}' alt='QR Code'>
                     </body>
                     </html>
                 """;

            return new ContentResult
            {
                Content = htmlTemplate,
                ContentType = "text/html",
                StatusCode = 200
            };
        }

        private static string GenerateQrCodeInBase64(string text)
        {
            var qrGenerator = new QRCoder.QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(text, QRCoder.QRCodeGenerator.ECCLevel.Q);

            var qrCode = new Base64QRCode(qrCodeData);
            var qrCodeImageAsBase64 = qrCode.GetGraphic(20);

            return qrCodeImageAsBase64;
        }
    }
}
