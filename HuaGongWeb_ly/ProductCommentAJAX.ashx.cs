using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace HuaGongWeb_ly
{
    /// <summary>
    /// ProductCommentAJAX 的摘要说明
    /// </summary>
    public class ProductCommentAJAX : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string action = context.Request["Action"];
            if (action == "Load")
            {
                int productId = Convert.ToInt32(context.Request["ProductId"]);
                DataTable dt = SqlHelper.ExecuteDataTable("select * from T_ProductComments where ProductId=@ProductId",
                    new SqlParameter("@ProductId", productId));
                object[] comments = new object[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comments[i] = new { CreateDateTime = dt.Rows[i]["CreateDateTime"].ToString(), Title = dt.Rows[i]["Title"], Msg = dt.Rows[i]["Msg"] };
                }
                string json = new JavaScriptSerializer().Serialize(comments);
                context.Response.Write(json);
            }
            else if (action == "Post")
            {
                int productId = Convert.ToInt32(context.Request["ProductId"]);
                string title = context.Request["Title"];
                string msg = context.Request["Msg"];
                if (title.Contains(">") || title.Contains("<")
                   || msg.Contains(">") || msg.Contains("<"))
                {
                    context.Response.Write("error:存在不合法的字符");
                    return;
                }
                try
                {
                    SqlHelper.ExecuteNonQuery("insert into T_ProductComments (ProductId,Title,Msg,CreateDateTime) values(@ProductId,@Title,@Msg,getdate())",
                        new SqlParameter("@ProductId", productId),
                        new SqlParameter("@Title", title),
                        new SqlParameter("@Msg", msg));
                    context.Response.Write("suc");
                }
                catch (Exception ex)
                {
                    context.Response.Write(ex.Message);
                }
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