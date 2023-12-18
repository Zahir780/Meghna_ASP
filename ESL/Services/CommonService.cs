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
using System.Security.Claims;
using System.Threading.Tasks;

namespace ESL.Services
{
    public class CommonService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IHttpContextAccessor _accessor;
        private ApplicationDbContext _db;
        string sqlCon;
        public CommonService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _accessor = accessor;
            _db = db;
            sqlCon= _db.Database.GetDbConnection().ConnectionString;
        }
        public string getUserId()
        {
            return _accessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
        }

        public string fiscalYearIdTrans(string voucherDate)
        {
            SqlConnection con = new SqlConnection(sqlCon);


            string sql = "Select vFiscalYearIdTrans from [dbo].[funFiscalYearInfo]('" + voucherDate + "',"
                     + " '" + voucherDate + "', '" +_accessor.HttpContext.Session.GetString("companyName") + "')";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["vFiscalYearIdTrans"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "0";
        }




        public string getMaxCode(string column, string table)
        {
            if (NumericCheck(column, table) > 0)
            {
                return NumericMax(column, table).ToString();
            }
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd;
            string ret = "";
            string sql = "select CONVERT(varchar,ISNULL(MAX(CAST(SUBSTRING(" + column + "," +
                " PATINDEX('%[0-9]%'," + column + ")," +
                " LEN(" + column + ")) as bigint)),0)+1) post from " + table + "";
            try
            {
                con.Open();
                cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        ret = reader["post"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return codePrefix(column, table) + ret;
        }

        public int NumericCheck(string column, string table)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd;
            int ret = 0;
            string sqlCheck = "select distinct ISNUMERIC(" + column + ") numb from " + table + " ";
            try
            {
                con.Open();
                cmd = new SqlCommand(sqlCheck, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        ret = Convert.ToInt32(reader["numb"]);
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        public int NumericMax(string column, string table)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            SqlCommand cmd;
            int ret = 0;
            string sqlCheck = "select MAX(CAST(" + column + " as bigint))+1 max from " + table + " ";
            try
            {
                con.Open();
                cmd = new SqlCommand(sqlCheck, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        ret = Convert.ToInt32(reader["max"]);
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return ret;
        }
        private string codePrefix(string column, string table)
        {
            SqlConnection con = new SqlConnection(sqlCon);
            string sql = "select distinct SUBSTRING(" + column + ",0,PATINDEX('%[0-9]%'," + column + ")) pre from " + table + "";

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    if (reader.Read())
                    {
                        return reader["pre"].ToString();
                    }
                }
            }
            finally
            {
                con.Close();
            }
            return "";
        }



    }
}
