using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models.Abstract;

namespace Test_Task.Models.Goods
{
    public class ArtistTitle : DeliveredOffer
    {
        public string Artist { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Media { get; set; }
        public string[] Starring { get; set; }
        public string Director { get; set; }
        public string OriginalName { get; set; }
        public string Country { get; set; }

    }
}
