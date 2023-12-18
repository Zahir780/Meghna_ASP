using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ESL.Utility
{
    public static class SD
    {
        
        public const string Proc_CoverType_Create = "usp_CreateCoverType";
        public const string Proc_CoverType_Get = "usp_GetCoverType";
        public const string Proc_CoverType_GetAll = "usp_GetCoverTypes";
        public const string Proc_CoverType_Update = "usp_UpdateCoverType";
        public const string Proc_CoverType_Delete = "usp_DeleteCoverType";

        public const string Role_User_Indi = "Reader";
        public const string Role_Admin = "Admin";
        public const string Role_User = "User";

        public const string pathDir = @"C:\eslCon";
        public const string fileName = "eslcon.txt";

        /*Common Message*/

        public const string errorMessage = "Unable to save!";
        public const string successSaveMessage = "Information save successfully!";
        public const string successEditMessage = "Information update successfully!";
        public const string successDeleteMessage = "Information delete successfully!";

        /*Common Message*/


        public static string getIp()
        {
            var addlist = Dns.GetHostEntry(Dns.GetHostName());
            string GetHostName = addlist.HostName.ToString();
            string GetIPV6 = addlist.AddressList[0].ToString();
            string GetIPV4 = addlist.AddressList[1].ToString();

            return GetIPV4;
        }
        public static string decFTwo(object amount)
        {
            return String.Format("{0:N}", Convert.ToDecimal(amount));
        }
        public static string decFZero(object amount)
        {
            return String.Format("{0:n0}", Convert.ToDecimal(amount));
        }

        public static DateTime stringToDate(string date)
       {
           return Convert.ToDateTime(date);
       }

        public static string getCurrentTime()
        {
            return DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss tt");
        }
        public static string dateToString(DateTime date, string format)
        {
            return date.ToString(format);
        }

        public static DateTime AddDay(DateTime date,int day)
        {
            return date.AddDays(day);
        }
        public static DateTime SubDay(DateTime date, int day)
        {
            return date.AddDays(-day);
        }
        public static DateTime AddMonth(DateTime date, int month)
        {
            return date.AddMonths(month);
        }
        public static DateTime SubMonth(DateTime date, int month)
        {
            return date.AddMonths(-month);
        }
        public static DateTime AddYear(DateTime date, int year)
        {
            return date.AddYears(year);
        }
        public static DateTime SubYear(DateTime date, int year)
        {
            return date.AddYears(-year);
        }
        public static string getDbToBd(string date)
        {
            string[] d = date.Split("-");
            return d[2] + '-' + d[1] + '-' + d[0];
        }
        public static string getCustomDigit(object value, int count,string prifix)
        {
            var values = value.ToString();
            if(values.Length>=count)
                return values;
            else
            {
                int digitbalancesize = count - values.Length;
                
                for(int i = 0; i < digitbalancesize; i++)
                {
                    values = prifix + "" + values;
                }
            }
            return values;
        }
        public static string getMonthName(int month)
        {
            string ret = "";
            switch(month)
            {
                case 1:ret = "January";break;
                case 2:ret = "February";break;
                case 3:ret = "March";break;
                case 4:ret = "April";break;
                case 5:ret = "May";break;
                case 6:ret = "June";break;
                case 7:ret = "July";break;
                case 8:ret = "August";break;
                case 9:ret = "September";break;
                case 10:ret = "October";break;
                case 11:ret = "November";break;
                case 12:ret = "December";break;
                default:break;
            }
            return ret;
        }

        public static DateTime shiftStartDate(DateTime date)
        {
            DateTime daystart = DateTime.Now;
            TimeSpan ts = new TimeSpan(8, 0, 0);
            daystart = daystart.Date + ts;
            if (date.Hour < daystart.Hour)
            {
                daystart = date.AddDays(-1);
                daystart = daystart.Date + ts;
            }
            Console.WriteLine(daystart);
            return daystart;
        }

        public static DateTime shiftEndDate(DateTime date)
        {
            DateTime dayend = DateTime.Now;
            TimeSpan ts = new TimeSpan(8, 0, 0);
            dayend = dayend.Date + ts;
            if (date.Hour >= dayend.Hour)
            {
                dayend = date.AddDays(1);
                dayend = dayend.Date + ts;
            }
            Console.WriteLine(dayend);
            return dayend;
        }

        public static DateTime FirstDayOfMonth_AddMethod(this DateTime value)
        {
            return value.Date.AddDays(1 - value.Day);
        }

        public static DateTime FirstDayOfMonth_NewMethod(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        public static DateTime LastDayOfMonth_AddMethod(this DateTime value)
        {
            return value.FirstDayOfMonth_AddMethod().AddMonths(1).AddDays(-1);
        }

        public static DateTime LastDayOfMonth_AddMethodWithDaysInMonth(this DateTime value)
        {
            return value.Date.AddDays(DateTime.DaysInMonth(value.Year, value.Month) - value.Day);
        }

        public static DateTime LastDayOfMonth_SpecialCase(this DateTime value)
        {
            return value.AddDays(DateTime.DaysInMonth(value.Year, value.Month) - 1);
        }

        public static int DaysInMonth(this DateTime value)
        {
            return DateTime.DaysInMonth(value.Year, value.Month);
        }

        public static DateTime LastDayOfMonth_NewMethod(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        public static DateTime LastDayOfMonth_NewMethodWithReuseOfExtMethod(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.DaysInMonth());
        }


        public static void FileRemove()
        {
            try
            {
                /*var addlist = Dns.GetHostEntry(Dns.GetHostName());
                string hostName = addlist.HostName.ToString();
                Console.WriteLine(@"\\" + hostName + "\\ESL SOFT\\A");
                DirectoryInfo dir = new DirectoryInfo(@"\\" + hostName + "\\ESL SOFT\\A");*/
                DirectoryInfo dir = new DirectoryInfo(@"D:\scanar");
                foreach (FileInfo fi in dir.GetFiles())
                {
                    fi.Delete();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static bool isEmpty(object obj)
        {
            try
            {
                if (obj == null || obj.ToString() == "")
                {
                    return true;
                }
                decimal amt = Convert.ToDecimal(obj.ToString());
                if (amt <= 0)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

    }
}
