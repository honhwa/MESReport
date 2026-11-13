using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Report.Sources
{
    public class LCM2PerformanceGraph1
    {
        #region Parameters
        private int down = 0;
        private int idle = 0;
        private int wait = 0;
        private int setup = 0;
        private int run = 0;
        private int eng = 0;
        private int other = 0;
        private int pm = 0;
        //private int utilization = 0;
        //private int availability = 0;
        #endregion

        #region Constructor
        public LCM2PerformanceGraph1()
        {
            //
        }
        #endregion

        #region Properties
        public int Down
        {
            get
            {
                return this.down;
            }
            set
            {
                this.down = value;
            }
        }

        public int Idle
        {
            get
            {
                return this.idle;
            }
            set
            {
                this.idle = value;
            }
        }

        public int Wait
        {
            get
            {
                return this.wait;
            }
            set
            {
                this.wait = value;
            }
        }

        public int Setup
        {
            get
            {
                return this.setup;
            }
            set
            {
                this.setup = value;
            }
        }

        public int Run
        {
            get
            {
                return this.run;
            }
            set
            {
                this.run = value;
            }
        }

        public int Eng
        {
            get
            {
                return this.eng;
            }
            set
            {
                this.eng = value;
            }
        }

        public int Other
        {
            get
            {
                return this.other;
            }
            set
            {
                this.other = value;
            }
        }

        public int Pm
        {
            get
            {
                return this.pm;
            }
            set
            {
                this.pm = value;
            }
        }


        //public int Utilization
        //{
        //    get
        //    {
        //        return this.utilization;
        //    }
        //    set
        //    {
        //        this.utilization = value;
        //    }
        //}

        //public int Availability
        //{
        //    get
        //    {
        //        return this.availability;
        //    }
        //    set
        //    {
        //        this.availability = value;
        //    }
        //}
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                DataSet dsResult = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("Down", Type.GetType("System.Int32"));
                dt.Columns.Add("Idle", Type.GetType("System.Int32"));
                dt.Columns.Add("Wait", Type.GetType("System.Int32"));
                dt.Columns.Add("Setup", Type.GetType("System.Int32"));
                dt.Columns.Add("Run", Type.GetType("System.Int32"));
                dt.Columns.Add("Eng", Type.GetType("System.Int32"));
                dt.Columns.Add("Other", Type.GetType("System.Int32"));
                dt.Columns.Add("Pm", Type.GetType("System.Int32"));
                //dt.Columns.Add("Utilization", Type.GetType("System.Int32"));
                //dt.Columns.Add("Availability", Type.GetType("System.Int32"));
                dsResult.Tables.Add(dt);

                DataRow dr = dsResult.Tables[0].NewRow();
                dr["Down"] = this.down;
                dr["Idle"] = this.idle;
                dr["Wait"] = this.wait;
                dr["Setup"] = this.setup;
                dr["Run"] = this.run;
                dr["Eng"] = this.eng;
                dr["Other"] = this.other;
                dr["Pm"] = this.pm;
                //dr["Utilization"] = this.utilization;
                //dr["Availability"] = this.availability;
                dsResult.Tables[0].Rows.Add(dr);

                return dsResult;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
