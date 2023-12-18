////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: IRepository.cs
//FileType: Visual C# Source file
//Author : Zahid Sarmon
//Created On : 02-11-2020
//Copy Rights : Evision Soft. Ltd.
//Description : Class for Repository
////////////////////////////////////////////////////////////////////////////////////////////////////////
using ESL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
namespace ESL.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T :class
    {
        T Get(int id);

        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );

        T GetFirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );

        T Add(T entity);
        IEnumerable<T> AddRange(IEnumerable<T> entity);

        T Update(T entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

        //Modified by asad 13-11-2020
        int Max(string table,string column);

        //jewel 29-10-2020
        int NumericMax(string table, string column);

        object EmployeeMax(string table, string column);

        //Modified by asad 7-01-2021
        int SalesMax(string table, string column);

        List<Object[]> BarChartCurrentMonthProjectData(Boolean isAll);
        List<Object[]> BarChartEachMonthProjectData();
        List<Object[]> PieChartEachMonthEachProjectData();

        List<Object[]> DonutChartProjectEmployeeData();
        List<ChatMessage> GetAllMyChattingUsers(string userId);
        decimal[] todayDieselSales(string date);
        decimal[] currentMonthDieselSales(int itemId);
        decimal[] previousMonthDieselSales(int itemId);
        decimal[] currentYearDieselSales(int itemId);
        decimal[] previousYearDieselSales(int itemId);

        object[] todaySales(string date);
        object[] todaySalesCollection(string date);
        object[] todaySalesDue(string date);
        decimal todayPurchase(string date);
        decimal todayExpanse(string date);
        decimal todayCashBalance(string date);
        decimal todayBankBalance(string date);
        decimal income();
        decimal cash();
        decimal bank();
        decimal expanse();
        decimal profit();
        decimal dues();
        object[] panddingSales();
        IEnumerable<string> readings();
    }
}
