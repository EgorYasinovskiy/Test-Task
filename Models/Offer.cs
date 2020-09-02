using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Task.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public int BId { get; set; }
        public int? CId { get; set; }
        public bool Avaliable { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }
        public string CurrencyID { get; set; }
        public Currency Currency { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
    }
}