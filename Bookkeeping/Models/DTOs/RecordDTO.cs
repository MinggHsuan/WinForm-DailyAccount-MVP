using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models.DTOs
{
    public class RecordDTO
    {
        public string Date { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Target { get; set; }
        public string PaymentMethods { get; set; }
        public Image Image1 { get; set; }
        public Image Image2 { get; set; }

        public RecordDTO(string date, string price, string type, string detail, string target, string payment, Image image1, Image image2)
        {
            this.Date = date;
            this.Price = price;
            this.Type = type;
            this.Detail = detail;
            this.Target = target;
            this.PaymentMethods = payment;
            this.Image1 = image1;
            this.Image2 = image2;
        }
    }
}
