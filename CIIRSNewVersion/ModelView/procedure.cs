using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using CIIRSUtility;

namespace CiirsDotnet.ModelView
{
    public class procedure
    {
        DataSet ds;
        dbAccess_VM db = new dbAccess_VM();
        public procedure()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataSet eIOM_Print(int iom_id, string cpno, string client_no)
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.StoredProcedure;
            this.db.Cmd.CommandTimeout = 0;
            //st = "Select Role_Per_ID from CIIRS_Admin.dbo.tbl_role where Role_ID in (Select Role_ID from Administrator where u_id = '" + u_id + "')";

            this.db.Cmd.CommandText = "pro_EIOM_Print";
            this.db.cmd.Parameters.AddWithValue("@clientid", client_no);
            this.db.cmd.Parameters.AddWithValue("@clpno", cpno);
            this.db.cmd.Parameters.AddWithValue("@iom_id", iom_id);
            this.db.cmd.Parameters.AddWithValue("@report", 0);
            this.db.Da.SelectCommand = this.db.Cmd;
            ds = new DataSet();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(ds);
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
                ds.Dispose();
                throw ex;
            }
            return ds;
        }
        public DataSet ppg_1stPage(string cpno, string client_no)
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.StoredProcedure;
            this.db.Cmd.CommandTimeout = 0;
            //st = "Select Role_Per_ID from CIIRS_Admin.dbo.tbl_role where Role_ID in (Select Role_ID from Administrator where u_id = '" + u_id + "')";

            this.db.Cmd.CommandText = "pro_ProductFormats";
            this.db.cmd.Parameters.AddWithValue("@clientid", client_no);
            this.db.cmd.Parameters.AddWithValue("@clpno", cpno);
            this.db.cmd.Parameters.AddWithValue("@report", 4);
            this.db.Da.SelectCommand = this.db.Cmd;
            ds = new DataSet();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(ds);
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
                ds.Dispose();
                throw ex;
            }
            return ds;
        }
        public DataSet ppg_3rdPage(string cpno, string client_no)
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.StoredProcedure;
            this.db.Cmd.CommandTimeout = 0;
            //st = "Select Role_Per_ID from CIIRS_Admin.dbo.tbl_role where Role_ID in (Select Role_ID from Administrator where u_id = '" + u_id + "')";

            this.db.Cmd.CommandText = "pro_SMEFormats";
            this.db.cmd.Parameters.AddWithValue("@clientid", client_no);
            this.db.cmd.Parameters.AddWithValue("@clpno", cpno);
            this.db.cmd.Parameters.AddWithValue("@report", 20);
            this.db.Da.SelectCommand = this.db.Cmd;
            ds = new DataSet();
            //db.CloseConn();
            try
            {
                this.db.Da.Fill(ds);
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
                ds.Dispose();
                throw ex;
            }
            return ds;
        }
        public DataSet ppg_2ndPage(string cpno, string client_no)
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.StoredProcedure;
            this.db.Cmd.CommandTimeout = 0;
            this.db.Cmd.CommandText = "pro_CROFormats_2017";
            this.db.cmd.Parameters.AddWithValue("@clientid", client_no);
            this.db.cmd.Parameters.AddWithValue("@clpno", cpno);
            this.db.cmd.Parameters.AddWithValue("@report", 26);
            this.db.cmd.Parameters.AddWithValue("@dep_id", 0);
            this.db.Da.SelectCommand = this.db.Cmd;
            ds = new DataSet();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(ds);
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
                ds.Dispose();
                throw ex;
            }
            return ds;
        }
        public DataSet ppg_2ndPage_Sec(string cpno, string client_no)
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.StoredProcedure;
            this.db.Cmd.CommandTimeout = 0;
            this.db.Cmd.CommandText = "pro_ProductSMEFormats_Copy";
            this.db.cmd.Parameters.AddWithValue("@clientid", client_no);
            this.db.cmd.Parameters.AddWithValue("@clpno", cpno);
            this.db.cmd.Parameters.AddWithValue("@depid", 1);
            this.db.cmd.Parameters.AddWithValue("@report", 26);
            this.db.Da.SelectCommand = this.db.Cmd;
            ds = new DataSet();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(ds);
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
                ds.Dispose();
                throw ex;
            }
            return ds;
        }
    }
}