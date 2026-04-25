using Bookkeeping.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    internal class DataModel : IDataModelRepository
    {
        public static string[] Type = { "食", "衣", "住", "行" };

        public static List<string> FoodData = new List<string> { "我家牛排", "島嶼", "海港" };
        public static List<string> DressData = new List<string> { "Uniqlo", "NET", "ZARA" };
        public static List<string> StayData = new List<string> { "洲際酒店", "圓山飯店", "義大皇家酒店" };
        public static List<string> TrafficData = new List<string> { "機車", "開車", "計程車", "高鐵" };
        public static Dictionary<string, List<string>> Detail = new Dictionary<string, List<string>>{
            {"食",FoodData},
            {"衣",DressData},
            {"住",StayData},
            {"行",TrafficData}
        };
        public static string[] Target = { "自用", "爸爸", "媽媽", "朋友", "女朋友" };
        public static string[] PaymentMethods = { "現金", "信用卡", "行動支付" };

        public DataModelDTO GetDataSource()
        {
            return new DataModelDTO(Type, Detail[Type[0]], Target, PaymentMethods);
        }

        public List<string> GetDetail(string type)
        {
            return Detail[type];
        }
    }
}
