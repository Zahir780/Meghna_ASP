using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ESL.Services
{
    public class BankService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        string sqlCon;


        public BankService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _db = db;
            sqlCon = _db.Database.GetDbConnection().ConnectionString;
        }



    }
}
