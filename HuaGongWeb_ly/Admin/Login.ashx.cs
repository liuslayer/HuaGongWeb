using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            bool isPostBack = !string.IsNullOrEmpty(context.Request["Login"]);
            if (isPostBack)
            {
                string name = context.Request["Name"];
                string password = context.Request["Password"];
                DataTable users = SqlHelper.ExecuteDataTable("select * from T_SysUsers where Name=@Name and Password=@Password",
                    new SqlParameter("Name", name),
                    new SqlParameter("Password", password));
                if (users.Rows.Count <= 0)
                {
                    context.Response.Write("用户名或密码错误！");
                }
                else
                {
                    context.Session["Name"] = name;
                    context.Response.Redirect("ProductList.ashx");
                }
            }
            else
            {
                string html = CommonHelper.RenderHtml("Admin/Login.html", null);
                context.Response.Write(html);
            }
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