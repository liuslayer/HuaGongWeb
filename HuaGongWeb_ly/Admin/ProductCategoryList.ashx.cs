using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// ProductCategoryList 的摘要说明
    /// </summary>
    public class ProductCategoryList : IHttpHandler
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
            DataTable productCategories = SqlHelper.ExecuteDataTable(@"select * from 
                                                                    (select *,row_number() over (order by Id asc) as num from T_ProductCategories) as t 
                                                                    where t.num between @Start and @End",
                                                                                                        new SqlParameter("@Start", (pageNum - 1) * 10 + 1),
                                                                                                        new SqlParameter("@End", pageNum * 10));
            int totalCount = (int)SqlHelper.ExecuteScalar("select count(1) from T_ProductCategories");
            int pageCount = (int)Math.Ceiling(totalCount / 10.0);
            object[] pageData = new object[pageCount];
            for (int i = 0; i < pageCount; i++)
            {
                pageData[i] = new { Href = string.Format("ProductCategoryList.ashx&PageNum={0}", i + 1), Title = "第" + (i + 1) + "页" };
            }
            var data = new { Title = "产品类别列表", ProductCategories = productCategories.Rows, PageData = pageData };
            string html = CommonHelper.RenderHtml("Admin/ProductCategoryList.html", data);
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