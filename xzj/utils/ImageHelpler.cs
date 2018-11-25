using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xzj.utils
{
    class ImageHelpler
    {
        // 将image转换成byte[]数据
        public static byte[] imageToByte(System.Drawing.Image _image)
        {
            MemoryStream ms = new MemoryStream();
            _image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            return ms.ToArray();
        }
        // 将byte[]数据转换成image
        public static Image byteToImage(byte[] myByte)
        {
            MemoryStream ms = new MemoryStream(myByte);
            Image _Image = Image.FromStream(ms);
            return _Image;
        }
        // 截取Bitmap中的部分
        public static Bitmap cutImage(Bitmap b, int x, int y, int width, int height) {
            return b.Clone(new System.Drawing.Rectangle(x, y, width, height), b.PixelFormat);
        }
    }
}
