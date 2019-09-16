using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArrivalReporting.DAL
{
    public interface IUnitOfWork
    {
        IArrivalRepository Arrivals { get; }

        Task CommitAsync();
    }
}
