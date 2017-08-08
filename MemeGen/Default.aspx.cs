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
            memeImage.Save(Server.MapPath("~/file.jpg"),System.Drawing.Imaging.ImageFormat.Jpeg);

            MemeImage.ImageUrl = "file.jpg";
        }

       

      

        

    }
}