using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// SysUsersEdit 的摘要说明
    /// </summary>
    public class SysUsersEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            bool isSave = !string.IsNullOrEmpty(context.Request["Save"]);
            if (action == "AddNew")
            {
                if (isSave)
                {
                    string name = context.Request["Name"];
                    string password = context.Request["Password"];
                    string realName = context.Request["RealName"];
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(realName))
                    {
                        context.Response.Write("用户名或密码或真实名不能为空！");
                        return;
                    }
                    if (name.Contains("毛泽东"))
                    {
                        context.Response.Write("用户名包含敏感词汇！");
                        return;
                    }
                    DataTable sysUser = SqlHelper.ExecuteDataTable("select * from T_SysUsers where Name=@Name",
                       new SqlParameter("@Name", name));
                    if (sysUser.Rows.Count != 0)
                    {
                        context.Response.Write("该用户名已被其他用户占用！");
                        return;
                    }
                    try
                    {
                        SqlHelper.ExecuteNonQuery("insert into T_SysUsers (Name,Password,RealName) values(@Name,@Password,@RealName)",
                            new SqlParameter("@Name", name),
                            new SqlParameter("@Password", password),
                            new SqlParameter("@RealName", realName));
                        context.Response.Redirect("SysUsersList.ashx");
                    }
                    catch (Exception ex)
                    {
                        context.Response.Write(ex.Message);
                        return;
                    }
                }
                else
                {
                    var data = new { Title = "新增用户", Action = action, SysUsers = new { Name = "", Password = "", RealName = "" } };
                    string html = CommonHelper.RenderHtml("Admin/SysUsersEdit.html", data);
                    context.Response.Write(html);
                }
            }
            else if (action == "Edit")
            {
                if (isSave)
                {
                    long Id = Convert.ToInt64(context.Request["Id"]);
                    string name = context.Request["Name"];
                    string password = context.Request["Password"];
                    string realName = context.Request["RealName"];
                    if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(realName))
                    {
                        context.Response.Write("用户名或密码或真实名不能为空！");
                        return;
                    }
                    if (name.Contains("毛泽东"))
                    {
                        context.Response.Write("用户名包含敏感词汇！");
                        return;
                    }
                    DataTable sysUser = SqlHelper.ExecuteDataTable("select * from T_SysUsers where Name=@Name and Id!=@Id",
                      new SqlParameter("@Name", name),
                      new SqlParameter("@Id", Id));
                    if (sysUser.Rows.Count != 0)
                    {
                        context.Response.Write("该用户名已被其他用户占用！");
                        return;
                    }
                    SqlHelper.ExecuteNonQuery("update T_SysUsers set Name=@Name,Password=@Password,RealName=@RealName where Id=@Id",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@Password", password),
                        new SqlParameter("@RealName", realName),
                        new SqlParameter("@Id", Id));
                    context.Response.Redirect("SysUsersList.ashx");

                }
                else
                {
                    long Id = Convert.ToInt64(context.Request["Id"]);
                    DataTable sysUser = SqlHelper.ExecuteDataTable("select * from T_SysUsers where Id=@Id",
                        new SqlParameter("@Id", Id));
                    if (sysUser.Rows.Count < 1)
                    {
                        context.Response.Write("找不到Id=" + Id + "的用户");
                    }
                    else if (sysUser.Rows.Count > 1)
                    {
                        context.Response.Write("找到多个Id=" + Id + "的用户");
                    }
                    else
                    {
                        var data = new { Title = "编辑用户", Action = action, SysUsers = sysUser.Rows[0] };
                        string html = CommonHelper.RenderHtml("Admin/SysUsersEdit.html", data);
                        context.Response.Write(html);
                    }
                }
            }
            else if (action == "Delete")
            {
                long Id = Convert.ToInt64(context.Request["Id"]);
                DataTable sysUser = SqlHelper.ExecuteDataTable("delete from T_SysUsers where Id=@Id",
                       new SqlParameter("@Id", Id));
                context.Response.Redirect("SysUsersList.ashx");
            }
            else
            {
                context.Response.Write("Action错误！");
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