using ArrivalReporting.BAL.IServices;
using ArrivalReporting.BAL.Services;
using ArrivalReporting.DAL;
using ArrivalReporting.Web.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ArrivalReporting.Web.Controllers
{
    public class HomeController : Controller
    {
        private const string DEFAULT_SORTING = "when_desc";
        private const int DEFAULT_PAGE = 1;

        private IDALService dalService;
        private IArrivalService arrivalService;
        private IUnitOfWork unitOfWork;

        private int _pageSize = 10;

        public HomeController(IDALService dalService, IArrivalService arrivalService, IUnitOfWork unitOfWork)
        {
            this.dalService = dalService;
            this.arrivalService = arrivalService;
            this.unitOfWork = unitOfWork;

            if (ConfigurationManager.AppSettings["PageSize"] != null)
            {
                _pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
            }
        }

        public async Task<ActionResult> Index()
        {
            if (IsApplicationSubscribedForData())
            {
                return GetIdexViewWithArrivals();
            }
          

            string webServiceUrl = ConfigurationManager.AppSettings["WebServiceURL"];
            string callback = Url.Action("ReceiveDataFromService", "Home", null, Request.Url.Scheme);
            DateTime arrivalDay = new DateTime(2016, 3, 10);

            bool success = false;

            try
            {
                var result = await dalService.SubscibeForData(webServiceUrl, arrivalDay, callback);
                if (result != null && result.IsSubscriptionSuccessful)
                {
                    StoreWebServiceToken(result.Token);
                    success = true;
                }
            }
            catch (Exception ex)
            {
                return View($"Error: + {ex.Message}");
            }

            if (!success)
            {
                return View("Error");
            }

            return GetIdexViewWithArrivals();
        }

        public async Task<HttpResponseMessage> ReceiveDataFromService()
        {
            var webServiceToken = GetWebServiceToken();
            bool valid = dalService.ValidateData(Request, webServiceToken);

            if (valid)
            {
                try
                {
                    var arrivals = dalService.ParseData(Request);
                    foreach (var arrival in arrivals)
                    {
                        arrivalService.Add(arrival);
                    }
                    await unitOfWork.CommitAsync();
                    HttpContext.Application["NeedRefresh"] = true;
                }
                catch (Exception)
                {
                    return new HttpResponseMessage() { StatusCode = HttpStatusCode.InternalServerError };
                }
            }

            return new HttpResponseMessage() { StatusCode = HttpStatusCode.OK };
        }

        public ActionResult LoadArrivals(string sortOrder = DEFAULT_SORTING, int page = 1)
        {
            var arrivals = GetArrivals(sortOrder, page, _pageSize);

            return PartialView("Arrivals", arrivals);
        }

        public ActionResult CheckForUpdates()
        {
            if (HttpContext.Application["NeedRefresh"] != null
                && (bool)HttpContext.Application["NeedRefresh"])
            {
                HttpContext.Application["NeedRefresh"] = false;
                return LoadArrivals();
            }
            return new EmptyResult();
        }


        private bool IsApplicationSubscribedForData()
        {
            var webServiceToken = GetWebServiceToken();

            return webServiceToken != null;
        }

        private void StoreWebServiceToken(TokenWebService token)
        {
            HttpContext.Application["WebServiceToken"] = token;
        }

        private TokenWebService GetWebServiceToken()
        {
            if (HttpContext.Application["WebServiceToken"] != null)
            {
                return (TokenWebService)HttpContext.Application["WebServiceToken"];
            }

            return null;
        }

        private ActionResult GetIdexViewWithArrivals()
        {
            var arrivals = GetArrivals(DEFAULT_SORTING, DEFAULT_PAGE, _pageSize);

            return View(new HomeViewModel()
            {
                Arrivals = arrivals
            });
        }

        private IPagedList<Arrival> GetArrivals(string sortOrder, int page, int pageSize)
        {
            ViewBag.EmployeeIdSortParam = sortOrder == "employeeId" ? "employeeId_desc" : "employeeId";
            ViewBag.WhenSortParam = sortOrder == "when" ? "when_desc" : "when";
            ViewBag.CurrentSort = sortOrder;

            var arrivals = arrivalService.GetAll();

            switch (sortOrder)
            {
                case "employeeId":
                    arrivals = arrivals.OrderBy(x => x.EmployeeId);
                    break;
                case "when":
                    arrivals = arrivals.OrderBy(x => x.When);
                    break;
                case "employeeId_desc":
                    arrivals = arrivals.OrderByDescending(x => x.EmployeeId);
                    break;
                case "when_desc":
                    arrivals = arrivals.OrderByDescending(x => x.When);
                    break;
                default:
                    break;
            }

            try
            {
                return arrivals.ToPagedList(page, pageSize);
            }
            catch (Exception)
            {
                return null;
            }

        }
    }
}