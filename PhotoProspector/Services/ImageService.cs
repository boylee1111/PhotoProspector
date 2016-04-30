using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace PhotoProspector.Services
{
    public interface IImageService
    {
        void CutImg(string oPath, string nPaht, int w, int h, string mode);
    }

    class ImageService : IImageService
    {
        public void CutImg(string oPath, string nPaht, int w, int h, string mode)
        {

            FileStream fs = new FileStream(oPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            System.Drawing.Image oimg = System.Drawing.Image.FromStream(fs);

            //System.Drawing.Image oimg = System.Drawing.Image.FromFile(oPath);

            int nToWidth = w;
            int nToHeight = h;
            int x = 0;
            int y = 0;
            int oWidth = oimg.Width;
            int oHeight = oimg.Height;
            switch (mode)
            {
                case "HW":
                    if (oimg.Width > oimg.Height)
                    {
                        nToHeight = nToWidth * oHeight / oWidth;
                    }
                    else
                    {
                        nToWidth = nToHeight * oWidth / oHeight;
                    }
                    break;
                case "W":
                    nToHeight = oWidth * oHeight / nToWidth;
                    break;
                case "H":
                    nToWidth = oWidth * oHeight / nToHeight;
                    break;
                case "CUT":
                    if ((oimg.Width / oimg.Height) > (nToWidth / nToHeight))
                    {
                        oHeight = oimg.Height;
                        oWidth = oimg.Height * nToWidth / nToHeight;
                        y = 0;
                        x = (oimg.Width - oWidth) / 2;
                    }
                    else
                    {
                        oWidth = oimg.Width;
                        oHeight = oimg.Width * nToHeight / nToWidth;
                        x = 0;
                        y = (oimg.Height - oHeight) / 2;
                    }
                    break;
                default: break;
            }

            System.Drawing.Image bitmap = new Bitmap(nToWidth, nToHeight);

            Graphics gp = Graphics.FromImage(bitmap);
            gp.InterpolationMode = InterpolationMode.High;
            gp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            gp.Clear(Color.Transparent);
            gp.DrawImage(oimg, new Rectangle(0, 0, nToWidth, nToHeight), new Rectangle(x, y, oWidth, oHeight), GraphicsUnit.Pixel);

            try
            {
                bitmap.Save(nPaht, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                oimg.Dispose();
                bitmap.Dispose();
                fs.Close();

            }
        }
    }
}
