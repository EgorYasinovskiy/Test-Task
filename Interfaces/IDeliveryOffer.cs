using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Task.Interfaces
{
    public interface IDeliveryOffer : IOffer
    {
        public bool Delivery { get; set; }
        public int LocalDeliveryCost { get; set; }
    }
}
