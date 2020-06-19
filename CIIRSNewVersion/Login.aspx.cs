using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CIIRSUtility;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Text;

namespace CIIRSNewVersion
{
    public partial class Login : System.Web.UI.Page
    {
        private dbAccess db = new dbAccess();
        string uName, pwd;
        string u_id, Dep_id, role_id, username, dep_flag;
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            //{ 
            //if (tb_uName.Text == "" && tb_pwd.Text == "")
            {
                //Response.Flush();
                //Session["u_id"] = "";
                Session.Remove("u_id");
                Session.RemoveAll();
            }
            //}
        }

        protected void btn_login_Click(object sender, EventArgs e)
        {
            uName = tb_uName.Text;
            pwd = tb_pwd.Text;
            if (verify(uName, pwd) == true)
            {
                Session["u_id"] = u_id;
                Session["Dep_id"] = Dep_id;
                Session["Role_id"] = role_id;
                Session["username"] = username;
                if (Dep_id == "900" || Dep_id == "910")
                    dep_flag = "1";
                else
                    dep_flag = "0";
                //Session["flag"] = 1;
                Session["dep_flag"] = dep_flag;

                Response.Redirect("client_mod.aspx");
            }
            else
                Response.Write("<script>alert('Please input Valid Username And Password')</script>");
        }
        protected bool verify(string uName, string pwd)
        {
            if (uName == "" && pwd == "")
                return false;
            else
            {
                try
                {
                    this.db.Cn.Open();
                    this.db.Cmd.CommandType = CommandType.Text;
                    this.db.Cmd.CommandTimeout = 0;
                    string st = "Select * from Administrator where (username = '" + uName + "' and password = '" + pwd + "') and Status = 1";
                    this.db.Cmd.CommandText = st;
                    this.db.Da.SelectCommand = this.db.Cmd;
                    DataTable dt = new DataTable();
                    this.db.Da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        u_id = dt.Rows[0]["u_id"].ToString();
                        Dep_id = dt.Rows[0]["Dep_id"].ToString();
                        role_id = dt.Rows[0]["Role_id"].ToString();                        
                    }
                    this.db.Cn.Close();
                    return true;
                }
                
                catch (Exception ex)
                {
                    //throw ex;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + ex.Message.ToString() + "')", true);
                    return false;
                }

            }
        }
    }
}