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
    public class LCM2PerformanceGraph2
    {
        #region Parameters
        private int acc = 0;
        private int eng = 0;
        private int fac = 0;
        private int iqc = 0;
        private int mfg = 0;
        private int other = 0;
        private int pc = 0;
        private int pe = 0;
        private int pur = 0;
        private int qra = 0;
        #endregion

        #region Constructor
        public LCM2PerformanceGraph2()
        {
            //
        }
        #endregion

        #region Property
        public int Acc
        {
            get
            {
                return this.acc;
            }
            set
            {
                this.acc = value;
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

        public int Fac
        {
            get
            {
                return this.fac;
            }
            set
            {
                this.fac = value;
            }
        }

        public int Iqc
        {
            get
            {
                return this.iqc;
            }
            set
            {
                this.iqc = value;
            }
        }

        public int Mfg
        {
            get
            {
                return this.mfg;
            }
            set
            {
                this.mfg = value;
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

        public int Pc
        {
            get
            {
                return this.pc;
            }
            set
            {
                this.pc = value;
            }
        }

        public int Pe
        {
            get
            {
                return this.pe;
            }
            set
            {
                this.pe = value;
            }
        }

        public int Pur
        {
            get
            {
                return this.pur;
            }
            set
            {
                this.pur = value;
            }
        }

        public int Qra
        {
            get
            {
                return this.qra;
            }
            set
            {
                this.qra = value;
            }
        }
        #endregion

        #region Methods
        public DataSet GetDataSet()
        {
            try
            {
                DataSet dsResult = new DataSet();
                DataTable dt = new DataTable();
                dt.Columns.Add("Acc", Type.GetType("System.Int32"));
                dt.Columns.Add("Eng", Type.GetType("System.Int32"));
                dt.Columns.Add("Fac", Type.GetType("System.Int32"));
                dt.Columns.Add("Iqc", Type.GetType("System.Int32"));
                dt.Columns.Add("Mfg", Type.GetType("System.Int32"));
                dt.Columns.Add("Other", Type.GetType("System.Int32"));
                dt.Columns.Add("Pc", Type.GetType("System.Int32"));
                dt.Columns.Add("Pe", Type.GetType("System.Int32"));
                dt.Columns.Add("Pur", Type.GetType("System.Int32"));
                dt.Columns.Add("Qra", Type.GetType("System.Int32"));
                dsResult.Tables.Add(dt);

                DataRow dr = dsResult.Tables[0].NewRow();
                dr["Acc"] = this.acc;
                dr["Eng"] = this.eng;
                dr["Fac"] = this.fac;
                dr["Iqc"] = this.iqc;
                dr["Mfg"] = this.mfg;
                dr["Other"] = this.other;
                dr["Pc"] = this.pc;
                dr["Pe"] = this.pe;
                dr["Pur"] = this.pur;
                dr["Qra"] = this.qra;
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
