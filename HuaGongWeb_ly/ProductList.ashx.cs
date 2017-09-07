using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly
{
    /// <summary>
    /// ProductList 的摘要说明
    /// </summary>
    public class ProductList : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string s = context.Request["PageNum"];
            int pageNum = string.IsNullOrEmpty(context.Request["PageNum"]) ? 1 : Convert.ToInt32(context.Request["PageNum"]);
            bool hasCategoryId = !string.IsNullOrEmpty(context.Request["CategoryId"]);
            DataTable products;
            int totalCount = 0;
            if (hasCategoryId)
            {
                int categoryId =Convert.ToInt32(context.Request["CategoryId"]);
                products = SqlHelper.ExecuteDataTable(@"select * from
                                                        ( select *,row_number() over (order by p.Id asc) as num from T_Products p where p.CategoryId=@CategoryId ) as s
                                                        where s.num between @Start and @End",
                                                                                           new SqlParameter("@CategoryId", categoryId),
                                                                                           new SqlParameter("@Start", (pageNum - 1) * 15 + 1),
                                                                                           new SqlParameter("@End", pageNum * 15));
                totalCount = (int)SqlHelper.ExecuteScalar("select Count(1) from T_Products where CategoryId=@CategoryId", new SqlParameter("@CategoryId", categoryId));
            }
            else
            {
                products = SqlHelper.ExecuteDataTable(@"select * from
                                                        ( select *,row_number() over (order by p.Id asc) as num from T_Products p ) as s
                                                        where s.num between @Start and @End",
                                                                                               new SqlParameter("@Start", (pageNum - 1) * 15 + 1),
                                                                                               new SqlParameter("@End", pageNum * 15));
                totalCount = (int)SqlHelper.ExecuteScalar("select Count(1) from T_Products");
            }


            int pageCount = (int)Math.Ceiling(totalCount / 15.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = "ProductList.ashx?PageNum=" + (i + 1), Title = i + 1 };
            }
            var data = new { Products = products.Rows, TotalCount = totalCount, PageCount = pageCount, PageNum = pageNum, PageData = pageData, ProductCategories = CommonHelper.GetProductCategories(), Settings = CommonHelper.GetSettings() };
            string html = CommonHelper.RenderHtml("Front/ProductList.html", data);
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