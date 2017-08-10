using System;
using System.Drawing;
using System.Drawing.Drawing2D;


namespace MemeGen
{
    public class MemeGenerator
    {
        private Image sourceImage;

        public MemeGenerator(Image image)
        {
            this.sourceImage = image;
        }

        public Image Generate(string topLine, string bottomLine)
        {
            Image memeImage = sourceImage.Clone() as Image;
            Graphics textGraphics = Graphics.FromImage(memeImage);

            textGraphics.CompositingQuality = CompositingQuality.HighQuality;
            textGraphics.SmoothingMode = SmoothingMode.HighQuality;
            textGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            float fontSize = 10.0f; //default starting font size - will be adjusted to fit image
            Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);

            float textWidthAtTenEm = Math.Max(estimateWidthOfString(topLine, font), estimateWidthOfString(bottomLine, font));

            //adjust font size for actual image size
            float scale = memeImage.Width / textWidthAtTenEm;
            fontSize *= scale;

            font = new Font(font.FontFamily, fontSize, font.Style); //Font object provides no mutator for size, so recreate

            Pen thickPen = new Pen(Brushes.Black, 3.0f);

            GraphicsPath topPath = textPathWithVerticalOffSet(topLine, font);
            textGraphics.DrawPath(thickPen, topPath);
            textGraphics.FillPath(Brushes.White, topPath);

            float bottomOffset = sourceImage.Height - (fontSize * 1.2f);
            GraphicsPath bottomPath = textPathWithVerticalOffSet(bottomLine, font, bottomOffset);
            textGraphics.DrawPath(thickPen, bottomPath);
            textGraphics.FillPath(Brushes.White, bottomPath);

            textGraphics.Dispose();
            return memeImage;
        }


        private GraphicsPath textPathWithVerticalOffSet(string text, Font font, float topOffset = 0)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
            float leftOffset = (sourceImage.Width - path.GetBounds().Width) / 2;
            Matrix translateMatrix = new Matrix();
            translateMatrix.Translate(leftOffset, topOffset);
            path.Transform(translateMatrix);
            return path;
        }


        /// <summary>
        /// Estimates the size of a string given a font
        /// Adapted from Eystein Bye (2012): https://stackoverflow.com/questions/5553965/how-to-programmatically-measure-string-pixel-width-in-asp-net
        /// </summary>
        /// <param name="text">The String to estimate the size of</param>
        /// <param name="font">The Font in which the text should be rendered</param>
        /// <returns></returns>
        private float estimateWidthOfString(string text, Font font)
        {
            Bitmap objBitmap = default(Bitmap);
            Graphics objGraphics = default(Graphics);

            objBitmap = new Bitmap(500, 200);
            objGraphics = Graphics.FromImage(objBitmap);

            SizeF stringSize = objGraphics.MeasureString(text, font);

            objBitmap.Dispose();
            objGraphics.Dispose();
            return stringSize.Width;
        }

    }
}