////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: IRepository.cs
//FileType: Visual C# Source file
//Author : Zahid Sarmon
//Created On : 02-11-2020
//Copy Rights : Evision Soft. Ltd.
//Description : Class for Repository
////////////////////////////////////////////////////////////////////////////////////////////////////////

using ESL.DataAccess.Data;
using ESL.DataAccess.Repository.IRepository;
using ESL.Models;
using ESL.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ESL.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public T Add(T entity)
        {
            _db.Entry<T>(entity).State = EntityState.Detached;
            return dbSet.Add(entity).Entity;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> entity)
        {
            List<T> des = new List<T>();
            foreach (var item in entity)
            {
                des.Add(item);
            }
            dbSet.AddRange(des);
            return des;
        }
        public T Update(T entity)
        {
            _db.Entry<T>(entity).State = EntityState.Detached;
            return dbSet.Update(entity).Entity;
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            
            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            Remove(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }

        public int Max(string table,string column)
        {
            int ret = 0;
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select ISNULL(Max(cast(SUBSTRING(" + column+", 5,LEN("+ column + "))as int))+1,1) from "+table+" ";
                _db.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    // do something with result
                    if (!result.Read())
                    {
                        ret= 1;
                    }
                    else {
                        ret= (int)result.GetValue(0);
                    }
                }
                _db.Database.CloseConnection();
            }
            return ret;

        }

        public int NumericMax(string table, string column)
        {
            int ret = 0;
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select ISNULL(Max(cast(SUBSTRING(" + column + ", 1,LEN(" + column + "))as int))+1,1) from " + table + " ";
                _db.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    // do something with result
                    if (!result.Read())
                    {
                        ret = 1;
                    }
                    else
                    {
                        ret = (int)result.GetValue(0);
                    }
                }
                _db.Database.CloseConnection();
            }
            return ret;

        }

        public int SalesMax(string table, string column)
        {
            int ret = 0;
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select Max(" + column + ") from " + table + " ";
                _db.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    // do something with result
                    if (!result.Read())
                    {
                        ret =0;
                    }
                    else
                    {
                        ret = (int)result.GetValue(0);
                    }
                }
                _db.Database.CloseConnection();
            }
            return ret;

        }


        public object EmployeeMax(string table, string column)
        {
            object ret = 0;
            using (var command = _db.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = "select right('0000' + convert(varchar(10), ISNULL(Max(cast(SUBSTRING("+ column + ", 5,LEN("+ column + "))as int))+1,1)), 5) from " + table + " ";
                _db.Database.OpenConnection();
                using (var result = command.ExecuteReader())
                {
                    // do something with result
                    if (!result.Read())
                    {
                        ret= 1;
                    }
                    else
                    {
                        ret= result.GetValue(0);
                    }
                }
                _db.Database.CloseConnection();
            }

            return ret;

        }

        public List<Object[]> BarChartCurrentMonthProjectData(Boolean isAll)
        {
            string all = "left join";
            if (!isAll)
                all = "right join";

            List<Object[]> list = new List<Object[]>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select projectName,totalemployee,COALESCE(totalverified,0) as totalverified,COALESCE(month,CONVERT(CHAR(3),DATENAME(MONTH, CURRENT_TIMESTAMP))) as month  from " +
                        "(select e.projectId, p.Name as projectName, count(e.ProjectId) as totalemployee from tbEmployeeInfo as e " +
                        "join tbProjectInfo as p " +
                        "on e.ProjectId = p.Id " +
                        "group by e.ProjectId, p.Name) as ep " +all+
                        " (select COUNT(a.ProjectId) as totalverified,CONVERT(CHAR(3), DATENAME(MONTH, v.date)) as month,a.ProjectId from tbEmployeeVerifie as v " +
                        "join tbEmployeeInfo as a " +
                        "on v.employeeId = a.id " +
                        "join tbProjectInfo as p " +
                        "on a.ProjectId = p.Id " +
                        "where YEAR(v.date) = YEAR(CURRENT_TIMESTAMP) and MONTH(v.date) = MONTH(CURRENT_TIMESTAMP) " +
                        "group by CONVERT(CHAR(3), DATENAME(MONTH, v.date)),a.ProjectId) as ev " +
                        "on ep.projectId = ev.ProjectId ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            object[] obj = new object[4];
                            obj[0] = result.GetValue(0);
                            obj[1] = result.GetValue(1);
                            obj[2] = result.GetValue(2);
                            obj[3] = result.GetValue(3);
                            list.Add(obj);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return list;
        }

       

        public List<Object[]> BarChartEachMonthProjectData()
        {

            List<Object[]> list = new List<Object[]>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select CONVERT(CHAR(3), DATENAME(MONTH, date)),COUNT(id),MONTH(date) from tbEmployeeVerifie " +
                        "where YEAR(date) = YEAR(CURRENT_TIMESTAMP) " +
                        "group by CONVERT(CHAR(3), DATENAME(MONTH, date)),MONTH(date) ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            object[] obj = new object[2];
                            obj[0] = result.GetValue(0);
                            obj[1] = result.GetValue(1);
                            list.Add(obj);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }
            
            return list;
        }

        public List<Object[]> PieChartEachMonthEachProjectData()
        {
            List<Object[]> list = new List<Object[]>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select COUNT(a.ProjectId),p.Name,CONVERT(CHAR(3), DATENAME(MONTH, v.date)) from tbEmployeeVerifie as v " +
                        "join tbEmployeeInfo as a " +
                        "on v.employeeId = a.id " +
                        "join tbProjectInfo as p " +
                        "on a.ProjectId = p.Id " +
                        "where YEAR(v.date) = YEAR(CURRENT_TIMESTAMP) and MONTH(v.date) = MONTH(CURRENT_TIMESTAMP) " +
                        "group by CONVERT(CHAR(3), DATENAME(MONTH, v.date)),a.ProjectId,p.Name ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            object[] obj = new object[3];
                            obj[0] = result.GetValue(0);
                            obj[1] = result.GetValue(1);
                            obj[2] = result.GetValue(2);
                            list.Add(obj);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return list;
        }

        public List<Object[]> DonutChartProjectEmployeeData()
        {
            List<Object[]> list = new List<Object[]>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select e.projectId, p.Name as projectName, count(e.ProjectId) as totalemployee from tbEmployeeInfo as e " +
                        "join tbProjectInfo as p " +
                        "on e.ProjectId = p.Id " +
                        "group by e.ProjectId,p.Name ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            object[] obj = new object[2];
                            obj[0] = result.GetValue(1);
                            obj[1] = result.GetValue(2);
                            list.Add(obj);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return list;
        }

        public List<ChatMessage> GetAllMyChattingUsers(string userId)
        {


            List<ChatMessage> list = new List<ChatMessage>();

            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select c.*," +
                        "(select Text from tbChatMessage as t where t.Id = c.id) as Text," +
                        "(select IsRead from tbChatMessage as t where t.Id = c.id) as isRead from " +
                        "(select Max(Id) as id, SenderId, SenderName, SenderPic from tbChatMessage " +
                        "where ReceiverId = '" + userId + "' " +
                        "Group by SenderId, SenderName, SenderPic) as c order by id desc  ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            ChatMessage chat = new ChatMessage();
                            chat.Id = (int)result.GetValue(0);
                            chat.SenderId = (string)result.GetValue(1);
                            chat.SenderName = (string)result.GetValue(2);
                            chat.SenderPic = (string)result.GetValue(3);
                            chat.Text = (string)result.GetValue(4);
                            chat.IsRead = (bool)result.GetValue(5);
                            list.Add(chat);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }
            return list;
        }

        public decimal[] todayDieselSales(string date)
        {
            decimal[] obj = new decimal[6];
            obj[0] = 0;
            obj[1] = 0;
            obj[2] = 0;
            obj[3] = 0;
            obj[4] = 0;
            obj[5] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    DateTime todate = SD.stringToDate(date);
                    var startDate = SD.dateToString(SD.shiftStartDate(todate), "yyyy-MM-dd");
                    var endDate = SD.dateToString(SD.shiftEndDate(todate), "yyyy-MM-dd");
                    _db.Database.OpenConnection();
                    string sql =  "select ISNULL(SUM(d.TotalPrice),0) amount, ISNULL(SUM(d.Qty),0) qty from tbSales as s " +
                        "left join tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where ItemId = 99 AND s.SellDateTime between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59' and s.UploadFlag=1 ";
                    command.CommandText = sql;
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                           obj[0] = (decimal)result.GetValue(0);
                           obj[1] = (decimal)result.GetValue(1);
                        }
                    }

                    command.CommandText = "select m.shifting, " +
                       "ISNULL(SUM(m.Qty),0) Qty, ISNULL(SUM(m.TotalPrice),0) Amount from " +
                       "(select(Select[dbo].[Shifting] (s.SellDateTime)) as shifting," +
                       "(Select[dbo].[ShiftingDate](s.SellDateTime)) SellDateTime, d.Qty, " +
                       "d.TotalPrice from tbSales as s " +
                       "inner join " +
                       "tbSalesDetails as d " +
                       "on s.Id = d.SalesId " +
                       "where UploadFlag = 1 and d.ItemId = 99 and " +
                       "SellDateTime between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59') as m " +
                       "group by m.SellDateTime,m.shifting";

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        int i = 2;
                        while (result.Read())
                        {
                            obj[i] = (decimal)result.GetValue(1);
                            i++;
                            obj[i] = (decimal)result.GetValue(2);
                            i++;
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal[] currentMonthDieselSales(int itemId)
        {
            decimal[] obj = new decimal[2];
            obj[0] = 0;
            obj[1] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(SUM(d.TotalPrice),0) amount, ISNULL(SUM(d.Qty), 0) qty from tbSales as s " +
                        "left join tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where ItemId =" + itemId + " AND DATENAME(yyyy,s.SellDateTime)+'-'+DATENAME(MM,s.SellDateTime) = DATENAME(yyyy,CURRENT_TIMESTAMP)+'-'+DATENAME(MM,CURRENT_TIMESTAMP) and s.UploadFlag=1 ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (decimal)result.GetValue(1);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal[] previousMonthDieselSales(int itemId)
        {
            decimal[] obj = new decimal[2];
            obj[0] = 0;
            obj[1] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(SUM(d.TotalPrice),0) amount, ISNULL(SUM(d.Qty), 0) qty from tbSales as s " +
                        "left join tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where ItemId =" + itemId + " AND DATENAME(yyyy,s.SellDateTime)+'-'+DATENAME(MM,s.SellDateTime) = DATENAME(yyyy,DATEADD(month,-1,CURRENT_TIMESTAMP))+'-'+DATENAME(MM,DATEADD(month,-1,CURRENT_TIMESTAMP)) and s.UploadFlag=1 ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (decimal)result.GetValue(1);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal[] currentYearDieselSales(int itemId)
        {
            decimal[] obj = new decimal[2];
            obj[0] = 0;
            obj[1] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(SUM(d.TotalPrice),0) amount, ISNULL(SUM(d.Qty), 0) qty from tbSales as s " +
                        "left join tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where ItemId =" + itemId + " AND DATENAME(yyyy,s.SellDateTime) = DATENAME(yyyy,CURRENT_TIMESTAMP) and s.UploadFlag=1";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (decimal)result.GetValue(1);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal[] previousYearDieselSales(int itemId)
        {
            decimal[] obj = new decimal[2];
            obj[0] = 0;
            obj[1] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(SUM(d.TotalPrice),0) amount, ISNULL(SUM(d.Qty), 0) qty from tbSales as s " +
                        "left join tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where ItemId =" + itemId + " AND DATENAME(yyyy,s.SellDateTime) = DATENAME(yyyy,DATEADD(year,-1,CURRENT_TIMESTAMP)) and s.UploadFlag=1 ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (decimal)result.GetValue(1);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public object[] todaySales(string date)
        {
            object[] obj = new object[6];
            obj[0] = 0;
            obj[1] = 0;
            obj[2] = 0;
            obj[3] = 0;
            obj[4] = 0;
            obj[5] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    DateTime todate = SD.stringToDate(date);
                    var startDate = SD.dateToString(SD.shiftStartDate(todate), "yyyy-MM-dd");
                    var endDate = SD.dateToString(SD.shiftEndDate(todate), "yyyy-MM-dd");
                    command.CommandText = "select ISNULL(SUM(Amount),0.0) Amount, ISNULL(COUNT(id),0) MR from tbSales where SellDateTime between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59' and UploadFlag=1 ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (int)result.GetValue(1);
                        }
                    }
                    //_db.Database.CloseConnection();

                    command.CommandText = "select m.shifting, " +
                        "ISNULL(SUM(m.Qty),0) Qty, ISNULL(SUM(m.Amount),0) Amount from " +
                        "(select(Select[dbo].[Shifting] (s.SellDateTime)) as shifting," +
                        "(Select[dbo].[ShiftingDate](s.SellDateTime)) SellDateTime, d.Qty, " +
                        "s.Amount from tbSales as s " +
                        "inner join " +
                        "tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where UploadFlag = 1 and d.ItemId = 99 and " +
                        "SellDateTime between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59') as m " +
                        "group by m.SellDateTime,m.shifting";
                   
                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        int i = 2;
                        while (result.Read())
                        {
                            obj[i] = (decimal)result.GetValue(1);
                            i++;
                            obj[i] = (decimal)result.GetValue(2);
                            i++;
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }
        public object[] todaySalesCollection(string date)
        {
            object[] obj = new object[2];
            obj[0] = 0;
            obj[1] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    DateTime todate = SD.stringToDate(date);
                    var startDate = SD.dateToString(SD.shiftStartDate(todate), "yyyy-MM-dd");
                    var endDate = SD.dateToString(SD.shiftEndDate(todate), "yyyy-MM-dd");
                    command.CommandText = "select ISNULL(SUM(PaidAmount),0) Amount, ISNULL(COUNT(id),0) MR from tbSales where SellDateTime between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59' and UploadFlag=1 and PaidAmount>0 ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (int)result.GetValue(1);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public object[] todaySalesDue(string date)
        {
            object[] obj = new object[2];
            obj[0] = 0;
            obj[1] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    DateTime todate = SD.stringToDate(date);
                    var startDate = SD.dateToString(SD.shiftStartDate(todate), "yyyy-MM-dd");
                    var endDate = SD.dateToString(SD.shiftEndDate(todate), "yyyy-MM-dd");
                    command.CommandText = "select ISNULL(SUM(Amount-PaidAmount),0) Amount, ISNULL(COUNT(id),0) mr from tbSales where SellDateTime between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59' and UploadFlag=1 and PaymentType='Credit' ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (decimal)result.GetValue(0);
                            obj[1] = (int)result.GetValue(1);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal todayPurchase(string date)
        {
            return 0;
        }

        public decimal todayExpanse(string date)
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(SUM(DrAmount),0) DrAmount from tbVoucher " +
                        "where convert(date, VoucherDate) = '"+ date + "' " +
                        "and EntryFrom != 'Sales Entry' " +
                        "and VoucherType in ('dca', 'dba') and DrAmount > 0  ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal todayCashBalance(string date)
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    DateTime todate = SD.stringToDate(date);
                    var startDate = SD.dateToString(SD.shiftStartDate(todate), "yyyy-MM-dd");
                    var endDate = SD.dateToString(SD.shiftEndDate(todate), "yyyy-MM-dd");
                    command.CommandText = "select ISNULL(sum(DrAmount-CrAmount),0) amount from  " +
                        "tbVoucher where VoucherDate between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59' and LedgerId='AL1'  ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal todayBankBalance(string date)
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    DateTime todate = SD.stringToDate(date);
                    var startDate = SD.dateToString(SD.shiftStartDate(todate), "yyyy-MM-dd");
                    var endDate = SD.dateToString(SD.shiftEndDate(todate), "yyyy-MM-dd");
                    command.CommandText = "select ISNULL(sum(DrAmount-CrAmount),0) amount from  " +
                        "tbVoucher where VoucherDate between '" + startDate + " 08:00:00' and '" + endDate + " 07:59:59' and LedgerId='AL11'  ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal cash()
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select dbo.openingBal('AL1', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'C', 11) ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal bank()
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select dbo.openingBal('AL11', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'C', 11) ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal income()
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select  ISNULL(sum(CrAmount),0) cramount from tbVoucher where LedgerId in (select LedgerId from tbLedger where Type = 'Income') ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }
        public decimal expanse()
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(sum(DrAmount),0) from tbVoucher where LedgerId in (select LedgerId from tbLedger where Type = 'Expense') ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public decimal profit()
        {
            return 0;
        }

        public decimal dues()
        {
            decimal obj = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select ISNULL(sum(DrAmount-CrAmount),0) Dues from  " +
                        "tbVoucher where LedgerId in " +
                        "(select LedgerId from tbLedger where CreateFrom in " +
                        "(select PrimaryGroupId from tbPrimaryGroup where Name like 'BILL RECEIVABLE'))";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj = (decimal)result.GetValue(0);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public object[] panddingSales()
        {
            object[] obj = new object[3];
            obj[0] = 0;
            obj[1] = 0;
            obj[2] = 0;
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select COUNT(SellId) memo, ISNULL(SUM(d.TotalPrice),0) amount, ISNULL(SUM(d.Qty), 0) qty from tbSales as s " +
                        "inner join tbSalesDetails as d " +
                        "on s.Id = d.SalesId " +
                        "where UploadFlag = 0 ";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                            obj[0] = (int)result.GetValue(0);
                            obj[1] = (decimal)result.GetValue(1);
                            obj[2] = (decimal)result.GetValue(2);
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return obj;
        }

        public IEnumerable<string> readings()
        {
            List<string> returnData = new List<string>();
            try
            {
                using (var command = _db.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "select GunId, MAX(TotalQty) Reading from tbSales as s " +
                        "inner join tbSalesDetails as d " +
                        "on s.Id = d.SalesId where GunId!= 0 " +
                        "group by GunId order by GunId";
                    _db.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        // do something with result
                        while (result.Read())
                        {
                           
                            returnData.Add(@"{'Gun':"+result["GunId"]+ ",'Reading':" + result["Reading"] + "}");
                        }
                    }
                    _db.Database.CloseConnection();

                }
            }
            catch (Exception) { }

            return returnData;
        }
    }
}
