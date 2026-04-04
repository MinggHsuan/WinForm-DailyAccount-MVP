using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping.Utility
{
    internal static class Extension
    {
        private static System.Threading.Timer timer;
        private static Form mainForm;
        private static object action;
        private static object number;
        public static void DebounceTime<T>(this Form form, Action<T> callback, T t, int delay)
        {
            number = t;
            action = callback;
            mainForm = form;
            TimerCallback doSomething = new TimerCallback(DoSomething);
            if (timer == null)
            {
                timer = new System.Threading.Timer(doSomething, t, delay, -1);
            }
            timer.Change(delay, -1);
        }
        private static void DoSomething(object t)
        {
            mainForm.Invoke(new Action(() =>
            {
                action.GetType().GetMethod("Invoke").Invoke(action, new object[] { number });
            }));
        }
        public static string Compression(this Image image, string filename)
        {
            using (Bitmap map = new Bitmap(image))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 15L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                string[] newFileName = filename.Split(new string[] { "small_" }, 0);
                map.Save(newFileName[0] + newFileName[1], jpgEncoder, myEncoderParameters);

                Bitmap resizedImage = new Bitmap(40, 40);
                using (Graphics graphics = Graphics.FromImage(resizedImage))
                {
                    graphics.DrawImage(map, 0, 0, 40, 40);
                    resizedImage.Save(filename, ImageFormat.Jpeg);
                    return filename;
                }
            }

        }
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
