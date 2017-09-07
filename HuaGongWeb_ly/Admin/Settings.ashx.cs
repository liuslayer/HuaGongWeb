using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// Settings 的摘要说明
    /// </summary>
    public class Settings : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            AdminHelper.CheckLogin();

            bool isPostback = !string.IsNullOrEmpty(context.Request["IsPostback"]);
            if (isPostback)
            {
                string siteName = context.Request["SiteName"];
                string siteURL = context.Request["SiteURL"];
                string address = context.Request["Address"];
                string postCode = context.Request["PostCode"];
                string contactPerson = context.Request["ContactPerson"];
                string telPhone = context.Request["TelPhone"];
                string fax = context.Request["Fax"];
                string mobile = context.Request["Mobile"];
                string email = context.Request["Email"];
                CommonHelper.WriteSetting("SiteName", siteName);
                CommonHelper.WriteSetting("SiteURL", siteURL);
                CommonHelper.WriteSetting("Address", address);
                CommonHelper.WriteSetting("PostCode", postCode);
                CommonHelper.WriteSetting("ContactPerson", contactPerson);
                CommonHelper.WriteSetting("TelPhone", telPhone);
                CommonHelper.WriteSetting("Fax", fax);
                CommonHelper.WriteSetting("Mobile", mobile);
                CommonHelper.WriteSetting("Email", email);
                context.Response.Write("保存成功！");
            }
            else
            {
                var data = new { Title = "系统配置", Settings = CommonHelper.GetSettings() };
                string html = CommonHelper.RenderHtml("Admin/Settings.html", data);
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