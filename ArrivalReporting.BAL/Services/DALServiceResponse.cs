using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalReporting.BAL.Services
{
    public class DALServiceResponse
    {
        public TokenWebService Token { get; set; }

        public bool IsSubscriptionSuccessful { get; set; }
    }
}
