using Bookkeeping.Models;
using Bookkeeping.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Contract
{
    internal class QueryContract
    {
        public interface IView
        {
            void OnGetModelResult(List<RecordModel> recordModels);
            void OnRemove();
            void OnUpdate();
            void OnMoveAndUpdate(RecordModel recordModel);
        }
        public interface IPresenter
        {
            void GetData(DateTime start, DateTime end);
            void RemoveData(AccountBookDTO bookDTO);
            void UpdateData(AccountBookDTO bookDTO);
            void UpdateAndMoveData(string beforeDate, AccountBookDTO bookDTO);
        }
    }

}
