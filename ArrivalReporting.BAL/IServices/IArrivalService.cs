using ArrivalReporting.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalReporting.BAL.IServices
{
    public interface IArrivalService
    {
        IQueryable<Arrival> GetAll();

        void Add(Arrival arrival);
    }
}
