using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping
{
    public partial class ImageForm : Form
    {
        public ImageForm(string image)
        {
            InitializeComponent();
            this.FormClosed += ImageForm_FormClosed;
            string[] newFileName = image.Split(new string[] { "small_" }, 0);
            pictureBox1.Image = Image.FromFile(newFileName[0] + newFileName[1]);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            pictureBox1.Image?.Dispose();
        }
    }
}
