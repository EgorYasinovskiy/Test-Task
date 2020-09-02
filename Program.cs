using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_Task.Models;

namespace Test_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Parser parser = new Parser();
            var a = parser.GetShopFromUrl("http://partner.market.yandex.ru/pages/help/YML.xml").Result;
            using (StreamWriter sw = new StreamWriter("ids.txt",false,Encoding.Unicode))
            {
                foreach (var offer in a.Offers)
                {
                    sw.WriteLine(offer.Id);
                }
            }
        }
    }
}
