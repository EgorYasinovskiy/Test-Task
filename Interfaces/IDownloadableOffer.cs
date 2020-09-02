using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Task.Interfaces
{
    public interface IDownloadableOffer : IOffer
    {
        public bool Downloadable { get; set; }
    }
}
