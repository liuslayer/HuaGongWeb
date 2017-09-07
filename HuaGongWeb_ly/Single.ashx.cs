﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly
{
    /// <summary>
    /// Single 的摘要说明
    /// </summary>
    public class Single : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            var data = new { ProductCategories = CommonHelper.GetProductCategories(), Settings = CommonHelper.GetSettings() };
            string html = CommonHelper.RenderHtml("Front/single.html", data);
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