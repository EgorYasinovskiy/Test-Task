using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models.Abstract;

namespace Test_Task.Models.Goods
{
    public class Audiobook : DownloadableOffer
    {
        public string Author { get; set; }
        public string Name { get; set; }
        public string Publisher { get; set; }
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string Language { get; set; }
        public string PerformedBy { get; set; }
        public string PerformanceType { get; set; }
        public string Storage { get; set; }
        public string Format { get; set; }
        public TimeSpan RecordingLength { get; set; }
    }
}
