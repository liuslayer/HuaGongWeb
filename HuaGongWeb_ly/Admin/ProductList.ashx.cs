using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.SessionState;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// ProductList 的摘要说明
    /// </summary>
    public class ProductList : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";

            AdminHelper.CheckLogin();

            int pageNum = 1;
            if (context.Request["PageNum"] != null)
            {
                pageNum = Convert.ToInt32(context.Request["PageNum"]);
            }
            DataTable products = SqlHelper.ExecuteDataTable(@"select * from
(
select p.Id as Id,p.Name as Name,c.Name as CategoryName, 
row_number() over (order by p.Id asc) as num 
from T_Products p 
left join T_ProductCategories c on p.CategoryId=c.Id
) as s
where s.num between @Start and @End",
                                    new SqlParameter("@Start", (pageNum - 1) * 10 + 1),
                                    new SqlParameter("@End", pageNum * 10));

            int totalCount = (int)SqlHelper.ExecuteScalar("select Count(1) from T_Products");
            int pageCount = (int)Math.Ceiling(totalCount / 10.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
			{
                pageData[i] = new { Href = "ProductList.ashx?PageNum=" + (i + 1), Title = "第" + (i + 1) + "页" };
			}
            var data = new {Title="产品列表", Products = products.Rows, PageData = pageData };
            string html = CommonHelper.RenderHtml("Admin/ProductList.htm", data);
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