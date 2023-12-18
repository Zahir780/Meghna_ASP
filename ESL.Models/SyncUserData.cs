
using System;
using System.Collections.Generic;
using System.Text;

namespace ESL.Models
{
   public class SyncUserData
    {
        public List<string> NotifiyUsers { get; set; }
        public List<string> OnlineUsers { get; set; }
        public ChatMessage ChatMessage { get; set; }
        public string NotifyMe { get; set; }

        public decimal Gun1 { get; set; }
        public decimal Gun2 { get; set; }
        public decimal Gun3 { get; set; }
        public decimal Gun4 { get; set; }
        public decimal Gun5 { get; set; }
        public decimal Gun6 { get; set; }
        public decimal Gun7 { get; set; }
        public decimal Gun8 { get; set; }
        public decimal Gun9 { get; set; }
        public decimal Gun10 { get; set; }

        public decimal todayDieselSalesAmount { get; set; }
        public decimal todayDieselSalesQty { get; set; }
        public decimal todayDieselDaySalesAmount { get; set; }
        public decimal todayDieselNightSalesAmount { get; set; }
        public decimal todayDieselDaySalesLtr { get; set; }
        public decimal todayDieselNightSalesLtr { get; set; }
        public decimal currentMonthDieselSalesAmount { get; set; }
        public decimal currentMonthDieselSalesQty { get; set; }
        public decimal previousMonthDieselSalesAmount { get; set; }
        public decimal previousMonthDieselSalesQty { get; set; }
        public decimal currentYearDieselSalesAmount { get; set; }
        public decimal currentYearDieselSalesQty { get; set; }
        public decimal previousYearDieselSalesAmount { get; set; }
        public decimal previousYearDieselSalesQty { get; set; }
        public decimal todaySalesAmount { get; set; }
        public int todaySalesMemoQty { get; set; }
        public decimal todayDaySalesAmount { get; set; }
        public decimal todayNightSalesAmount { get; set; }
        public decimal todayDaySalesLtr { get; set; }
        public decimal todayNightSalesLtr { get; set; }
        public decimal todaySalesCollection { get; set; }
        public int todaySalesCollectionQty { get; set; }
        public decimal todaySalesDue { get; set; }
        public int todaySalesDueQty { get; set; }
        public decimal todayPurchase { get; set; }
        public decimal todayExpance { get; set; }
        public decimal todayCashBalance { get; set; }
        public decimal todayBankBalance { get; set; }
        public decimal totalIncome { get; set; }
        public decimal totalCash { get; set; }
        public decimal totalBank { get; set; }
        public decimal totalExpanse { get; set; }
        public decimal totalProfit { get; set; }
        public decimal totalDues { get; set; }
        public int notCollectionMemo { get; set; }
        public decimal notCollectionLtr { get; set; }
        public decimal notCollectionAmount { get; set; }
    }
}
