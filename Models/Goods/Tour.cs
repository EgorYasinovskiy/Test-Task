using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models.Abstract;

namespace Test_Task.Models.Goods
{
    public class Tour : DeliveredOffer
    {
        public string WorldRegion { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public int Days { get; set; }
        public DateTime DataTourStart { get; set; }
        public DateTime DataTourEnd { get; set; }
        public string Name { get; set; }
        public int HotelStars { get; set; }
        public string Room { get; set; }
        public string Meal { get; set; }
        public string[] Included { get; set; }
        public string Transport { get; set; }
    }
}
