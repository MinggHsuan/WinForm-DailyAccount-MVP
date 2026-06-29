using Bookkeeping.Contract;
using Bookkeeping.Models;
using Bookkeeping.Models.DTOs;
using Bookkeeping.Repository;
using Bookkeeping.Utility;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Presenter
{
    internal class QueryPresenter : QueryContract.IPresenter
    {
        private IRecordRepository recordRepository;
        private QueryContract.IView view;
        public string filename = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase";
        public QueryPresenter(QueryContract.IView view)
        {
            this.view = view;
            recordRepository = new IRepository();
        }
        public void GetData(DateTime start, DateTime end)
        {
            var records = recordRepository.GetRecords(start, end);
            List<AccountBookDTO> accountBookDTOs = Mapper.Map<Record, AccountBookDTO>(records).ToList();
            //List<RecordModel> recordModels = Mapper.Map<AccountBookDTO, RecordModel>(accountBookDTOs).ToList();
            view.OnGetModelResult(Mapper.Map<AccountBookDTO, RecordModel>(accountBookDTOs).ToList());
        }

        public void RemoveData(AccountBookDTO bookDTO)
        {
            File.Delete(bookDTO.Image1);
            string[] newFileName = bookDTO.Image1.Split(new string[] { "small_" }, 0);
            File.Delete(newFileName[0] + newFileName[1]);

            File.Delete(bookDTO.Image2);
            newFileName = bookDTO.Image2.Split(new string[] { "small_" }, 0);
            File.Delete(newFileName[0] + newFileName[1]);

            recordRepository.DeleteRecord(Mapper.Map<AccountBookDTO, Record>(bookDTO));
            view.OnRemove();
        }
        public void UpdateData(AccountBookDTO bookDTO)
        {
            recordRepository.UpdateRecord(Mapper.Map<AccountBookDTO, Record>(bookDTO));
            view.OnUpdate();
        }
        public void UpdateAndMoveData(string beforeDate, AccountBookDTO bookDTO)
        {
            AccountBookDTO accountBookDTOs;
            RecordModel recordModels;

            string directoryNewPath = Path.Combine(filename, bookDTO.Date);
            if (!Directory.Exists(directoryNewPath))
            {
                Directory.CreateDirectory(directoryNewPath);
            }
            var records = recordRepository.UpdateAndMoveRecord(beforeDate, Mapper.Map<AccountBookDTO, Record>(bookDTO));
            accountBookDTOs = Mapper.Map<Record, AccountBookDTO>(records);
            recordModels = Mapper.Map<AccountBookDTO, RecordModel>(accountBookDTOs);
            //accountBookDTOs = new AccountBookDTO(records.Date, records.Price, records.Type, records.Detail, records.Target, records.PaymentMethods, records.Image1, records.Image2);
            //recordModels = new RecordModel(accountBookDTOs.Date, accountBookDTOs.Price, accountBookDTOs.Type, accountBookDTOs.Detail, accountBookDTOs.Target, accountBookDTOs.PaymentMethods, accountBookDTOs.Image1, accountBookDTOs.Image2);
            view.OnMoveAndUpdate(recordModels);
        }

    }
}
