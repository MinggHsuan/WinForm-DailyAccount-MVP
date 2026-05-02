using Bookkeeping.Models;
using Bookkeeping.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Contract
{
    public class AddRecordContract
    {
        public interface IView
        {
            void OnGetResult(DataModelDTO records);
            void OnDetailResult(List<string> Detail);
            void ReSetUI();
        }
        public interface IPresenter
        {
            void GetDataSource();
            void GetDetail(string type);
            void AddRecord(RecordDTO recordDTO);
        }
    }

}
