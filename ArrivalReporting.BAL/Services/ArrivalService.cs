using ArrivalReporting.BAL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArrivalReporting.DAL;

namespace ArrivalReporting.BAL.Services
{
    public class ArrivalService : IArrivalService
    {
        private IArrivalRepository repository;

        public ArrivalService(IUnitOfWork unitOfWork)
        {
            repository = unitOfWork.Arrivals;
        }

        public void Add(Arrival arrival)
        {
            repository.Add(arrival);
        }

        public IQueryable<Arrival> GetAll()
        {
            return repository.GetAll();
        }
        
    }
}
