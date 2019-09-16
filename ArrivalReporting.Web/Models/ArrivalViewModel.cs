using ArrivalReporting.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrivalReporting.Web.Models
{
    public class ArrivalViewModel
    {
        public int? EmployeeId { get; set; }
        public DateTime? When { get; set; }

        public ArrivalViewModel(Arrival arrival)
        {
            EmployeeId = arrival.EmployeeId;
            When = arrival.When;
        }
    }
}