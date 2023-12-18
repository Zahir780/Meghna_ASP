using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ESL.Models;
using ESL.Models.ViewModels;
using ESL.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using ESL.DataAccess;
using ESL.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ECL.Services.Payroll;
using ESL.DataAccess.Data;
using Microsoft.AspNetCore.Http;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;
        private IUnitOfWork _unitOfWork;
        private IBackgroundTaskQueue _backgroundTaskQueue;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        private DashboardHRMService service;

        public HomeController
        (
            ILogger<HomeController> logger,
            IUnitOfWork unitOfWork,
            IBackgroundTaskQueue backgroundTaskQueue,
            ApplicationDbContext db,
            IHttpContextAccessor accessor
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _backgroundTaskQueue = backgroundTaskQueue;
            _accessor = accessor;
            _db = db;
            service = new DashboardHRMService(_unitOfWork, _accessor, _db);
        }

        public IActionResult Index()
        {
            DateTime date = DateTime.Now;
            InitDashboardData(date.ToString("yyyy-MM-dd HH:mm:ss"));
            return View();
        }

        [HttpGet]
        #region Dashboard HRM

        #endregion
        public IActionResult GetEmployee()
        {
            var obj = service.getEmployeeInfo();
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }
        public IActionResult GetYear()
        {
            var obj = service.getYear();
            if (obj != null)
            {
                return Json(new SelectList(obj, "Id", "Name"));
            }
            return Json(new { });
        }
        public IActionResult GetMonthAndSalary(string year)
        {
            var obj = service.getMonthWithSalary(year);
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }
        public IActionResult GetTypeWiseSalary(string month)
        {
            var obj = service.getTypeWiseSalary(month);
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }
        public IActionResult GetOverTimeChart()
        {
            var obj = service.getOverTimeChart();
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }
        public IActionResult getMealAllowChart()
        {
            var obj = service.getMealAllowChart();
            if (obj != null)
            {
                return Json(new { data = obj });
            }
            return Json(new { });
        }








        public IActionResult GetCurrentMonthProjectBarChart(Boolean isAll)
        {
            var barChartObj = new List<String>();
            return Json(barChartObj);
        }

        [HttpGet]
        public IActionResult GetEachMonthProjectBarChart()
        {
            var barChartObj = new List<String>() ;
            return Json(barChartObj);
        }
        [HttpGet]
        public IActionResult GetProjectEmployeeDonutChart()
        {
            var paiChartObj = new List<String>();
            return Json(paiChartObj);
        }
        [HttpGet]
        public IActionResult GetPieChart()
        {
            var paiChartObj = new List<String>();
            return Json(paiChartObj);
        }


        [HttpGet]
        public string InitDashboardData(string date)
        {
            _logger.LogInformation(date);
            SyncUserData model = _backgroundTaskQueue.getDashboardData();
            decimal[] todayDieselSales = _unitOfWork.Notification.todayDieselSales(date);
            //_logger.LogInformation(todayDieselSales[0] + "");
            //_logger.LogInformation(todayDeiselSales[1] + "");
            model.todayDieselSalesAmount = todayDieselSales[0];
            model.todayDieselSalesQty = todayDieselSales[1];
            model.todayDieselDaySalesLtr = Convert.ToDecimal(todayDieselSales[2]);
            model.todayDieselDaySalesAmount = Convert.ToDecimal(todayDieselSales[3]);
            model.todayDieselNightSalesLtr = Convert.ToDecimal(todayDieselSales[4]);
            model.todayDieselNightSalesAmount = Convert.ToDecimal(todayDieselSales[5]);

            decimal[] currentMonthDieselSales = _unitOfWork.Notification.currentMonthDieselSales(99);
            model.currentMonthDieselSalesAmount = currentMonthDieselSales[0];
            model.currentMonthDieselSalesQty = currentMonthDieselSales[1];

            decimal[] previousMonthDieselSales = _unitOfWork.Notification.previousMonthDieselSales(99);
            model.previousMonthDieselSalesAmount = previousMonthDieselSales[0];
            model.previousMonthDieselSalesQty = previousMonthDieselSales[1];

            decimal[] currentYearDieselSales = _unitOfWork.Notification.currentYearDieselSales(99);
            model.currentYearDieselSalesAmount = currentYearDieselSales[0];
            model.currentYearDieselSalesQty = currentYearDieselSales[1];

            decimal[] previousYearDieselSales = _unitOfWork.Notification.previousYearDieselSales(99);
            model.previousYearDieselSalesAmount = previousYearDieselSales[0];
            model.previousYearDieselSalesQty = previousYearDieselSales[1];

            object[] todaySales = _unitOfWork.Notification.todaySales(date);
            model.todaySalesAmount = (decimal)todaySales[0];
            model.todaySalesMemoQty = (int)todaySales[1];
            model.todayDaySalesLtr = Convert.ToDecimal(todaySales[2]);
            model.todayDaySalesAmount = Convert.ToDecimal(todaySales[3]);
            model.todayNightSalesLtr = Convert.ToDecimal(todaySales[4]);
            model.todayNightSalesAmount = Convert.ToDecimal(todaySales[5]);


            object[] todaySalesCollection = _unitOfWork.Notification.todaySalesCollection(date);
            model.todaySalesCollection = (decimal)todaySalesCollection[0];
            model.todaySalesCollectionQty = (int)todaySalesCollection[1];

            object[] todaySalesDue = _unitOfWork.Notification.todaySalesDue(date);
            model.todaySalesDue = (decimal)todaySalesDue[0];
            model.todaySalesDueQty = (int)todaySalesDue[1];

            decimal todayPurchase = _unitOfWork.Notification.todayPurchase(date);
            model.todayPurchase = todayPurchase;

            decimal todayExpanse = _unitOfWork.Notification.todayExpanse(date);
            model.todayExpance = todayExpanse;

            decimal todayCashBalance = _unitOfWork.Notification.todayCashBalance(date);
            model.todayCashBalance = todayCashBalance;

            decimal todayBankBalance = _unitOfWork.Notification.todayBankBalance(date);
            model.todayBankBalance = todayBankBalance;

            decimal bank = _unitOfWork.Notification.bank();
            model.totalBank = bank;

            decimal cash = _unitOfWork.Notification.cash();
            model.totalCash = cash;

            decimal income = _unitOfWork.Notification.income();
            model.totalIncome = income;

            decimal expanse = _unitOfWork.Notification.expanse();
            model.totalExpanse = expanse;

            model.totalProfit = model.totalIncome - model.totalExpanse;

            decimal dues = _unitOfWork.Notification.dues();
            model.totalDues = dues;

            object[] notCollection = _unitOfWork.Notification.panddingSales();
            model.notCollectionMemo = (int)notCollection[0];
            model.notCollectionAmount = (decimal)notCollection[1];
            model.notCollectionLtr = (decimal)notCollection[2];

            var readingCollection = _unitOfWork.Notification.readings().ToList();
            foreach (string obj in readingCollection)
            {
                dynamic jsonData = JObject.Parse(obj);
                //Console.WriteLine(jsonData["Gun"]);
                int gun = jsonData["Gun"];
                switch (gun)
                {
                    case 1:
                        model.Gun1 = jsonData["Reading"];
                        break;
                    case 2:
                        model.Gun2 = jsonData["Reading"];
                        break;
                    case 3:
                        model.Gun3 = jsonData["Reading"];
                        break;
                    case 4:
                        model.Gun4 = jsonData["Reading"];
                        break;
                    case 5:
                        model.Gun5 = jsonData["Reading"];
                        break;
                    case 6:
                        model.Gun6 = jsonData["Reading"];
                        break;
                    case 7:
                        model.Gun7 = jsonData["Reading"];
                        break;
                    case 8:
                        model.Gun8 = jsonData["Reading"];
                        break;
                    case 9:
                        model.Gun9 = jsonData["Reading"];
                        break;
                    case 10:
                        model.Gun10 = jsonData["Reading"];
                        break;

                }
                
            }
            //Console.WriteLine(model.Gun1);
            return JsonConvert.SerializeObject(model);
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
