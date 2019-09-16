using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalReporting.DAL
{
    public class ArrivalRepository : IArrivalRepository
    {

        private readonly ArrivalReportingToolEntities dbContext;

        public ArrivalRepository(ArrivalReportingToolEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Arrival arrival)
        {
            dbContext.Arrivals.Add(arrival);
        }

        public IQueryable<Arrival> GetAll()
        {
            return dbContext.Arrivals;
        }
    }
}
