using Bookkeeping.Models;
using Bookkeeping.Utility;
using CSVlibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping.Views
{
    public partial class 記帳本 : Form
    {
        // DaaGridView構成
        // DataGridViewColumn(欄) : 用反射抓取所有公開屬性,將每一個公開屬性都創建: DataGridViewTextBoxColumn
        // DataGridViewRow(列): 對list跑for迴圈創建每一個DataGridViewRow
        // DAtaGRidViewCell(格) : 對list當中的每一個item都創建一個DaaGridViewCel
        public List<RecordModel> recordModels;
        public 記帳本()
        {
            InitializeComponent();
            dataGridView1.CellMouseDoubleClick += DataGridView1_CellMouseDoubleClick;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            recordModels = new List<RecordModel>();
            TimeSpan timeSpan = endTimePicker.Value.Date - startTimePicker.Value.Date;

            for (int i = 0; i <= timeSpan.Days; i++)
            {
                DateTime currentDay = startTimePicker.Value.AddDays(i);
                if (!Directory.Exists($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{currentDay.ToString("yyyy-MM-dd")}"))
                {
                    continue;
                }
                recordModels.AddRange(CSVHelper.Read<RecordModel>($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{currentDay.ToString("yyyy-MM-dd")}\Data.csv"));
            }

            this.DebounceTime((x) =>
            {
                RefreshView(x);
            }, recordModels, 500);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string removeDate;
            if (e.ColumnIndex == 0)
            {
                DataGridView cell = (DataGridView)sender;
                removeDate = cell.Rows[e.RowIndex].Cells[15].Value.ToString();
                string newDate = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (!Directory.Exists($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{newDate}"))
                {
                    Directory.CreateDirectory($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{newDate}");
                }

                File.Delete($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{newDate}\Data.csv");
                CSVHelper.Write<RecordModel>($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{newDate}\Data.csv", recordModels.Where(x => x.Date == newDate).ToList());
                recordModels.RemoveAt(e.RowIndex);
                File.Delete($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{removeDate}\Data.csv");
                CSVHelper.Write<RecordModel>($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{removeDate}\Data.csv", recordModels.Where(x => x.Date == removeDate).ToList());

                cell.Rows[e.RowIndex].Cells[15].Value = newDate;
                RefreshView(recordModels);
                return;
            }
            if (e.ColumnIndex == 1)
            {
                DataGridView cell = (DataGridView)sender;
                recordModels[e.RowIndex].Price = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            if (e.ColumnIndex == 3)
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex + 2];
                cell.DataSource = DataModel.Detail[dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()];
                cell.Value = DataModel.Detail[dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()][0];
                recordModels[e.RowIndex].Type = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                recordModels[e.RowIndex].Detail = DataModel.Detail[dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()][0];
            }
            if (e.ColumnIndex == 5)
            {
                DataGridView cell = (DataGridView)sender;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                recordModels[e.RowIndex].Detail = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            if (e.ColumnIndex == 7)
            {
                DataGridView cell = (DataGridView)sender;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                recordModels[e.RowIndex].Target = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            if (e.ColumnIndex == 9)
            {
                DataGridView cell = (DataGridView)sender;
                dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                recordModels[e.RowIndex].PaymentMethods = cell.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            }
            removeDate = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            File.Delete($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{removeDate}\Data.csv");
            CSVHelper.Write<RecordModel>($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{removeDate}\Data.csv", recordModels.Where(x => x.Date == removeDate).ToList());

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 14 && e.RowIndex != -1)
            {
                string removeDate = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                File.Delete(dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString());
                string[] newFileName = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString().Split(new string[] { "small_" }, 0);
                File.Delete(newFileName[0] + newFileName[1]);

                File.Delete(dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString());
                newFileName = dataGridView1.Rows[e.RowIndex].Cells[12].Value.ToString().Split(new string[] { "small_" }, 0);
                File.Delete(newFileName[0] + newFileName[1]);

                File.Delete($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{removeDate}\Data.csv");
                recordModels.RemoveAt(e.RowIndex);
                CSVHelper.Write<RecordModel>($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{removeDate}\Data.csv", recordModels.Where(x => x.Date == removeDate).ToList());

                RefreshView(recordModels);
            }
        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridViewColumn column = dataGridView1.Columns[dataGridView1.CurrentCell.ColumnIndex];
            if (column is DataGridViewComboBoxColumn)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
                dataGridView1.EndEdit();
            }
        }

        private void DataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 11 && e.RowIndex != -1 || e.ColumnIndex == 13 && e.RowIndex != -1)
            {
                ImageForm form = new ImageForm(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString());
                form.Text = dataGridView1.Columns[e.ColumnIndex].HeaderText;
                form.Show();
            }

        }


        private void RefreshView(List<RecordModel> model)
        {
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                Image image = (Image)dataGridView1.Rows[j].Cells[10 + 1].Value;
                image?.Dispose();
                Image image2 = (Image)dataGridView1.Rows[j].Cells[12 + 1].Value;
                image2?.Dispose();
                Image image3 = (Image)dataGridView1.Rows[j].Cells[14].Value;
                image3?.Dispose();
            }
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = model;

            DataGridViewComboBoxColumn TypeComboBoxColumn = new DataGridViewComboBoxColumn();
            TypeComboBoxColumn.HeaderText = "類型選單";
            TypeComboBoxColumn.DataSource = DataModel.Type;
            dataGridView1.Columns.Insert(2 + 1, TypeComboBoxColumn);
            dataGridView1.Columns[2].Visible = false;

            DataGridViewComboBoxColumn DetailComboBoxColumn = new DataGridViewComboBoxColumn();
            DetailComboBoxColumn.HeaderText = "細項選單";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns.Insert(4 + 1, DetailComboBoxColumn);

            DataGridViewComboBoxColumn TargetComboBoxColumn = new DataGridViewComboBoxColumn();
            TargetComboBoxColumn.HeaderText = "對象選單";
            TargetComboBoxColumn.DataSource = DataModel.Target;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns.Insert(6 + 1, TargetComboBoxColumn);

            DataGridViewComboBoxColumn PayComboBoxColumn = new DataGridViewComboBoxColumn();
            PayComboBoxColumn.HeaderText = "支付方式選單";
            PayComboBoxColumn.DataSource = DataModel.PaymentMethods;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns.Insert(8 + 1, PayComboBoxColumn);

            DataGridViewImageColumn ImageColumn1 = new DataGridViewImageColumn();
            ImageColumn1.HeaderText = dataGridView1.Columns[10].HeaderText.Replace("檔", "片");
            ImageColumn1.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns.Insert(10 + 1, ImageColumn1);

            DataGridViewImageColumn ImageColumn2 = new DataGridViewImageColumn();
            ImageColumn2.HeaderText = dataGridView1.Columns[12].HeaderText.Replace("檔", "片");
            ImageColumn2.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns[12].Visible = false;
            dataGridView1.Columns.Insert(12 + 1, ImageColumn2);

            DataGridViewImageColumn garbageCan = new DataGridViewImageColumn();
            garbageCan.HeaderText = "刪除";
            garbageCan.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dataGridView1.Columns.Add(garbageCan);


            DataGridViewTextBoxColumn DateTextBoxColumn = new DataGridViewTextBoxColumn();
            DateTextBoxColumn.HeaderText = "舊日期";
            dataGridView1.Columns.Add(DateTextBoxColumn);
            dataGridView1.Columns[15].Visible = false;

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                dataGridView1.Rows[j].Cells[15].Value = dataGridView1.Rows[j].Cells[0].Value;

                dataGridView1.Rows[j].Cells[2 + 1].Value = dataGridView1.Rows[j].Cells[2].Value;

                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView1.Rows[j].Cells[4 + 1];
                cell.DataSource = DataModel.Detail[dataGridView1.Rows[j].Cells[2].Value.ToString()];
                cell.Value = dataGridView1.Rows[j].Cells[4].Value;

                dataGridView1.Rows[j].Cells[6 + 1].Value = dataGridView1.Rows[j].Cells[6].Value;

                dataGridView1.Rows[j].Cells[8 + 1].Value = dataGridView1.Rows[j].Cells[8].Value;


                byte[] imageByte = File.ReadAllBytes(dataGridView1.Rows[j].Cells[10].Value.ToString());
                MemoryStream memoryStream = new MemoryStream(imageByte);
                dataGridView1.Rows[j].Cells[10 + 1].Value = Image.FromStream(memoryStream);


                imageByte = File.ReadAllBytes(dataGridView1.Rows[j].Cells[12].Value.ToString());
                memoryStream = new MemoryStream(imageByte);
                dataGridView1.Rows[j].Cells[12 + 1].Value = Image.FromStream(memoryStream);


                imageByte = File.ReadAllBytes(@"C:\Users\user\Desktop\CsharpClass\程式圖片\垃圾桶.jpg");
                memoryStream = new MemoryStream(imageByte);
                dataGridView1.Rows[j].Cells[14].Value = Image.FromStream(memoryStream);

            }
        }
    }
}

