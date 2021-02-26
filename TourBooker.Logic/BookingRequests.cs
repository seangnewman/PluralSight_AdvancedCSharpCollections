using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourBooker.Logic
{
    public struct BookingRequests
    {
        public Customer Customer { get;  }
        public Tour RequestedTour { get; }
    }
}
