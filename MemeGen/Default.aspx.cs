using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
        {
            Byte[] file = ImageUploader.FileBytes;
            MemoryStream str = new MemoryStream(file);
            System.Drawing.Image image = System.Drawing.Image.FromStream(str);

            MemeGenerator memeGen = new MemeGenerator(image);
            var memeImage = memeGen.Generate(txtTopLine.Text,txtBottomLine.Text);
            String guid = Guid.NewGuid().ToString();
            memeImage.Save(Server.MapPath("~/Memes/"+guid+".jpg"),System.Drawing.Imaging.ImageFormat.Jpeg);

            MemeImage.ImageUrl = "memes/"+guid+".jpg";
            MemeImage.AlternateText = txtTopLine.Text + ": " + txtBottomLine.Text;
            MemeImage.Attributes.Add("width", memeImage.Width.ToString());
            MemeImage.Attributes.Add("height", memeImage.Height.ToString());
        }

       

      

        

    }
}