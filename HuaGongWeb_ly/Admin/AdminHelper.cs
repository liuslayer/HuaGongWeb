using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    public class AdminHelper
    {
        public static void CheckLogin()
        {
            HttpContext context = HttpContext.Current;
            if (context.Session["Name"] == null)
            {
                context.Response.Redirect("Login.ashx");
            }
            else
            {
                string name = context.Session["Name"].ToString();
            }
        }
    }
}