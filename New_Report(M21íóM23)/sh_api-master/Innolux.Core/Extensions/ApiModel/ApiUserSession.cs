namespace Innolux.Core.Extensions.ApiModel
{
    /// <summary>
    /// 登錄者信息
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// 人員識別碼 corpemployee.id
        /// </summary>
        public string Eid { get; set; }
        /// <summary>
        /// 工號
        /// </summary>
        public string EmpNo { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 部門識別碼 corporganization_bz_fact.id
        /// </summary>
        public string WorkOrgId { get; set; }
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DeptCode { get; set; }
        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 賬號識別碼 dguser.id
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 組織層級
        /// </summary>
        public string OrgLevel { get; set; }
        /// <summary>
        /// 直間接
        /// </summary>
        public string Indirect { get; set; }
        public string DomainAccount { get; set; }
        public string EMail { get; set; }

    }
}
