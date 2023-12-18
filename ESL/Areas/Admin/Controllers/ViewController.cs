using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ESL.DataAccess;
using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Models.ViewModels;
using ESL.TreeMenu;
using ESL.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ViewController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<ViewController> _logger;
        string sqlCon;
        private ApplicationDbContext _db;
        public ViewController
        (
            UserManager<IdentityUser> userManager,
            ILogger<ViewController> logger,
            ApplicationDbContext db
        )
        {
            _userManager = userManager;
            _logger = logger;
            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }

       

        [HttpGet]
        public IActionResult ViewBoard(int id)
        {
            _logger.LogInformation(id.ToString());
            string attachment="";
            DateTime date=new DateTime();
            DateTime entryDate = new DateTime();
            string user="";
            SqlConnection con = new SqlConnection(sqlCon);
            try
            {
                con.Open();
                string sql = "select AttachBill,VoucherDate,UserName,EntryTime from tbVoucher where id=" + id;
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader sqlData = cmd.ExecuteReader();
                if (sqlData.Read())
                {
                    if (sqlData.HasRows)
                    {
                        attachment = sqlData["AttachBill"].ToString();
                        date = (DateTime)sqlData["VoucherDate"];
                        entryDate = (DateTime)sqlData["EntryTime"];
                        user = sqlData["UserName"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }



            // string[] arr = attachment.Split("/");
            ViewBag.Url = attachment;
            ViewBag.Heading = "";
            ViewBag.Date = date.ToString("dd-MM-yyyy hh:mm");
            ViewBag.EntryDate = entryDate.ToString("dd-MM-yyyy hh:mm");
            ViewBag.Details = "";//arr[arr.Length-1];
            ViewBag.UserName = user;
            ViewBag.Designation = "";
            return PartialView("_AttachmentPertialView");
           
           
        }
    }
}