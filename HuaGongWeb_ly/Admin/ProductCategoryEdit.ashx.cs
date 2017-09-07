using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// ProductCategoryEdit 的摘要说明
    /// </summary>
    public class ProductCategoryEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/hteml";
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            bool isPostback = !string.IsNullOrEmpty(context.Request["IsPostback"]);
            if (action == "AddNew")
            {
                if (isPostback)
                {
                    string name = context.Request["Name"];
                    SqlHelper.ExecuteNonQuery("insert into T_ProductCategories(Name) values(@Name)",
                       new SqlParameter("@Name", name));
                    context.Response.Redirect("ProductCategoryList.ashx");
                }
                else
                {
                    var data = new { Title = "新增产品分类", Action = action, ProductCategory = new { Name = "" } };
                    string html = CommonHelper.RenderHtml("Admin/ProductCategoryEdit.html", data);
                    context.Response.Write(html);
                }
            }
            else if (action == "Edit")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                DataTable productCategories = SqlHelper.ExecuteDataTable("select * from T_ProductCategories where Id=@Id", new SqlParameter("@Id", id));
                if (isPostback)
                {
                    string name = context.Request["Name"];
                    SqlHelper.ExecuteNonQuery("update T_ProductCategories set Name=@Name where Id=@Id",
                            new SqlParameter("@Name", name),
                            new SqlParameter("@Id", id));
                    context.Response.Redirect("ProductCategoryList.ashx");
                }
                else
                {
                    if (productCategories.Rows.Count < 1)
                    {
                        context.Response.Write("找不到Id=" + id + "的产品类别");
                    }
                    else if (productCategories.Rows.Count > 1)
                    {
                        context.Response.Write("找到多个Id=" + id + "的产品类别");
                    }
                    else
                    {
                        var data = new { Title = "编辑产品类别", Action = action, ProductCategory = productCategories.Rows[0] };
                        string html = CommonHelper.RenderHtml("Admin/ProductCategoryEdit.html", data);
                        context.Response.Write(html);
                    }                 
                }
            }
            else if (action == "Delete")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                SqlHelper.ExecuteNonQuery("delete from T_ProductCategories where Id=@Id",
                           new SqlParameter("@Id", id));
                context.Response.Redirect("ProductCategoryList.ashx");
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