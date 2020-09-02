using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models.Abstract;

namespace Test_Task.Models.Goods
{
    public class VendorModel : DeliveredOffer
    {
        public string TypePrefix { get; set; }
        public string Vendor { get; set; }
        public string VendorCode { get; set; }
        public string Model { get; set; }
        public bool ManufacturerWarranty { get; set; }
        public string CountryOfOrigin { get; set; }
    }   
}
