using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;

namespace MemeGen
{
    public class MemeGenerator
    {
        private Image sourceImage;

        public MemeGenerator(Image image)
        {
            this.sourceImage = ProcessOrientation(image);
        }

        private Image ProcessOrientation(Image image)
        {
            const int ExifRotationId = 0x0112;

            if (image.PropertyIdList.Contains(ExifRotationId))
            {
                PropertyItem rotationPropertyItem = image.GetPropertyItem(ExifRotationId);
                UInt16 rotationValue = rotationPropertyItem.Value[0];
                switch (rotationValue)
                {
                    case 1: // normal
                        break;
                    case 2: //mirror horizontal
                        image.RotateFlip(rotateFlipType: RotateFlipType.RotateNoneFlipX);
                        break;
                    case 3: //rotate 180
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                        break;
                    case 4: //mirror vertical
                        image.RotateFlip(rotateFlipType: RotateFlipType.RotateNoneFlipY);
                        break;                        
                    case 5: // mirror horizontal and rotate 270 CW
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipX);
                        break;
                    case 6: // rotated 90 CW
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                        break;
                    case 7: //mirror horizontal and rotate 90 CW
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipX);
                        break;                    
                    case 8: // rotate 270 CW
                        image.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                        break;
                }

                rotationPropertyItem.Value[0] = 1;
                image.SetPropertyItem(rotationPropertyItem);
            }
            return image;
        }

        public Image Generate(string topLine, string bottomLine)
        {
            Image memeImage = sourceImage.Clone() as Image;
            Graphics textGraphics = Graphics.FromImage(memeImage);

            textGraphics.CompositingQuality = CompositingQuality.HighQuality;
            textGraphics.SmoothingMode = SmoothingMode.HighQuality;
            textGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            float fontSize = 20.0f; //default starting font size - will be adjusted to fit image
            Font font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);

            RectangleF topSize = estimateSizeOfString(topLine, font);
            RectangleF bottomSize = estimateSizeOfString(bottomLine, font);
            float textWidthAtTenEm = Math.Max(topSize.Width, bottomSize.Width);

            //adjust font size for actual image size
            float scale = memeImage.Width / textWidthAtTenEm;
            fontSize *= scale * 0.98f;

            font = new Font(font.FontFamily, fontSize, font.Style); //Font object provides no mutator for size, so recreate

            Pen thickPen = new Pen(Brushes.Black, 3.0f);

            GraphicsPath topPath = textPathWithVerticalOffSet(topLine, font);
            textGraphics.DrawPath(thickPen, topPath);
            textGraphics.FillPath(Brushes.White, topPath);

            float bottomOffset = sourceImage.Height - (fontSize * 1.2f);
            GraphicsPath bottomPath = textPathWithVerticalOffSet(bottomLine, font, (int)bottomOffset);
            textGraphics.DrawPath(thickPen, bottomPath);
            textGraphics.FillPath(Brushes.White, bottomPath);

            textGraphics.Dispose();
            return memeImage;
        }


        private GraphicsPath textPathWithVerticalOffSet(string text, Font font, int topOffset = 0)
        {
            StringFormat centreFormat = new StringFormat();
            centreFormat.Alignment = StringAlignment.Center;
            Point renderPoint = new Point(sourceImage.Width / 2, topOffset);
            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, renderPoint, centreFormat);
            return path;
        }


        /// <summary>
        /// Estimates the size of a string given a font
        /// </summary>
        /// <param name="text">The string to estimate the size of</param>
        /// <param name="font">The Font in which the text should be rendered</param>
        /// <returns>A RectangleF with the size of the String as would be rendered</returns>
        private RectangleF estimateSizeOfString(string text, Font font)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddString(text, font.FontFamily, (int)font.Style, font.Size, new Point(0, 0), new StringFormat());
            return path.GetBounds();
        }

    }
}