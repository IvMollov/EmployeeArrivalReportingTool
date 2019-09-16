using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalReporting.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ArrivalReportingToolEntities dbContext;
        private IArrivalRepository arrivals;

        public UnitOfWork(ArrivalReportingToolEntities dbContext)
        {
            this.dbContext = dbContext;
        }

        public IArrivalRepository Arrivals
        {
            get
            {
                if (arrivals == null)
                {
                    arrivals = new ArrivalRepository(dbContext);
                }
                return arrivals;
            }
        }


        public async Task CommitAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
