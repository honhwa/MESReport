using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Report.Library
{
    [Serializable]
    public class UserLogin
    {
        #region parameters
        private string strSql = string.Empty;
        private string strErrorMsg = string.Empty;

        private string strUid = string.Empty;
        private string strPwd = string.Empty;
        private string strFabId = string.Empty;

        //private DataSet dsUser;
        //private XmlDataDocument userXml;
        private UserInfo user;
        private bool bIfLoginSuccess = false;
                
        DBConn dbConn;
        

        
        #endregion

        #region Properties
        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMsg
        {
            get
            {
                return this.strErrorMsg;
            }
        }

        /// <summary>
        /// UserInfo
        /// </summary>
        public UserInfo User
        {
            get
            {
                return this.user;
            }
        }

        /// <summary>
        /// User DataSet
        /// </summary>
        //public DataSet UserDataSet
        //{
        //    get
        //    {
        //        return this.dsUser;
        //    }
        //}

        /// <summary>
        /// Login Result
        /// </summary>
        public bool LoginResult
        {
            get
            {
                return this.bIfLoginSuccess;
            }
        }
        #endregion

        #region constructor
        public UserLogin(string fabId, string uid)
        {
            try
            {
                this.user = UserInfo.Create(fabId, uid);
                if (this.user == null)
                {
                    this.strErrorMsg = "找不到此用戶信息!";
                    this.bIfLoginSuccess = false;
                }
                else
                {
                    if (this.user.OnJob != "0")
                    {
                        this.strErrorMsg = "您已經離職!";
                        this.bIfLoginSuccess = false;
                    }
                    else
                    {
                        this.bIfLoginSuccess = true;
                        this.strErrorMsg = string.Empty;
                    }
                }

                //this.strSql = "SELECT FABID,USERID,USERNAME,[PASSWORD],ONJOB FROM MEMM WHERE USERID = '" + uid + "' AND FABID = '" + fabId + "'";

                //dbConn = new DBConn();
                ////this.dsUser = dbConn.GetDataSetBySql(strSql, "");
                //if (this.dsUser.Tables[0].Rows.Count <= 0)
                //{
                //    this.strErrorMsg = "User is not exist!";
                //    this.bIfLoginSuccess = false;
                //}
                //else if (!this.dsUser.Tables[0].Rows[0]["ONJOB"].ToString().Equals("0"))
                //{
                //    this.strErrorMsg = "You have left!";
                //    this.bIfLoginSuccess = false;
                //}
                //else
                //{
                    //this.user = UserInfo.Create(fabId, uid);
                    //XmlSerializer serializer = new XmlSerializer(typeof(UserInfo));
                    //this.userXml = new XmlDataDocument();
                    //MemoryStream stream = new MemoryStream();
                    //TextWriter writer = new StreamWriter(stream, new UTF8Encoding());
                    //serializer.Serialize(writer, this.user);
                    //userXml.Load(stream);
                    //writer.Close();
                    //stream.Close();

                    //this.bIfLoginSuccess = true;
                    //this.strErrorMsg = string.Empty;
                
            }
            catch (Exception ex)
            {
                this.strErrorMsg = ex.Message;
            }
            
        }
        #endregion
    }
}
