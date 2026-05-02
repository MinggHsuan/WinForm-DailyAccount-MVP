using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Repository
{
    public interface IDataModelRepository
    {
        DataModel GetDataSource();
        List<string> GetDetail(string type);
    }
}
