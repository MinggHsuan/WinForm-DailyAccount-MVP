using Bookkeeping.Models;
using CSVlibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bookkeeping.Repository
{
    public class Repository : IRecordRepository
    {
        private string filename = $@"C:\Users\user\Desktop\CsharpClass\BookkeepingDataBase";
        public void CreateRecord(Record record)
        {
            string directoryPath = Path.Combine(filename, record.Date);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            directoryPath = Path.Combine(filename, record.Date, "Data.csv");
            CSVHelper.Write<Record>(directoryPath, record);
        }

        public void DeleteRecord(Record record)
        {
            List<Record> records = GetRecords(DateTime.Parse(record.Date));
            Record removeRecord = records.First(x => x.Image1 == record.Image1);
            records.Remove(removeRecord);
            string directoryPath = Path.Combine(filename, record.Date, "Data.csv");
            File.Delete(directoryPath);
            CSVHelper.Write(directoryPath, records);
        }
        public void UpdateRecord(Record record)
        {
            //List<Record> records = GetRecords(DateTime.Parse(record.Date));
            //Record removeRecord = records.First(x => x.Image1 == record.Image1);
            //records.Remove(removeRecord);
            string directoryPath = Path.Combine(filename, record.Date, "Data.csv");
            File.Delete(directoryPath);
            CSVHelper.Write<Record>(directoryPath, record);
        }
        public List<Record> GetRecords(DateTime start, DateTime end)
        {
            List<Record> records = new List<Record>();
            TimeSpan timeSpan = end - start;
            for (int i = 0; i <= timeSpan.Days; i++)
            {
                DateTime currentDay = start.AddDays(i);
                string directoryPath = Path.Combine(filename, currentDay.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(directoryPath))
                {
                    continue;
                }
                directoryPath = Path.Combine(directoryPath, "Data.csv");
                records.AddRange(CSVHelper.Read<Record>(directoryPath));
            }
            return records;

        }

        public List<Record> GetRecords(DateTime date)
        {
            List<Record> records = new List<Record>();
            string directoryPath = Path.Combine(filename, date.ToString("yyyy-MM-dd"), "Data.csv");
            records = CSVHelper.Read<Record>(directoryPath);
            return records;
        }


    }
}
