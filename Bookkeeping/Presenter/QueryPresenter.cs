using Bookkeeping.Contract;
using Bookkeeping.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Presenter
{
    internal class QueryPresenter : QueryContract.IPresenter
    {
        private IRecordRepository recordRepository;
        private QueryContract.IView view;
        public QueryPresenter(QueryContract.IView view)
        {
            this.view = view;
        }
        public void getResult(DateTime start, DateTime end)
        {
            var records = recordRepository.GetRecords(start, end);
            view.onGetResult(records);
        }


    }
}
