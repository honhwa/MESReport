using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Report.Library;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Report.Sources
{
    public class WIPAnalysis
    {
        private string strSql = string.Empty;
        private string strFab = string.Empty;
        private List<string> lststrProdGroup = new List<string>();
        private string strWorder = string.Empty;
        private List<string> lststrPartID = new List<string>();
        private List<string> lststrCell2Workctr=new List<string> ();
        private List<string> lststrModuleWorkctr = new List<string>();


        public string FabID
        {
            get
            {
                return  this.strFab;
            }

            set
            {
                this.strFab = value;
            }
        }

        public List<string> Cell2Station
        {
            get
            {
                return this.lststrCell2Workctr;
            }
            set
            {
                this.lststrCell2Workctr = value;
            }
        }   
       
        public List<string> ModuleStation
        {
            get
            {
                return this.lststrModuleWorkctr;
            }
            set
            {
                this.lststrModuleWorkctr = value;
            }
        }

        public List<string> ProdGroup
        {
            get
            {
                return this.lststrProdGroup;
            }
            set
            {
                this.lststrProdGroup = value;
            }
        }

        public List<string> ProdID
        {
            get
            {
                return this.lststrPartID;
            }
            set
            {
                this.lststrPartID = value;
            }
        }


    }
}
