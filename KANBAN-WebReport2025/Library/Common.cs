using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebReport.Library
{
    public class MyCommon
    {
        #region constructor
        /// <summary>
        /// 建構式
        /// </summary>
        /// 
        public MyCommon()
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region methods

        public static void GetTime(out string BeginTime, out string EndTime)
        {

            if (DateTime.Now.Hour >= 7 && DateTime.Now.Hour < 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 07:00";
                EndTime = DateTime.Now.ToShortDateString() + " 19:00";
            }
            else if (DateTime.Now.Hour >= 19)
            {
                BeginTime = DateTime.Now.ToShortDateString() + " 19:00";
                EndTime = DateTime.Now.AddDays(1).ToShortDateString() + " 07:00";
            }
            else
            {
                BeginTime = DateTime.Now.AddDays(-1).ToShortDateString() + " 19:00";
                EndTime = DateTime.Now.ToShortDateString() + " 07:00";
            }

        }

        #endregion
    }
}