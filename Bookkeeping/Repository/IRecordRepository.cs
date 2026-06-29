using Bookkeeping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Repository
{
    public interface IRecordRepository
    {
        void CreateRecord(Record record);
        void DeleteRecord(Record record);
        void UpdateRecord(Record record);
        Record UpdateAndMoveRecord(string beforeDate, Record record);
        List<Record> GetRecords(DateTime start, DateTime end);
        List<Record> GetRecords(DateTime date);

    }
}
