using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Globalization;

namespace Report.Library
{
    public class DateHelper
    {
        /// <summary>
        /// 取得一年中某一周的開始日期和結束日期
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="weekNum">周</param>
        /// <param name="weekFirstDay">設定一周起始日</param>
        /// <param name="weekStart">開始日期</param>
        /// <param name="weekeEnd">結束日期</param>
        public static void GetWeekDate(int year, int weekNum, DayOfWeek weekFirstDay, out   DateTime weekStart, out   DateTime weekeEnd)
        {
            DateTime dt = new DateTime(year, 1, 1);
            dt += new TimeSpan((weekNum - 1) * 7, 0, 0, 0);
            weekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)weekFirstDay);
            weekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
        }

        /// <summary>
        /// 獲取一年中指定的一周的開始日期和結束日期。開始日期遵循ISO 8601即星期一。
        /// </summary>
        /// <remarks>Write by vrhero</remarks>
        /// <param name="year">年（1 到 9999）</param>
        /// <param name="weeks">周（1 到 53）</param>
        /// <param name="weekrule">確定首周的規則</param>
        /// <param name="first">當此方法返回時，則包含參數 year 和 weeks 指定的周的開始日期的 System.DateTime 值；如果失敗，則為 System.DateTime.MinValue。如果參數 year 或 weeks 超出有效範圍，則操作失敗。該參數未經初始化即被傳遞。</param>
        /// <param name="last">當此方法返回時，則包含參數 year 和 weeks 指定的周的結束日期的 System.DateTime 值；如果失敗，則為 System.DateTime.MinValue。如果參數 year 或 weeks 超出有效範圍，則操作失敗。該參數未經初始化即被傳遞。</param>
        /// <returns>成功返回 true，否則為 false。</returns>
        public static bool GetDaysOfWeeks(int year, int weeks, CalendarWeekRule weekrule, out DateTime first, out DateTime last)
        {
            //初始化 out 參數
            first = DateTime.MinValue;
            last = DateTime.MinValue;
            if (year < 1 | year > 9999)
                return false;
            //一年最多53周
            if (weeks < 1 | weeks > 53)
                return false;
            //取當年首日為基準
            DateTime firstCurr = new DateTime(year, 1, 1);
            //取下一年首日用於計算
            DateTime firstNext = new DateTime(year + 1, 1, 1);
            //將當年首日星期幾轉換為數位星期日特別處理ISO 8601 標準
            int dayOfWeekFirst = (int)firstCurr.DayOfWeek;
            if (dayOfWeekFirst == 0) dayOfWeekFirst = 7;
            //得到未經驗證的周首日
            first = firstCurr.AddDays((weeks - 1) * 7 - dayOfWeekFirst + 1);
            //周首日是上一年日期的情況
            if (first.Year < year)
            {
                switch (weekrule)
                {
                    case CalendarWeekRule.FirstDay:
                        first = firstCurr;
                        break;
                    case CalendarWeekRule.FirstFullWeek:
                        //順延一周
                        first = first.AddDays(7);
                        break;
                    case CalendarWeekRule.FirstFourDayWeek:
                        //周首日距年首日不足4天則順延一周
                        if (firstCurr.Subtract(first).Days > 3)
                        {
                            first = first.AddDays(7);
                        }
                        break;
                    default:
                        break;
                }
            }
            //得到未經驗證的週末日
            last = first.AddDays(7).AddSeconds(-1);
            //週末日是下一年日期的情況
            if (last.Year > year)
            {
                switch (weekrule)
                {
                    case CalendarWeekRule.FirstDay:
                        last = firstNext.AddSeconds(-1);
                        break;
                    case CalendarWeekRule.FirstFullWeek:
                        //不用處理
                        break;
                    case CalendarWeekRule.FirstFourDayWeek:
                        //週末日距下一年首日不足4天則提前一周
                        if (firstNext.Subtract(first).Days < 4)
                        {
                            first = first.AddDays(-7);
                            last = last.AddDays(-7);
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }



        /// <summary>
        /// 取得當前天在一年中的第幾周
        /// </summary>
        /// <param name="curDay"></param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime curDay)
        {
            int firstdayofweek = Convert.ToInt32(Convert.ToDateTime(curDay.Year.ToString() + "- " + "1-1 ").DayOfWeek);

            int days = curDay.DayOfYear;
            int daysOutOneWeek = days - (7 - firstdayofweek);

            if (daysOutOneWeek <= 0)
            {
                return 1;
            }
            else
            {
                int weeks = daysOutOneWeek / 7;
                if (daysOutOneWeek % 7 != 0)
                    weeks++;

                return weeks + 1;
            }
        }


        public static int NumberOfDays(int year, int month)
        {
            int[] numDays = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            int n = numDays[month - 1];

            if (month == 2 && year % 4 == 0)
            {
                ++n;
            }
            return n;
        }

    }
}
