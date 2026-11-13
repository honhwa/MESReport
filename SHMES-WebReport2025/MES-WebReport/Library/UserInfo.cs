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
using System.Text;

namespace Report.Library
{
    /// <summary>
    /// UserInfomation
    /// </summary>
    [Serializable]
    public class UserInfo
    {
        #region parameters
        private string strUserID = string.Empty;
        private string strPassword = string.Empty;
        private string strFabId = string.Empty;
        private string strLoginTime = string.Empty;
        private string strUserName = string.Empty;
        private string strOnJob = string.Empty;
        //20211221 penny : for tisax
        private string strRole = string.Empty;
        #endregion

        #region properties
        /// <summary>
        /// UserID
        /// </summary>
        public string UserID
        {
            get
            {
                return this.strUserID;
            }
        }

        /// <summary>
        /// User Name
        /// </summary>
        public string UserName
        {
            get
            {
                return this.strUserName;
            }
        }

        /// <summary>
        /// Password
        /// </summary>
        public string Password
        {
            get
            {
                return this.strPassword;
            }
        }

        /// <summary>
        /// FabId
        /// </summary>
        public string FabId
        {
            get
            {
                return this.strFabId;
            }
        }

        /// <summary>
        /// OnJob
        /// </summary>
        public string OnJob
        {
            get
            {
                return this.strOnJob;
            }
        }

        /// <summary>
        /// LoginTime
        /// </summary>
        public string LoginTime
        {
            get
            {
                return this.strLoginTime;
            }
        }

        /// <summary>
        /// 20211221 penny : for tisax
        /// </summary>
        public string Role
        {
            get
            {
                return this.strRole;
            }
        }
        #endregion

        #region constructor
        /// <summary>
        /// User Object
        /// </summary>
        public UserInfo()
        {
            //
        }
        #endregion

        #region methods
        /// <summary>
        /// 創建User的信息
        /// </summary>
        /// <param name="uid">UserID</param>
        /// <param name="fabId">FabID</param>
        /// <returns>UserInfo</returns>
        internal static UserInfo Create(string fabId, string uid)
        {
            //string strSql = "SELECT FABID,USERID,USERNAME,[PASSWORD],ONJOB,GETDATE() AS LOGINTIME,TITLE FROM MEMM WHERE FABID = '" + fabId + "' AND USERID = '" + uid + "'";
            string strSql = "";
            strSql = $@"SELECT FABID,USERID,USERNAME,[PASSWORD],ONJOB,GETDATE() AS LOGINTIME ,TITLE
                        FROM [SHMESWIP\MESWIP].[SHMESWIP].[dbo].[memm] WHERE FABID = '{fabId}' AND USERID = '{uid}'";
            DBConn dbConn = new DBConn();
            DataTable dtUserInfo = dbConn.GetDataSetBySql(strSql).Tables[0];
            if (dtUserInfo.Rows.Count <= 0)
            {
                //throw new Exception("沒有此用戶信息!");
                return null;
            }
            else
            {
                UserInfo u = new UserInfo();
                u.strFabId = dtUserInfo.Rows[0]["FABID"].ToString();
                u.strUserID = dtUserInfo.Rows[0]["USERID"].ToString();
                u.strUserName = dtUserInfo.Rows[0]["USERNAME"].ToString();
                //20240228 penny : decrypt user password
                if(ConfigurationManager.AppSettings.Get("EnableCryptUserPassword").ToLower() == "true")
                {
                    //对密码解密
                    //u.strPassword=mdlEncryption.DecryptPSW(Encoding.UTF8.GetString(Convert.FromBase64String(dtUserInfo.Rows[0]["PASSWORD"].ToString())));

                    //使用AES对密码解密
                    u.strPassword = AESCbcPbkdf2.DecryptAesCBC(dtUserInfo.Rows[0]["PASSWORD"].ToString(), Const.SECRET_KEY);
                }
                else
                {
                    u.strPassword = dtUserInfo.Rows[0]["PASSWORD"].ToString(); 
                }
                u.strOnJob = dtUserInfo.Rows[0]["ONJOB"].ToString();
                u.strLoginTime = dtUserInfo.Rows[0]["LOGINTIME"].ToString();
                //20211221 penny : for tisax
                u.strRole = dtUserInfo.Rows[0]["TITLE"].ToString();

                return u;
            }
        }
        #endregion
    }
}
