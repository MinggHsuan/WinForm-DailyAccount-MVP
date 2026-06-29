using Bookkeeping.Attributes;
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
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using Image = System.Drawing.Image;

namespace Bookkeeping.Views
{
    public partial class 記帳本 : Form, QueryContract.IView
    {
        // DaaGridView構成
        // DataGridViewColumn(欄) : 用反射抓取所有公開屬性,將每一個公開屬性都創建: DataGridViewTextBoxColumn
        // DataGridViewRow(列): 對list跑for迴圈創建每一個DataGridViewRow
        // DAtaGRidViewCell(格) : 對list當中的每一個item都創建一個DaaGridViewCel
        private QueryContract.IPresenter presenter;
        public List<RecordModel> recordModels;
        //public string filename = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase";
        public string garbagePath = @"C:\Users\user\Desktop\CsharpClass\程式圖片\垃圾桶.jpg";
        public 記帳本()
        {
            InitializeComponent();
            presenter = new QueryPresenter(this);
            dataGridView1.CellMouseDoubleClick += DataGridView1_CellMouseDoubleClick;
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            recordModels = new List<RecordModel>();
            presenter.GetData(startTimePicker.Value.Date, endTimePicker.Value.Date);
        }
        public void OnGetModelResult(List<RecordModel> recordModels)
        {
            this.DebounceTime(() =>
            {
                this.recordModels = recordModels;
                RefreshView(recordModels);
            }, 500);
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var column = dataGridView1.Columns[e.ColumnIndex];
            var UpdateData = recordModels[e.RowIndex];
            if (column.Tag == null)
            {
                return;
            }
            string Date = dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["Date"].Index].Value.ToString();
            if (column.Tag.ToString() == "Date")
            {
                Date = dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns[column.Tag.ToString()].Index + 1].Value.ToString();
                if (Date == UpdateData.Date)
                {
                    return;
                }
                recordModels.Remove(UpdateData);
                presenter.UpdateAndMoveData(Date, Mapper.Map<RecordModel, AccountBookDTO>(UpdateData));
                //presenter.UpdateAndMoveData(Date, new AccountBookDTO(UpdateData.Date, UpdateData.Price, UpdateData.Type, UpdateData.Details, UpdateData.Target, UpdateData.PaymentMethods, UpdateData.Image1, UpdateData.Image2));
                return;
            }
            if (column.Tag.ToString() == "Type")
            {
                DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns["Details"].Index + 1];
                cell.DataSource = DataModel.Details[dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns[column.Tag.ToString()].Index + 1].Value.ToString()];
                cell.Value = DataModel.Details[dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns[column.Tag.ToString()].Index + 1].Value.ToString()][0];
            }
            presenter.UpdateData(Mapper.Map<RecordModel, AccountBookDTO>(UpdateData));


        }
        public void OnUpdate()
        {
            RefreshView(recordModels);
        }
        public void OnMoveAndUpdate(RecordModel recordModel)
        {
            RefreshView(recordModels);
            recordModels.Add(recordModel);
            recordModels.OrderBy(x => x.Date);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column.Tag.ToString() == "刪除" && e.RowIndex != -1)
            {
                var removeData = recordModels[e.RowIndex];
                recordModels.Remove(removeData);
                presenter.RemoveData(Mapper.Map<RecordModel, AccountBookDTO>(removeData));
                //presenter.RemoveData(new AccountBookDTO(removeData.Date, removeData.Price, removeData.Type, removeData.Details, removeData.Target, removeData.PaymentMethods, removeData.Image1, removeData.Image2));
            }
        }
        public void OnRemove()
        {
            RefreshView(recordModels);
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
            var column = dataGridView1.Columns[e.ColumnIndex];
            if (column is DataGridViewImageColumn && e.RowIndex != -1 && column.Tag.ToString() != "刪除")
            {
                ImageForm form = new ImageForm(dataGridView1.Rows[e.RowIndex].Cells[dataGridView1.Columns[column.Tag.ToString()].Index].Value.ToString());
                form.Text = column.HeaderText;
                form.Show();
            }
        }


        private void RefreshView(List<RecordModel> model)
        {
            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                dataGridView1.Rows[j].Cells.OfType<DataGridViewImageCell>()
                   .ToList()
                   .ForEach(x =>
                   {
                       Image image;
                       if (x.OwningColumn.Tag.ToString() == "刪除")
                       {
                           image = (Image)dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index].Value;
                           image?.Dispose();
                       }
                       else
                       {
                           image = (Image)dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index + 1].Value;
                           image?.Dispose();
                       }
                   });
            }
            dataGridView1.DataSource = null;
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = model;


            var props = typeof(RecordModel).GetProperties();
            var dataModels = typeof(DataModel).GetFields();

            foreach (var prop in props)
            {
                string headerText = prop.GetCustomAttribute<DisplayNameAttribute>().DisplayName;
                int index;
                if (prop.GetCustomAttribute<TextBoxColumnAttribute>() is TextBoxColumnAttribute)
                {
                    var column = dataGridView1.Columns[prop.Name];
                    if (prop.Name == "Date")
                    {
                        DataGridViewTextBoxColumn TextBoxColumn = new DataGridViewTextBoxColumn();
                        TextBoxColumn.HeaderText = $"{headerText}欄位";
                        TextBoxColumn.Name = $"{prop.Name}_TextBox";
                        TextBoxColumn.Tag = $"{prop.Name}_TextBox";
                        index = dataGridView1.Columns[prop.Name].Index;
                        dataGridView1.Columns.Insert(index + 1, TextBoxColumn);
                        dataGridView1.Columns[index + 1].Visible = false;
                    }
                    column.Tag = prop.Name;
                    column.DataPropertyName = prop.Name;
                }
                if (prop.GetCustomAttribute<ComboBoxColumnAttribute>() is ComboBoxColumnAttribute)
                {
                    DataGridViewComboBoxColumn ComboBoxColumn = new DataGridViewComboBoxColumn();
                    ComboBoxColumn.HeaderText = $"{headerText}選單";
                    ComboBoxColumn.Name = $"{prop.Name}_ComboBox";
                    ComboBoxColumn.Tag = prop.Name;
                    if (prop.Name != "Details")
                    {
                        ComboBoxColumn.DataSource = dataModels.FirstOrDefault(x => x.Name == prop.Name).GetValue(new DataModel());
                    }
                    ComboBoxColumn.DataPropertyName = prop.Name;
                    index = dataGridView1.Columns[prop.Name].Index;
                    dataGridView1.Columns[index].Visible = false;
                    dataGridView1.Columns.Insert(index + 1, ComboBoxColumn);

                }
                if (prop.GetCustomAttribute<ImageColumnAttribute>() is ImageColumnAttribute)
                {
                    DataGridViewImageColumn ImageColumn = new DataGridViewImageColumn();
                    ImageColumn.HeaderText = headerText.Replace("檔", "片");
                    ImageColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                    ImageColumn.Name = $"{prop.Name}_Image";
                    ImageColumn.Tag = prop.Name;
                    //ImageColumn.DataPropertyName = prop.Name;
                    index = dataGridView1.Columns[prop.Name].Index;
                    dataGridView1.Columns[index].Visible = false;
                    dataGridView1.Columns.Insert(index + 1, ImageColumn);
                }
            }

            DataGridViewImageColumn garbageCan = new DataGridViewImageColumn();
            garbageCan.HeaderText = "刪除";
            garbageCan.ImageLayout = DataGridViewImageCellLayout.Zoom;
            garbageCan.Name = "刪除";
            garbageCan.Tag = "刪除";
            dataGridView1.Columns.Add(garbageCan);

            for (int j = 0; j < dataGridView1.Rows.Count; j++)
            {
                dataGridView1.Rows[j].Cells.OfType<DataGridViewTextBoxCell>()
                  .Where(y => y.OwningColumn.Name == "Date")
                  .ToList()
                  .ForEach(x =>
                  {
                      dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index + 1].Value = dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index].Value;
                  });


                dataGridView1.Rows[j].Cells.OfType<DataGridViewComboBoxCell>()
                    .ToList()
                    .ForEach(x =>
                    {
                        if (x.OwningColumn.Tag.ToString() == "Details")
                        {
                            x.DataSource = DataModel.Details[dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index - 1].Value.ToString()];
                            x.Value = dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index].Value;
                        }
                        dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index + 1].Value = dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index].Value;
                    });


                dataGridView1.Rows[j].Cells.OfType<DataGridViewImageCell>()
                    .ToList()
                    .ForEach(x =>
                    {
                        byte[] imageByte;
                        MemoryStream memoryStream;
                        if (x.OwningColumn.Tag.ToString() == "刪除")
                        {
                            imageByte = File.ReadAllBytes(garbagePath);
                            memoryStream = new MemoryStream(imageByte);
                            dataGridView1.Rows[j].Cells[dataGridView1.Columns.GetLastColumn(0, 0).Index].Value = Image.FromStream(memoryStream);
                        }
                        else
                        {
                            imageByte = File.ReadAllBytes(dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index].Value.ToString());
                            memoryStream = new MemoryStream(imageByte);
                            dataGridView1.Rows[j].Cells[dataGridView1.Columns[x.OwningColumn.Tag.ToString()].Index + 1].Value = Image.FromStream(memoryStream);
                        }
                    });


            }
        }


    }
}

