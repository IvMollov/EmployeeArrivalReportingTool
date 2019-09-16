using ArrivalReporting.BAL.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArrivalReporting.DAL;
using System.Web;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace ArrivalReporting.BAL.Services
{
    public class DALService : IDALService
    {
        private const string WEB_SERVICE_TOKEN = "X-Fourth-Token";
        private const string ACCEPT_CLIENT_HEADER = "Fourth-Monitor";
        private string ARRIVALS_URL = "api/clients/subscribe";

        public async Task<DALServiceResponse> SubscibeForData(string url, DateTime arrivalDate, string callback)
        {
            DALServiceResponse result = new DALServiceResponse();

            using (HttpClient client = new HttpClient())
            {
                string request = $"{url}{ARRIVALS_URL}?date={arrivalDate.ToString("yyyy-MM-dd")}&callback={callback}";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add("Accept-Client", ACCEPT_CLIENT_HEADER);

                HttpResponseMessage responseMessage = await client.GetAsync(request);

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    result.IsSubscriptionSuccessful = true;
                    var body = await responseMessage.Content.ReadAsStringAsync();
                    result.Token = (TokenWebService)JsonConvert.DeserializeObject(body, typeof(TokenWebService));
                }
            }

            return result;
        }

        public bool ValidateData(HttpRequestBase request, TokenWebService serviceToken)
        {
            if (request.Headers != null && request.Headers[WEB_SERVICE_TOKEN] == serviceToken.Token)
                return true;

            return false;
        }

        public IEnumerable<Arrival> ParseData(HttpRequestBase request)
        {
            using (var stream = new StreamReader(request.InputStream))
            {
                string jsonData = stream.ReadToEnd();
                var arrivals = (List<Arrival>)JsonConvert.DeserializeObject(jsonData, typeof(List<Arrival>));

                return arrivals;
            }
        }
    }
}
