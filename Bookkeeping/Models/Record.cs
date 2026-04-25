using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    internal class Record
    {
        public string Date { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Target { get; set; }
        public string PaymentMethods { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
    }
}
