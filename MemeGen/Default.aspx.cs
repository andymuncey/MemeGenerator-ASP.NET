using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MemeGen
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SubmitButton_Click(object sender, EventArgs e)
        {   //file to Image object
            Byte[] file = ImageUploader.FileBytes;
            MemoryStream str = new MemoryStream(file);
            System.Drawing.Image image = System.Drawing.Image.FromStream(str);

            //Image to Meme
            MemeGenerator memeGen = new MemeGenerator(image);
            System.Drawing.Image memeImage = memeGen.Generate(txtTopLine.Text,txtBottomLine.Text);
            String guid = Guid.NewGuid().ToString();

            //set JPEG encoding quality
            ImageCodecInfo encoder = ImageCodecInfo.GetImageEncoders().First(c => c.FormatID == ImageFormat.Jpeg.Guid);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(Encoder.Quality, 100L);

            //Meme to Image file
            memeImage.Save(Server.MapPath("~/Memes/"+guid+".jpg"),encoder,encoderParams);

            //Set Image contriol to show file
            MemeImage.ImageUrl = "memes/"+guid+".jpg";
            MemeImage.AlternateText = txtTopLine.Text + ": " + txtBottomLine.Text;
            MemeImage.Attributes.Add("width", memeImage.Width.ToString());
            MemeImage.Attributes.Add("height", memeImage.Height.ToString());
        }

    }
}