using Bookkeeping.Components;
using Bookkeeping.Contract;
using Bookkeeping.Models;
using Bookkeeping.Models.DTOs;
using Bookkeeping.Presenter;
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
using static Bookkeeping.Contract.AddRecordContract;

namespace Bookkeeping.Views
{
    public partial class 記一筆 : Form, AddRecordContract.IView
    {
        private AddRecordContract.IPresenter presenter;
        private string fileName = @"C:\Users\user\Desktop\CsharpClass\程式圖片\上傳圖片.jpg";
        public 記一筆()
        {
            InitializeComponent();
            presenter = new AddRecordPresenter(this);
            presenter.GetDataSource();
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

            pictureBox1.Image = Image.FromFile(fileName);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Image = Image.FromFile(fileName);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }
        public void OnGetResult(DataModelDTO records)
        {
            comboBox1.DataSource = records.Type;
            comboBox2.DataSource = records.Detail;
            comboBox3.DataSource = records.Target;
            comboBox4.DataSource = records.PaymentMethods;
        }
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox str = (ComboBox)sender;
            presenter.GetDetail(str.Text);
        }
        public void OnDetailResult(List<string> Detail)
        {
            comboBox2.DataSource = Detail;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DebounceTime(() =>
            {
                presenter.AddRecord(new RecordDTO(dateTimePicker1.Value.ToString("yyyy-MM-dd"), textBox1.Text,
                    comboBox1.Text, comboBox2.Text, comboBox3.Text, comboBox4.Text, pictureBox1.Image, pictureBox2.Image));
            }, 500);
        }
        public void ReSetUI()
        {
            pictureBox1.Image.Dispose();
            pictureBox2.Image.Dispose();
            pictureBox1.Image = Image.FromFile(fileName);
            pictureBox2.Image = Image.FromFile(fileName);
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
