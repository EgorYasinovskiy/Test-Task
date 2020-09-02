using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models.Abstract;

namespace Test_Task.Models.Goods
{
    public class Book : DownloadableDeliveredOffer
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string Series { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public int Volume { get; set; }
        public int Part { get; set; }
        public string Language { get; set; }
        public string Binding { get; set; }
        public int PageExtent { get; set; }
    }
}
