using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace HuaGongWeb_ly.Admin
{
    /// <summary>
    /// ProductEdit 的摘要说明
    /// </summary>
    public class ProductEdit : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            bool isPostback = !string.IsNullOrEmpty(context.Request["IsPostback"]);
            DataTable productCategories = SqlHelper.ExecuteDataTable("select * from T_ProductCategories");

            if (action == "AddNew")
            {
                if (isPostback)
                {
                    string name = context.Request["Name"];
                    long categoryId = Convert.ToInt64(context.Request["CategoryId"]);
                    HttpPostedFile productImg = context.Request.Files["ProductImage"];
                    string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(productImg.FileName);//有bug的，一毫秒内多个人上传多个文件
                    productImg.SaveAs(context.Server.MapPath("~/uploadfile/" + filename));
                    string msg = context.Request["Msg"];
                    bool isRecommend = Convert.ToBoolean(context.Request["IsRecommend"] == "on");
                    SqlHelper.ExecuteNonQuery("insert into T_Products(Name,ImagePath,Msg,CategoryId,IsRecommend) values(@Name,@ImagePath,@Msg,@CategoryId,@IsRecommend)",
                        new SqlParameter("@Name", name),
                        new SqlParameter("@ImagePath", "/uploadfile/" + filename),
                        new SqlParameter("@Msg", msg),
                        new SqlParameter("@CategoryId", categoryId),
                        new SqlParameter("@IsRecommend", isRecommend));
                    context.Response.Redirect("ProductList.ashx");
                }
                else
                {
                    var data = new { Title = "新增产品", Action = action, Product = new { Name = "", CategoryId = 0, ImagePath = "", Msg = "" }, Categories = productCategories.Rows };
                    string html = CommonHelper.RenderHtml("Admin/ProductEdit.htm", data);
                    context.Response.Write(html);
                }
            }
            else if (action == "Edit")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                DataTable products = SqlHelper.ExecuteDataTable("select * from T_Products where Id=@Id", new SqlParameter("@Id", id));
                if (isPostback)
                {
                    string name = context.Request["Name"];
                    long categoryId = Convert.ToInt64(context.Request["CategoryId"]);
                    string msg = context.Request["Msg"];
                    bool isRecommend = Convert.ToBoolean(context.Request["IsRecommend"] == "on");
                    HttpPostedFile productImg = context.Request.Files["ProductImage"];
                    if (CommonHelper.HasFile(productImg))
                    {
                        string oldFileName = products.Rows[0]["ImagePath"].ToString();
                        string path = context.Server.MapPath(oldFileName);
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }

                        string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(productImg.FileName);
                        productImg.SaveAs(context.Server.MapPath("~/uploadfile/" + filename));
                        SqlHelper.ExecuteNonQuery("update T_Products set Name=@Name,ImagePath=@ImagePath,Msg=@Msg,CategoryId=@CategoryId,IsRecommend=@IsRecommend where Id=@Id",
                            new SqlParameter("@Name", name),
                            new SqlParameter("@ImagePath", "/uploadfile/" + filename),
                            new SqlParameter("@Msg", msg),
                            new SqlParameter("@CategoryId", categoryId),
                            new SqlParameter("@IsRecommend", isRecommend),
                            new SqlParameter("@Id", id));
                    }
                    else
                    {
                        SqlHelper.ExecuteNonQuery("update T_Products set Name=@Name,Msg=@Msg,CategoryId=@CategoryId,IsRecommend=@IsRecommend where Id=@Id",
                               new SqlParameter("@Name", name),
                               new SqlParameter("@Msg", msg),
                               new SqlParameter("@CategoryId", categoryId),
                               new SqlParameter("@IsRecommend", isRecommend),
                               new SqlParameter("@Id", id));
                    }
                    context.Response.Redirect("ProductList.ashx");
                }
                else
                {                 
                    if (products.Rows.Count < 1)
                    {
                        context.Response.Write("找不到Id=" + id + "的产品");
                    }
                    else if (products.Rows.Count > 1)
                    {
                        context.Response.Write("找到多个Id=" + id + "的产品");
                    }
                    else
                    {
                        var data = new { Title = "编辑产品", Action = action, Product = products.Rows[0], Categories = productCategories.Rows };
                        string html = CommonHelper.RenderHtml("Admin/ProductEdit.htm", data);
                        context.Response.Write(html);
                    }                 
                }
            }
            else if (action == "Delete")
            {
                long id = Convert.ToInt64(context.Request["Id"]);
                SqlHelper.ExecuteNonQuery("delete from T_Products where Id=@Id",
                    new SqlParameter("@Id", id));
                context.Response.Redirect("ProductList.ashx");
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