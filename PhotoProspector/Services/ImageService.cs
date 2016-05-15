using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace PhotoProspector.Services
{
    public interface IImageService
    {
        void CutImg(string oPath, string nPaht, int w, int h, string mode);
        void DrawImg(string path, string newpath, int[] intarray, string[] namearray, int facenum);
    }

    class ImageService : IImageService
    {
        public void CutImg(string oPath, string nPaht, int w, int h, string mode)
        {
            FileStream fs = new FileStream(oPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            Image oimg = Image.FromStream(fs);

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

            Image bitmap = new Bitmap(nToWidth, nToHeight);

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

        public void DrawImg(string path, string newpath, int[] intarray, string[] namearray, int facenum)
        {
            Image img = new Bitmap(path);
            Graphics g = Graphics.FromImage(img);

            int x = 0;
            int y = 0;
            int w = 0;
            int h = 0;

            if (namearray.Length != facenum)
            {
                namearray = new string[facenum];
            }

            for (int i = 0; i < facenum; i++)
            {
                Pen pen = new Pen(Color.DeepSkyBlue, 2);
                Brush b = Brushes.DeepSkyBlue;

                if (namearray[i] == "No Match" | namearray[i] == "No face Detected!")
                {
                    pen = new Pen(Color.Red, 2);
                    b = Brushes.Red;
                }

                x = intarray[4 * i];
                y = intarray[4 * i + 1];
                w = intarray[4 * i + 2];
                h = intarray[4 * i + 3];

                int fontrate = 4;
                int fontsize = (h * 72) / (fontrate * 96);

                if (x + y + w + h == 0)
                {
                    fontsize = 50;
                }
                g.DrawRectangle(pen, x, y, w, h);

                int FY = (y + h);

                g.DrawString(namearray[i], new Font("calibri", fontsize), b, new PointF(x, FY));
            }

            try
            {
                img.Save(newpath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                img.Dispose();
                g.Dispose();
            }
        }
    }
}
