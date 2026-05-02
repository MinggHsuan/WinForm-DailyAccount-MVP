using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class DataModelDTO
    {
        public string[] Type;
        public List<string> Detail;
        public string[] Target;
        public string[] PaymentMethods;

        public DataModelDTO(string[] type, List<string> detail, string[] target, string[] paymentMethods)
        {
            Type = type;
            Detail = detail;
            Target = target;
            PaymentMethods = paymentMethods;
        }
    }
}
