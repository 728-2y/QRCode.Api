using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using QRCode.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using static QRCoder.PayloadGenerator;

namespace QRCode.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IQRCodeService _qrCode;
        private readonly IHostingEnvironment _hostingEnvironment;
        public ValuesController(IQRCodeService qrCode, IHostingEnvironment hostingEnvironment)
        {
            _qrCode = qrCode;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("qrCode")]
        public IActionResult Get(string plainText, int pixel)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return BadRequest("parameter is null");
            }
            if (pixel <= 0)
            {
                return BadRequest("pixel <= 0");
            }

            var bitmap = _qrCode.GetQRCode(plainText, pixel);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);

            return File(ms.GetBuffer(), "image/jpeg");
        }

        [HttpGet("logo")]
        public IActionResult GetQRCodeWithLogo(string plainText, int pixel)
        {

            if (string.IsNullOrEmpty(plainText))
            {
                return BadRequest("parameter is null");
            }
            if (pixel <= 0)
            {
                return BadRequest("pixel <= 0");
            }

            var logoPath = @"E:\EFCore\QRCode.Api\QRCode.Api\0000_2.jpg";
            var bitmap = _qrCode.GetQRCodeWithLogo(plainText, pixel, logoPath);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);

            return File(ms.GetBuffer(), "image/jpeg");
        }

        [HttpGet("wifi")]
        public IActionResult GetWIFIQRCode(int pixel)
        {
            if (pixel <= 0)
            {
                return BadRequest("pixel <= 0");
            }

            var payload = new WiFi("ssid","password",WiFi.Authentication.WPA);
            var bitmap = _qrCode.GetQRCode(payload.ToString(), pixel);
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Jpeg);

            return File(ms.GetBuffer(), "image/jpeg");
        }

        [HttpGet("svg")]
        public IActionResult GetSvgQRCode(string plainText, int pixel)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return BadRequest("parameter is null");
            }
            if (pixel <= 0)
            {
                return BadRequest("pixel <= 0");
            }

            var svgQrCode = _qrCode.GetSvgQRCode(plainText, pixel);

            var rootPath = _hostingEnvironment.ContentRootPath;
            var svgName = "svgQRCode.svg";
            System.IO.File.WriteAllText($@"{rootPath}\{svgName}", svgQrCode);

            var readByte = System.IO.File.ReadAllBytes($@"{rootPath}\{svgName}");

            return File(readByte, "image/svg", svgName);
        }

        [HttpGet("download")]
        public IActionResult DownloadQRCode(int pixel)
        {
            if (pixel <= 0)
            {
                return BadRequest("pixel <= 0");
            }

            var url = "there is qrcode";
            var bitmap = _qrCode.GetQRCode(url, pixel);
            var rootPath = _hostingEnvironment.ContentRootPath;
            var photoName = "photo.jpeg";
            bitmap.Save($@"{rootPath}\{photoName}", ImageFormat.Jpeg);

            var readStream = System.IO.File.ReadAllBytes($@"{rootPath}\{photoName}");

            return File(readStream, "image/jpeg", photoName);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
