using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly
{
    /// <summary>
    /// ProductView 的摘要说明
    /// </summary>
    public class ProductView : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string Id = context.Request["Id"];
            DataTable products = SqlHelper.ExecuteDataTable("select * from T_Products where Id=@Id",
                new SqlParameter("@Id", Id));
            if (products.Rows.Count < 1)
            {
                context.Response.Write("找不到");
            }
            else if (products.Rows.Count > 1)
            {
                context.Response.Write("找到多条数据");
            }
            else
            {
                DataRow product = products.Rows[0];
                var data = new { Product = product, ProductCategories = CommonHelper.GetProductCategories(), Settings = CommonHelper.GetSettings() };
                string html = CommonHelper.RenderHtml("Front/ProductView.html", data);
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