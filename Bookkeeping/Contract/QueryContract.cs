using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Contract
{
    public class QueryContract
    {
        public interface IView
        {
            void onGetResult(List<Record> records);
        }
        public interface IPresenter
        {
            void getResult(DateTime start, DateTime end);
        }
    }

}
