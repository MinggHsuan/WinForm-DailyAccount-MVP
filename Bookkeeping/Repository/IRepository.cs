using Bookkeeping.Models;
using Bookkeeping.Utility;
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
    public class IRepository : IRecordRepository
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
            if (records.Count == 0)
            {
                directoryPath = Path.Combine(filename, record.Date);
                Directory.Delete(directoryPath);
                return;
            }
            CSVHelper.Write(directoryPath, records);
        }
        public void UpdateRecord(Record record)
        {
            List<Record> records = GetRecords(DateTime.Parse(record.Date));
            //var config = new MapperConfiguration(x => x.CreateMap<Record, Record>());
            //var mapper = config.CreateMapper();
            //record = mapper.Map<Record>(records.First(x => x.Image1 == record.Image1));
            record = Mapper.Map<Record, Record>(records.First(x => x.Image1 == record.Image1));
            string directoryPath = Path.Combine(filename, record.Date, "Data.csv");
            File.Delete(directoryPath);
            CSVHelper.Write<Record>(directoryPath, records);
        }
        public Record UpdateAndMoveRecord(string beforeDate, Record record)
        {
            List<Record> records = GetRecords(DateTime.Parse(beforeDate));
            Record moveRecord = records.First(x => x.Image1 == record.Image1);

            string[] small_SourceFileName = moveRecord.Image1.Split(new string[] { $"{beforeDate}" }, 0);
            string small_DestFileName = $"{small_SourceFileName[0]}{record.Date}{small_SourceFileName[1]}";
            File.Move(moveRecord.Image1, small_DestFileName);

            string[] big_SourceFileName = moveRecord.Image1.Split(new string[] { "small_" }, 0);
            string[] big_DestFileName = small_DestFileName.Split(new string[] { "small_" }, 0);
            File.Move(big_SourceFileName[0] + big_SourceFileName[1], big_DestFileName[0] + big_DestFileName[1]);
            moveRecord.Image1 = small_DestFileName;

            small_SourceFileName = moveRecord.Image2.Split(new string[] { $"{beforeDate}" }, 0);
            small_DestFileName = $"{small_SourceFileName[0]}{record.Date}{small_SourceFileName[1]}";
            File.Move(moveRecord.Image2, small_DestFileName);

            big_SourceFileName = moveRecord.Image2.Split(new string[] { "small_" }, 0);
            big_DestFileName = small_DestFileName.Split(new string[] { "small_" }, 0);
            File.Move(big_SourceFileName[0] + big_SourceFileName[1], big_DestFileName[0] + big_DestFileName[1]);
            moveRecord.Image2 = small_DestFileName;
            moveRecord.Date = record.Date;

            string directoryPath = Path.Combine(filename, beforeDate, "Data.csv");
            File.Delete(directoryPath);
            var beforeRecord = records.Where(x => x.Date == beforeDate).ToList();
            if (beforeRecord.Count == 0)
            {
                directoryPath = Path.Combine(filename, beforeDate);
                Directory.Delete(directoryPath);
            }
            else
            {
                CSVHelper.Write<Record>(directoryPath, beforeRecord);
            }

            directoryPath = Path.Combine(filename, moveRecord.Date, "Data.csv");
            if (!File.Exists(directoryPath))
            {
                CSVHelper.Write<Record>(directoryPath, moveRecord);
                return moveRecord;
            }
            records = GetRecords(DateTime.Parse(moveRecord.Date));
            records.Add(moveRecord);
            File.Delete(directoryPath);
            CSVHelper.Write<Record>(directoryPath, records);
            return moveRecord;
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
