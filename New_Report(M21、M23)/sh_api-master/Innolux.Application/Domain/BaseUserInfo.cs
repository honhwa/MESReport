using Innolux.Application.Dto;
using Innolux.EFCore;
using Innolux.Core.Extensions.EntityFrameworkCore;
using Innolux.Core.Exception;
using Innolux.Core.Extensions.ApiModel;

namespace Innolux.Application.Domain
{
    public class BaseUserInfo
    {
        private readonly MSSqlDbContext _mssql;

        public BaseUserInfo(MSSqlDbContext mssql)
        {
            _mssql = mssql;
        }

        public UserInfo GetUserInfo(string empno)
        {
            var users = _mssql.Database.SqlQuery<UserInfo>($@"
                        select a.id eid,a.workorgid,empno,a.name,a.indirect, c.deptcode,c.name deptname,c.deptcode,d.id uid,
                         LOWER(d.email) email,e.code orglevel,
                        LOWER(SUBSTRING(d.email,0, CHARINDEX('@', d.email, 0))) domainAccount
                        from CORPEMPLOYEE a,corporganization b ,corpdept c,dguser d,corpcode e
                        where a.workorgid=b.id 
                        and b.sid =c.id
                        and a.empno=d.uid
                        and b.orglevel=e.id
                        and a.empno = '{empno}' ");

            if (users.Count == 0)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSuchUser));
            }
            return users[0];
        }

        public List<UserInfo> GetUserInfoList(string[] empnoarray)
        {
            string sql = @"select a.id eid,a.workorgid,empno,a.name,a.indirect, c.deptcode,c.name deptname,c.deptcode,d.id uid,
                         LOWER(d.email) email,e.code orglevel,
                        LOWER(SUBSTRING(d.email,0, CHARINDEX('@', d.email, 0))) domainAccount
                        from CORPEMPLOYEE a,corporganization b ,corpdept c,dguser d,corpcode e
                        where a.workorgid=b.id 
                        and b.sid =c.id
                        and a.empno=d.uid
                        and b.orglevel=e.id
                        and a.empno in ('{0}')";
            sql = string.Format(sql, string.Join("','", empnoarray));
            var users = _mssql.Database.SqlQuery<UserInfo>(sql);
            if (users.Count == 0)
            {
                throw new InnoluxException(new ErrResult(ApiCodeEnum.NoSuchUser));
            }
            return users;
        }

    }
}
