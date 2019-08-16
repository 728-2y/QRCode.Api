using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace QRCode.Api.Services.Interfaces
{
    public interface IQRCodeService
    {
        Bitmap GetQRCode(string url, int pixel);

        Bitmap GetQRCodeWithLogo(string plainText, int pixel, string logoPath);

        string GetSvgQRCode(string plainText, int pixel);
    }
}
