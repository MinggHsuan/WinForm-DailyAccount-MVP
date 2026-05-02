using Bookkeeping.Contract;
using Bookkeeping.Models;
using Bookkeeping.Models.DTOs;
using Bookkeeping.Repository;
using Bookkeeping.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping.Presenter
{
    internal class AddRecordPresenter : AddRecordContract.IPresenter
    {
        private IDataModelRepository dataModelRepository;
        private IRecordRepository recordRepository;
        private AddRecordContract.IView view;
        public AddRecordPresenter(AddRecordContract.IView view)
        {
            this.view = view;
            dataModelRepository = new DataModel();
            recordRepository = new IRepository();
        }
        public void GetDataSource()
        {
            var records = dataModelRepository.GetDataSource();
            view.OnGetResult(new DataModelDTO(records.Type, records.Detail[records.Type[0]], records.Target, records.PaymentMethods));
        }

        public void GetDetail(string type)
        {
            var detail = dataModelRepository.GetDetail(type);
            view.OnDetailResult(detail);
        }

        public void AddRecord(RecordDTO recordDTO)
        {
            string Imagefile1 = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{recordDTO.Date}\small_{Guid.NewGuid()}.jpg";
            string Imagefile2 = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{recordDTO.Date}\small_{Guid.NewGuid()}.jpg";
            if (!Directory.Exists($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{recordDTO.Date}"))
            {
                Directory.CreateDirectory($@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase\{recordDTO.Date}");
            }
            var image1 = ImageCompress.Compress(recordDTO.Image1, 15L);
            string[] newFileName = Imagefile1.Split(new string[] { "small_" }, 0);
            image1.Save(newFileName[0] + newFileName[1]);

            var image2 = ImageCompress.Compress(recordDTO.Image2, 15L);
            newFileName = Imagefile2.Split(new string[] { "small_" }, 0);
            image2.Save(newFileName[0] + newFileName[1]);

            Bitmap resizeImage1 = ImageCompress.Compress(recordDTO.Image1, 40, 40);
            resizeImage1.Save(Imagefile1);
            Bitmap resizeImage2 = ImageCompress.Compress(recordDTO.Image2, 40, 40);
            resizeImage2.Save(Imagefile2);

            recordRepository.CreateRecord(new Record(recordDTO.Date, recordDTO.Price, recordDTO.Type, recordDTO.Detail, recordDTO.Target, recordDTO.PaymentMethods, Imagefile1, Imagefile2));
            view.ReSetUI();
        }


    }
}
