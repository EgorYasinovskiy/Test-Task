using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models;

namespace Test_Task.Interfaces
{
    public interface IOffer
    {
        public int Id { get; set; }
        public int BId { get; set; }
        public int? CbId { get; set; }
        public bool Available { get; set; }
        public string Url { get; set; }
        public int Price { get; set; }
        public Currency Currency { get; set; }
        public Category Category { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }
    }
}