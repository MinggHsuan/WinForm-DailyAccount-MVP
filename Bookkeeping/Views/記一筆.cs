using Bookkeeping.Components;
using Bookkeeping.Models;
using Bookkeeping.Utility;
using CSVlibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping.Views
{
    public partial class 記一筆 : Form
    {
        public 記一筆()
        {
            InitializeComponent();
            comboBox1.DataSource = DataModel.Type;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;
            comboBox2.DataSource = DataModel.Detail[DataModel.Type[0]];
            comboBox3.DataSource = DataModel.Target;
            comboBox4.DataSource = DataModel.PaymentMethods;

            pictureBox1.Image = Image.FromFile(@"C:\Users\user\Desktop\CsharpClass\程式圖片\上傳圖片.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Image.FromFile(@"C:\Users\user\Desktop\CsharpClass\程式圖片\上傳圖片.jpg");
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox str = (ComboBox)sender;
            comboBox2.DataSource = DataModel.Detail[str.Text];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RecordModel model = new RecordModel(dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox1.Text,
                    comboBox1.Text, comboBox2.Text, comboBox3.Text, comboBox4.Text, "image1", "image2");

            this.DebounceTime((x) =>
            {
                if (!Directory.Exists($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{dateTimePicker1.Value.ToString("yyyy-MM-dd")}"))
                {
                    Directory.CreateDirectory($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{dateTimePicker1.Value.ToString("yyyy-MM-dd")}");
                }
                model.Image1 = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{dateTimePicker1.Value.ToString("yyyy-MM-dd")}\small_{Guid.NewGuid()}.jpg";
                pictureBox1.Image.Compression(model.Image1);
                pictureBox1.Image.Dispose();
                model.Image2 = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{dateTimePicker1.Value.ToString("yyyy-MM-dd")}\small_{Guid.NewGuid()}.jpg";
                pictureBox2.Image.Compression(model.Image2);
                pictureBox2.Image.Dispose();
                CSVHelper.Write<RecordModel>($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{dateTimePicker1.Value.ToString("yyyy-MM-dd")}\Data.csv", x);

                pictureBox1.Image = Image.FromFile(@"C:\Users\user\Desktop\CsharpClass\程式圖片\上傳圖片.jpg");
                pictureBox2.Image = Image.FromFile(@"C:\Users\user\Desktop\CsharpClass\程式圖片\上傳圖片.jpg");
            }, model, 500);
        }
        private void Image_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "圖片檔|*.png;*.jpg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PictureBox pictureBox = (PictureBox)sender;
                pictureBox.Image.Dispose();
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);
            }
        }
    }
}
