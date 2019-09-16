using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ArrivalReporting.DAL;
using ArrivalReporting.BAL.Services;

namespace ArrivalReporting.BAL.IServices
{
    public interface IDALService
    {
        IEnumerable<Arrival> ParseData(HttpRequestBase request);

        Task<DALServiceResponse> SubscibeForData(string url, DateTime arrivalDate, string callback);

        bool ValidateData(HttpRequestBase request, TokenWebService serviceToken);
    }
}
