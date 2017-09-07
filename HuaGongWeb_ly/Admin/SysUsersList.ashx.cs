using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// SysUsersList 的摘要说明
    /// </summary>
    public class SysUsersList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            AdminHelper.CheckLogin();

            int pageNum = string.IsNullOrEmpty(context.Request["PageNum"]) ? 1 : Convert.ToInt32(context.Request["PageNum"]);
            DataTable sysUsers = SqlHelper.ExecuteDataTable(@"select * from 
            (select *,ROW_NUMBER() over (order by Id asc) as num from T_SysUsers) t where t.num between @Start and @End",
                                                                                                                     new SqlParameter("@Start", (pageNum - 1) * 5 + 1),
                                                                                                                     new SqlParameter("@End", pageNum * 5));
            int totalCount = (int)SqlHelper.ExecuteScalar("select count(1) from T_SysUsers");
            int pageCount = (int)Math.Ceiling(totalCount / 5.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "SysUsersList.ashx?PageNum=" + (i + 1), Title = "第" + (i + 1) + "页" };
            }
            var data = new { Title = "系统用户管理", SysUsers = sysUsers.Rows, PageData = pageData };
            string html = CommonHelper.RenderHtml("Admin/SysUsersList.html", data);
            context.Response.Write(html);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}