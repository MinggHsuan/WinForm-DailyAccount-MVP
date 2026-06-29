using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models.DTOs
{
    internal class AccountBookDTO
    {
        public string Date { get; set; }
        public string Price { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Target { get; set; }
        public string PaymentMethods { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }

        public AccountBookDTO(string date, string price, string type, string detail, string target, string payment, string image1, string image2)
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
        public AccountBookDTO()
        {
        }
    }
}
