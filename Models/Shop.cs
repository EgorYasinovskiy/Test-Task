using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Interfaces;

namespace Test_Task.Models
{
    public class Shop
    {
        public string Name { get; set; }
        public string Company { get; set; }
        public string Url { get; set; }
        public int LocalDeliveryCost { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<Category> Categories { get; set; }
        public List<IOffer> Offers { get; set; }
    }
}
