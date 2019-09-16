using ArrivalReporting.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ArrivalReporting.Web.Models
{
    public class HomeViewModel
    {
        public PagedList.IPagedList<Arrival> Arrivals { get; set; }
    }
}