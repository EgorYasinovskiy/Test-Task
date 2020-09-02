using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models.Abstract;
namespace Test_Task.Models.Goods
{
    public class EventTicket:DeliveredOffer
    {
        public string Name { get; set; }
        public string Place { get; set; }
        public string Hall { get; set; }
        public string HallPart { get; set; }
        public DateTime Date { get; set; }
        public bool IsPremiere { get; set; }
        public bool IsKids { get; set; }

    }
}
