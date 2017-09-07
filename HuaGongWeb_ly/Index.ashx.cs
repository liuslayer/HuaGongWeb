using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly
{
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    public class Index : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            //master
            context.Response.ContentType = "text/html";
            DataTable recommendPorducts = SqlHelper.ExecuteDataTable("select * from T_Products where IsRecommend=1");
            var data = new { RecommendPorducts = recommendPorducts.Rows, ProductCategories = CommonHelper.GetProductCategories(), Settings = CommonHelper.GetSettings() };
            string html = CommonHelper.RenderHtml("Front/index.html", data);
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