using Bookkeeping.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookkeeping.Models
{
    public class RecordModel
    {
        [DisplayName("日期")]
        [TextBoxColumn]
        public string Date { get; set; }
        [DisplayName("金額")]
        [TextBoxColumn]
        public string Price { get; set; }
        [DisplayName("類型")]
        [ComboBoxColumn]
        public string Type { get; set; }
        [DisplayName("細項")]
        [ComboBoxColumn]
        public string Details { get; set; }
        [DisplayName("對象")]
        [ComboBoxColumn]

        public string Target { get; set; }
        [DisplayName("支付方式")]
        [ComboBoxColumn]

        public string PaymentMethods { get; set; }
        [DisplayName("圖檔1")]
        [ImageColumn]
        public string Image1 { get; set; }
        [DisplayName("圖檔2")]
        [ImageColumn]
        public string Image2 { get; set; }
        public RecordModel(string date, string price, string type, string detail, string target, string payment, string image1, string image2)
        {
            this.Date = date;
            this.Price = price;
            this.Type = type;
            this.Details = detail;
            this.Target = target;
            this.PaymentMethods = payment;
            this.Image1 = image1;
            this.Image2 = image2;
        }


        public RecordModel()
        {
        }
    }
}
