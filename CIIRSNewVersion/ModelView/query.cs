using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using CIIRSUtility; 

namespace CiirsDotnet.ModelView
{
    public class query
    {
        SqlCommand objCmd4;
        public dbAccess db = new dbAccess();
        DataTable dt;
        DataSet ds;
        string client_no;
        int iom_id;
        string u_id, st, cpno;

        public query()
        {

            //
            // TODO: Add constructor logic here
            //
        }
        public DataTable Show_Eclps_ClientWise(string client_no)
        {
            this.client_no = client_no;
            Start_Method();
            st = "Select ROW_NUMBER() OVER(ORDER BY cpno DESC) AS SNo,cpno, "
                        + "Case when CLP_Status_ID =0 then 'In Process' else CLP_Status end [CLP_Status],te.name "
                        + "from proposalinfo p "
                        + "inner join tbl_eCLP_Type te on te.ID = p.eCLP_type "
                        + "where client_no =" + client_no + " AND not CLP_Status_ID = 16 "
                        + "order by cpno desc";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable Show_Eclps(string u_id)
        {

            this.u_id = u_id;
            Start_Method();
            st = " Select pr.client_no,pr.cpno , name ,BrName,ad.First_Name +' '+ isnull(ad.Last_Name,'') [eCLP_Send_By], pr.CLP_SendDate[eCLP_Recieving_Date] "
                        + "from ProposalInfo pr "
                        + "inner join (Select max(CLP_Log_id)CLP_Log_id , CpNo from CLP_Logs_New group by cpno) cln on cln.CpNo = pr.cpno "
                        + "inner join CLP_Logs_New cln1 on cln1.CpNo = cln.CpNo and cln1.CLP_Log_ID = cln.CLP_Log_id "
                        + "inner join Profile p on p.Client_No = pr.client_no "
                        + "inner join Administrator ad on ad.u_id = cln1.CLP_SendBy_UID "
                        + "left join brdirt bd on bd.BrCd = left(pr.brwr_key, 4) "
                        + "where pr.CLP_SendTO_UID = '" + u_id + "' AND pr.CLP_Status_ID in (2,4,8,9,11,12,13,15,17) "
                        + "order by pr.cpno";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public string get_email(int u_id)
        {
            st = "Select u_id,First_Name + ' '+ ISNULL(Last_Name,'')[UserName],email from Administrator where (u_id=" + u_id + " and Status = 1) ";
            db.OpenConn();
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            dt = new DataTable();
            this.db.Da.Fill(dt);
            db.CloseConn();
            return dt.Rows[0]["email"].ToString();
        }
        public int get_Created_by(string cpno, string client_no, int clp_status_id)
        {
            int created_by;
            DataTable dt_Created = new DataTable();
            st = "";
            st = "Select Created_by,client_no,clp_status_id from Proposalinfo where cpno= '" + cpno + "' ";
            Start_Method();
            dt = new DataTable();
            this.db.Da.Fill(dt_Created);
            db.CloseConn();
            client_no = dt_Created.Rows[0]["client_no"].ToString();
            created_by = Convert.ToInt32(dt_Created.Rows[0]["Created_by"].ToString());
            clp_status_id = Convert.ToInt32(dt_Created.Rows[0]["clp_status_id"].ToString());
            return created_by;
        }
        public DataTable Market_Checks(string cpno)
        {

            this.cpno = cpno;
            Start_Method();
            st = " Select case when Credit_Report=1 then 'Satisfactory' when Credit_Report=2 then 'Un-Satisfactory'  end[Credit_Report], Comp_Prof, "
                        + "case when Market_Check=1 then 'Satisfactory' when Market_Check=2 then 'Un-Satisfactory' end [Market_Check],Credit_Rep_Source,Market_Check_Source "
                        + "from CIBReport c "
                        + "inner join Hist_Comments hc on hc.cpno = c.cpno  "
                        + "where c.cpno=" + cpno + "";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataSet show_tracker()
        {
            Start_Method();
            st = "Select Top 1  cpno,name,br.BrName[Branch],ar.BrName[Area],p.Cr_Date,First_Name +' '+ ISNULL(last_name,'')[Relationship_Manager],p.CLP_Status,DATEDIFF(Day,cr_date,CLP_SendDate)[Total_Days],rat.Final_Rating, "
                        + " case when rat.brwr_type =1 then 'Corporate' when rat.brwr_type =2 then 'Commercial' when rat.brwr_type =3 then 'SE' when rat.brwr_type =8 then 'ME' when rat.brwr_type =5 then 'Agri' end [brwr_type] "
                        + " from ProposalInfo p "
                        + " inner join profile pf on pf.Client_No = p.client_no "
                        + " inner join (Select max(yr)yr,client_no "
                        + "             from Brwr_Rating br "
                        + "            where authorized_by is not null "
                        + "            group by client_no) br1 on br1.client_no = pf.Client_No "
                        + " inner join Brwr_Rating rat on rat.client_no = br1.client_no and rat.yr <= CLP_SendDate "
                        + " left join Brdirt br on br.brcd = LEFT(p.brwr_key, 4) "
                        + " left join Administrator RM on RM.u_id = p.u_id "
                        + " left join Brdirt ar on ar.brcd = p.Created_By "
                        + " where cpno = 24947 "
                        + " order by rat.yr desc "
                         + "  "
                         + " Select ROW_NUMBER() Over (order by CLP_Log_ID desc)[Sr.no],s.First_Name +' ' + ISNULL(s.last_name,'')[Send By],CLP_SendDate,Days_Pending,r.First_Name +' ' + ISNULL(r.last_name,'')[Send To],st.CLP_Status "
                         + " from CLP_Logs_New cln "
                         + " inner join administrator s on s.u_id = cln.CLP_SendBy_UID "
                         + " inner join administrator r on r.u_id = cln.CLP_SendTo_UID "
                         + " inner join CIIRS_Admin.dbo.tbl_eCLP_Statuses st on st.CLP_Status_ID = cln.CLP_Status_ID "
                         + " where cpno = 29406 "
                         + " order by CLP_Log_ID desc ";
            ds = new DataSet();
            End_Method();
            return ds;
        }
        public DataSet page2_ppg(string cpno)
        {
            Start_Method();
            st = "Select case when DAC =1 then 'No' when DAC= 2 then 'Yes'else 'N/A' end [DAC], "
                    + "             Case when Audit_Obj = 1 then 'No'when Audit_Obj = 1 then 'Yes' else 'N/A' end[Audit_Obj], "
                    + " 			case when PR = 1 then 'No'when PR = 1 then 'Yes'else 'N/A' end[PR], "
                    + " 			case when Product_Policy = 1 then 'No'when Product_Policy = 2 then 'Yes'else 'N/A' end[Product_Policy], "
                    + " 			Case when Last_Markup = 1 then 'No'when Last_Markup = 2 then 'Yes'else 'N/A' end[Last_Markup], "
                    + " 			Case when Pattern_Markup = 1 then 'Within 15 days of due date'when Pattern_Markup = 2 then 'Within 30 days of due date' when Pattern_Markup = 3 then 'Above 30 days of due date' else 'N/A' end[Pattern_Markup], "
                    + " 			Case when Account_Cleanup = 1 then 'No'when Account_Cleanup = 2 then 'Yes'else 'N/A' end[Account_Cleanup], "
                    + " 			case when brwr_type_Group = 4 then 'Pattern of Profit/Installment paid for last 4 quarters' else 'Pattern of Mark-up/Installment paid for last 4 quarters' end[Pattern_Markup_head], "
                    + " 			case when brwr_type_Group = 4 then 'Last Profit / Installment Recieved 'else 'Last Markup / Installment Recieved' end[Last_Markup_head],h.Fin_perf_Comp,h.Comp_Prof,h.Oth_Info,teb.Visit_Summary,teb.Date_of_Visit_Business,teb.Date_of_Visit_Mort_Prop,teb.Visiting_Officer_Name, "
                    + " 				case when Cr_Date < '08/09/2017' then "
                    + "                 (case when review = 1 then 'New' "
                    + "                 when review = 2 then 'Renewal' when review = 3 then 'Revision' "
                    + "                 when review = 4 then 'Restructuring / Settlement' "
                    + "                 when review = 5 then 'Re-activation of Relationship' "
                    + "                 when review = 10 then 'Enhancement' "
                    + "                 when review = 11 then 'Renewal / Enhancement' "
                    + "                 when review = 9 then 'BTF' when review = 8 then pf.other else 'Others' end) "
                    + "                 else (case when review = 1 then 'Fresh' "
                    + "                 when review = 2 then 'Renewal' when review = 3 then 'Revision' "
                    + "                 when review = 4 then 'Restructuring / Settlement' "
                    + "                 when review = 6 then 'Reduction' "
                    + "                 when review = 7 then 'Renewal / Reduction' "
                    + "                 when review = 10 then 'Enhancement' "
                    + "                 when review = 11 then 'Renewal / Enhancement' end)end[review],tet.name,teb.Visit_Summary "
                    + "                 from tbl_eCLP_Acct_Turnover tea "
                    + " inner join profile p on p.client_no = tea.Client_No "
                    + " inner join Hist_Comments h on h.cpno = tea.CPNO "
                    + " inner join tbl_eCLP_Business_Visit teb on teb.CPNO = tea.CPNO "
                    + " inner join ProposalInfo pf on pf.cpno = tea.cpno "
                    + " inner join tbl_eCLP_Type tet on tet.ID = pf.eCLP_type "
                    + " where tea.cpno = '" + cpno + "' "
                    + "  "
                    + " select top 4 bs.yr[fiancial_year],year(bs.yr) as yr ,IsNull(net_inc, 0) as net_inc,IsNull(inc_net_sal, 0) as inc_net_sal,IsNull(tot_asts, 0) as tot_asts,IsNull(tot_liab, 0) as tot_liab,IsNull(paidup_capital, 0) as paidup_capital,IsNull(total_Networth, 0) as total_Networth, IsNull(ret_earn, 0) as ret_earn,IsNull(reserves, 0) as reserves, "
                    + " IsNull(o_prft,0) as o_prft,isnull(round((gross_prft / case when  inc_net_sal <> 0  then inc_net_sal end),3),0)*100 as g_margin,IsNull(net_inc,0) as net_inc,isnull(round((o_prft / case when  inc_net_sal <> 0  then inc_net_sal end),3),0)*100 as o_margin,"
                    + " isnull(round((EQM_NIncLoss / case when  inc_net_sal <> 0  then inc_net_sal end),3),0)*100 as net_margin,IsNull(lng_trm_sen_debt,0) as lng_trm_sen_debt, "
                    + " IsNull(net_inc,0) as net_inc,IsNull(inc_net_sal,0) as inc_net_sal,IsNull(tot_asts,0) as tot_asts,IsNull(tot_liab,0) as tot_liab, IsNull(paidup_capital,0) as paidup_capital,IsNull(total_Networth,0) as total_Networth,IsNull(ret_earn,0) as ret_earn,IsNull(reserves,0)  as reserves, "
                    + " IsNull(sht_trm_debt,0) as sht_trm_debt,isnull(round(Cash_Flow_Opns,3),0) as Net_Cash,isnull(round((tot_liab/ case when  tot_asts <> 0  then tot_asts end),3),0) as leverage, "
                    + " case when tot_cur_liab is null or tot_cur_liab = 0 then 0 else   tot_cur_asts/tot_cur_liab end  as cur_ratio,case when inc_int=0 then 0 else isnull(round((o_prft /case when   inc_int <> 0  then  inc_int end),3),0) end [Interest_cov_ratio], "
                    + " IsNull(net_inc,0) as net_inc,IsNull(inc_net_sal,0) as inc_net_sal,IsNull(tot_asts,0) as tot_asts,IsNull(tot_liab,0) as tot_liab,IsNull(paidup_capital,0) as paidup_capital,IsNull(total_Networth,0) as total_Networth,IsNull(ret_earn,0) as ret_earn,IsNull(reserves,0)  as reserves, "
                    + " case when bs.audit = 1 then 'Audited' when bs.audit = 2 then 'Un-Audited' when bs.audit = 3 then 'Audited by Unrated Company' end[audit],(lng_trm_sen_debt + lng_fin_leas_oblig + sht_trm_debt + lng_trm_debt) / (total_NetWorth)[Gearing_Ratio] "
                    + " from bal_sheet bs "
                    + " left join Cash_Flows cf on cf.Client_No = bs.Client_No and bs.yr = cf.yr "
                    + " where bs.client_no = (Select Client_No from ProposalInfo where cpno = '" + cpno + "') "
                    + " order by bs.yr desc "
                    + "  "
                    + " Select sec_Exist, Sec_prop from ProposalInfo where cpno = '" + cpno + "' "
                    + "  ";
            ds = new DataSet();
            End_Method();
            return ds;
        }
        public DataTable show_Eioms(string client_no)
        {

            this.client_no = client_no;
            Start_Method();
            st = "Select Row_Number() over (order by iom_id desc)[SNo], Iom_id,IOM_Status,IOM_Subject "
                        + " from tbl_eCLP_IOM "
                        + " where client_no = '" + client_no + "' "
                        + " order by iom_id desc";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_cpno(int iom_id)
        {

            this.iom_id = iom_id;
            Start_Method();
            st = "Select client_no,cpno,iom_id "
                        + " from tbl_eCLP_IOM "
                        + " where iom_id = '" + iom_id + "'";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_eclpType(string cpno)
        {
            Start_Method();
            st = "Select * "
                        + " from proposalinfo "
                        + " where cpno = '" + cpno + "'";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_eclpFormats(int eclp_type)
        {
            Start_Method();
            st = "Select eclpTypeDet,link "
                        + " from CIIRS_Admin.dbo.tbl_eclp_Formats "
                        + " where eclp_type = '" + eclp_type + "' order by ID";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public bool get_eclpStatus(string cpno)
        {
            Start_Method();
            st = " Select CLP_Log_ID from clp_logs_new where CLP_Status_ID in (2,3,5) and cpno = ' " + cpno + " '";
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            dt = new DataTable();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            st = "";
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        public DataTable CD_Comments(string cpno)
        {
            Start_Method();
            st = "Select * from eclp_MIS.dbo.termsconditions "
                        + " where cpno = '" + cpno + "' order by ID";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int updateECLPCharges(string client_no, string cpno, int CA, int CW, double CAA, double CPA)
        {
            int vRes;
            Start_Method();
            st = " Update tbl_eIOM_Request_Detail "
                + " Set eCLP_Charges_Applicable= '" + CA + "' ,eCLP_Charges_Waiver='" + CW + "',eCLP_auto_charges = " + CAA + ",eCLP_BU_charges = " + CPA + " "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertECLPCharges(string client_no, string cpno, int CA, int CW, double CAA, double CPA)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO tbl_eIOM_Request_Detail(Request_ID, IOM_ID, cpno, client_no, eCLP_auto_charges, eCLP_BU_charges, eCLP_Charges_Waiver, eCLP_Charges_Applicable) "
                    + "VALUES(27,''," + cpno + "," + client_no + "," + CAA + "," + CPA + "," + CA + "," + CW + ")";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable checkCharges(string cpno)
        {
            this.cpno = cpno;
            Start_Method();
            st = "select client_no,cpno,eCLP_Charges_Applicable,eCLP_Charges_Waiver,eCLP_auto_charges,eCLP_BU_charges,Request_ID from tbl_eIOM_Request_Detail where cpno='" + cpno + "' order by Request_ID";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable Hist_comments(string cpno)
        {
            this.cpno = cpno;
            Start_Method();
            st = "Select * from Hist_Comments where cpno= " + cpno + "";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int updateTransDet(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Trans_Detail= '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertTransDet(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Trans_Detail) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable getIncStatData(string client_no, string yr)
        {
            Start_Method();//formatnumber(ccur(inc_op_net_wrth) + ccur(tot_add) - ccur(inc_dividend),3)
            st = " Select * ,case when audit = 1 then 'Audit' when audit = 2 then 'UnAudited' when audit = 3 then 'Audited By UnRated Company' end [audit_text], "
                        + " case when fin_stat = 1 then 'Unqualified' when fin_stat = 2 then 'Qualified' when fin_stat = 3 then 'Disclaimer' when fin_stat = 4 then 'Adverse' when fin_stat = 5 then 'Projected' end [fin_stat_text],  "
                        + " isnull(prft_extra,0)+isnull(inc_pri_yr_adj,0)-isnull(inc_pri_yr_adj_loss,0)[tot_net_inc],isnull(inc_op_net_wrth,0)+isnull(tot_add,0)-isnull(inc_dividend,0)[tot_end_wrth] "//formatnumber(ccur(prft_extra) + ccur(inc_pri_yr_adj) - ccur(inc_pri_yr_adj_loss),3)
                        + " from bal_sheet "
                        + " where client_no = " + client_no + " and yr = Convert(Varchar(20),'" + yr + "',120)";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int InsertIncStatData(string client_no, string yr)
        {
            int vRes;
            Start_Method();//formatnumber(ccur(inc_op_net_wrth) + ccur(tot_add) - ccur(inc_dividend),3)
            st = " Insert";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable showProfData(string client_no)
        {

            this.client_no = client_no;
            Start_Method();
            st = " Select * from profile where client_no = " + client_no + " ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable adjustedHist(string client_no)
        {

            this.client_no = client_no;
            Start_Method();
            st = " select * from Adjusted_History where client_no= " + client_no + " order by H_ID desc";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int InsertAdjHistData(string client_no, int Active, string date, string u_id, double amt_OS, double amt_wo, double amt_waiv, string reason, string auth_level, double part_adj)
        {
            int vRes;
            Start_Method();//formatnumber(ccur(inc_op_net_wrth) + ccur(tot_add) - ccur(inc_dividend),3)
            st = " Insert into "
                        + " Adjusted_History( Client_No, System_Report_Date, Active, Adj_Date, u_id, Amount_OS, Amount_Writeoff, Amount_Waiver, Reason, Authority_Level, Partial_Adjustment) "
                        + " VALUES(" + client_no + ",GetDate()," + Active + ",'" + date + "'," + u_id + "," + amt_OS + "," + amt_wo + "," + amt_waiv + ",'" + reason + "','" + auth_level + "'," + part_adj + ")";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();

            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        //public int UpdateAdjHistData(string client_no, int Active, string date,string u_id,double amt_OS,double  amt_wo, double amt_waiv, string reason,string auth_level, double part_adj)
        //{
        //    int vRes;
        //    db.OpenConn();
        //    this.db.Cmd.CommandType = CommandType.Text;
        //    this.db.Cmd.CommandTimeout = 0;//formatnumber(ccur(inc_op_net_wrth) + ccur(tot_add) - ccur(inc_dividend),3)
        //    st = "UPDATE       Adjusted_History "
        //                + " SET Client_No =" + client_no + ", System_Report_Date = GetDate(), Active =" + Active + ", Adj_Date ='" + date + "', u_id ='" + u_id + "', Amount_OS ='" + amt_OS + "', Amount_Writeoff '" + amt_wo + "'=, Amount_Waiver ='" + amt_waiv + "', Reason ='" + reason + "', Authority_Level =, Partial_Adjustment ='" + part_adj + "' "
        //                + " where client_no = " + client_no + " ";
        //    this.db.Cmd.CommandText = st;
        //    //this.db.Da.SelectCommand = this.db.Cmd;
        //    //dt = new DataTable();
        //    vRes = db.cmd.ExecuteNonQuery();
        //    db.CloseConn();
        //    st = "";
        //    return vRes;
        //}
        //public int updateProfData(string client_no, int Active)
        //{
        //    int vRes;
        //    db.OpenConn();
        //    this.db.Cmd.CommandType = CommandType.Text;
        //    this.db.Cmd.CommandTimeout = 0;//formatnumber(ccur(inc_op_net_wrth) + ccur(tot_add) - ccur(inc_dividend),3)
        //    st = " Update"
        //                + " Set Active ="+ Active + ", Date_Adjust=GetDate() "
        //                + " where client_no = " + client_no + " ";
        //    this.db.Cmd.CommandText = st;
        //    //this.db.Da.SelectCommand = this.db.Cmd;
        //    //dt = new DataTable();
        //    vRes = db.cmd.ExecuteNonQuery();
        //    db.CloseConn();
        //    st = "";
        //    return vRes;
        //}
        public int updateProfData(string client_no, int Active)
        {
            int vRes;
            Start_Method();
            st = " Update Profile "
                        + " Set Active =" + Active + ", Date_Adjust=GetDate() "
                        + " where client_no = " + client_no + " ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updatebrwr_key(string client_no, string brwr_key)
        {
            int vRes;
            Start_Method();
            st = " Update Profile  Set brwr_key ='" + brwr_key + "'  where client_no = " + client_no + " ";
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int UpdateClientStat(int active, string client_no, string date, string u_id, double amt_OS, double amt_wo, double amt_waiv, string reason, string auth_level, double part_adj)
        {
            int ind;
            this.client_no = client_no;
            //db = new dbAccess();
            //string strConnString = db.Cn.ConnectionString; // get it from Web.config file  
            SqlTransaction objTrans = null;
            using (SqlConnection objConn = new SqlConnection(db.Cn.ConnectionString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction("Trans");
                SqlCommand objCmd1 = new SqlCommand("Update Profile Set Active =" + active + ", Date_Adjust=GetDate() where client_no = " + client_no + "", objConn, objTrans);
                SqlCommand objCmd2 = new SqlCommand("insert into AdjustedHistory( Client_No, System_Report_Date, Active, Adj_Date, u_id, Amount_OS, Amount_Writeoff, Amount_Waiver, Reason, Authority_Level, Partial_Adjustment) VALUES(" + client_no + ",GetDate()," + active + ",'" + date + "'," + u_id + "," + amt_OS + "," + amt_wo + "," + amt_waiv + ",'" + reason + "','" + auth_level + "'," + part_adj + ")", objConn, objTrans);
                try
                {
                    objCmd1.ExecuteNonQuery();
                    objCmd2.ExecuteNonQuery();
                    // Throws exception due to foreign key constraint  
                    objTrans.Commit();
                    ind = 1;
                }
                catch (Exception ex)
                {
                    objTrans.Rollback();
                    objTrans.Dispose();
                    ind = 0;
                }
                finally
                {
                    objConn.Close();
                }
                return ind;
            }
        }
        public DataTable pendAuthClient(string u_id)
        {

            this.u_id = u_id;
            Start_Method();
            st = " select Distinct ROW_NUMBER() OVER (ORDER BY p.Client_no,br.yr)[Sno], p.Client_no,name,isnull(BrName,'Branch Not Updated')[Branch],isnull(ad.First_Name +' '+ISNULL(ad.last_name,''),'')[Inputter],br.yr "
                        + " from profile P "
                        + " inner join brwr_rating Br on br.client_no = p.Client_No "
                        + " left join Administrator ad on ad.u_id = br.u_id "
                        + " left join Brdirt bd on bd.BrCd = LEFT(p.brwr_key,4) "
                        + " where P.brwr_type_group = 3 and  "
                        + " 		P.created_by in (1550,1560,1570,1580,1590,1600,1610,1200,1790,1030,1565,1050,1060,1090,1230,990)  and  "
                        + " 		P.active in (1,2)  AND "
                        + " 		Br.authorized_by_CD is NULL AND "
                        + " 		NOT Br.authorized_by is NULL AND  "
                        + " 		(Br.Fin_Input_Date >= '03/03/2014') "
                        + "order by P.client_no,br.yr";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable loadEiomMasterReq()
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.Text;
            this.db.Cmd.CommandTimeout = 0;
            st = " Select *  "
                        + " from CIIRS_admin.dbo.tbl_eIOM_Master_Request_Types "
                        + " where M_Request_ID not in (2) "
                        + " order by M_ID ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable loadSubReq(int M_Req)
        {
            Start_Method();
            st = " Select *  "
                        + " from CIIRS_admin.[dbo].[tbl_eIOM_Request_Types] "
                        + " where M_Request_id = " + M_Req + " "
                        + " order by ID ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable loadCurReq(int Req_ID)
        {
            Start_Method();
            st = " Select *  "
                        + " from CIIRS_admin.[dbo].[tbl_eIOM_Request_Types] "
                        + " where Request_id = " + Req_ID + " "
                        + " order by ID ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable ShowCurReq(int iom_id, int Req_ID)
        {
            Start_Method();
            st = " Select Request_Detail ,Request_type,M_Request_Type,isnull(Request_Caveats, '-')[Request_Caveats]  "
                        + " from tbl_eCLP_IOM_Requests req "
                        + " inner join CIIRS_ADmin.dbo.tbl_eIOM_Request_Types reqtyp on reqtyp.Request_ID = req.Request_ID "
                        + " inner join CIIRS_admin.dbo.tbl_eIOM_Master_Request_Types Mreqtyp on Mreqtyp.M_ID = reqtyp.M_Request_ID "
                        + " where iom_id = " + iom_id + " and reqtyp.Request_ID = " + Req_ID + " ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable retFacility()
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.Text;
            this.db.Cmd.CommandTimeout = 0;

            st = "Select * from NAT "
                      + "where  active =1 "
                      + "order by ntt";


            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable retMainLimits(string cpno, string wc, string stat)
        {
            this.cpno = cpno;
            Start_Method();
            st = "Select ROW_NUMBER() OVER(ORDER BY fbfac1 ) AS SNO, fbfac1,client_no,NAT.ntt,id "
                        + "from Recommendation rec "
                        + "inner join NAT on NAT.CDN = rec.fbfac1 and stat in (" + stat + ") "
                        + "where cpno= " + cpno + " and sta1 in " + wc + " "
                        + "order by fbfac1";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataSet FacParameters()
        {
            Start_Method();
            st = "select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 7 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 8 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 9 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 10 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 11 "
                        + " order by FRS_Fac_ID_Detail "
                        + "  "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 1 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 2 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 3 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 4 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 5 "
                        + " order by FRS_Fac_ID_Detail "
                        + " "
                        + " select * from "
                        + " CIIRS_Admin.dbo.FRS_Facility_Detail where FRS_Fac_ID = 6 "
                        + " order by FRS_Fac_ID_Detail"
                        + " ";
            ds = new DataSet();
            End_Method();
            return ds;
        }
        public DataTable RetFacParH()
        {
            Start_Method();
            st = "select * from CIIRS_Admin.dbo.FRS_Facility_Master order by FRS_Fac_ID";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable getFailityDet(int id, int dep_id)
        {
            Start_Method();
            if (dep_id == 0)
                st = "Select * from Recommendation "
                    + " inner join NAT on NAT.CDN = fbfac1 "
                    + " where id = " + id;
            else
                st = "Select * from ECLP_Mis.dbo.approva where id = " + id;
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int insertFacilityDet(int dep_id, string client_no, string cpno, int fbfac1, int sta1)
        {
            int vRes;
            Start_Method();
            if (dep_id == 0)
                st = "Insert Into "
                    + " Recommendation(client_no,brwr_key,cpno,fbfac1,sta1) "
                    + " Values(" + client_no + ",''," + cpno + "," + fbfac1 + "," + sta1 + ") ";
            else
                st = "Insert Into "
                    + " ECLP_Mis.dbo.approva(client_no,brwr_key,cpno,fbfac1,sta1) "
                    + " Values(" + client_no + ",''," + cpno + "," + fbfac1 + "," + sta1 + ") ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int UpdateFacilityDet(int dep_id, int id, string client_no, string cpno, int fbfac1, int sta1)
        {
            int vRes;
            Start_Method();
            if (dep_id == 0)
                st = " Update Recommendation Set "
                    + " client_no=" + client_no + ",fbfac1=" + fbfac1 + ",sta1 = " + sta1 + " "
                    + " Where ID = id" + id + " and cpno = " + cpno + "";
            else
                st = " Update ECLP_Mis.dbo.approva Set "
                    + " client_no=" + client_no + ",fbfac1=" + fbfac1 + ",sta1 = " + sta1 + " "
                    + " Where ID = id" + id + " and cpno = " + cpno + "";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataSet FacParametersData(string cpno, int fbfac1, int id)
        {
            Start_Method();
            st = "Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 7 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 8 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 9 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 10 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 11 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + "  "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 1 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 2 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 3 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 4 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 5 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " "
                        + " Select * from Facility_Pts "
                        + " where FRS_Fac_ID = 6 and cpno = " + cpno + " and Fac_id = " + fbfac1 + " and ID = " + id + ""
                        + " ";
            ds = new DataSet();
            End_Method();
            return ds;
        }
        public DataTable getFacilityPTS(int ID)
        {
            Start_Method();
            st = "select * from CIIRS_Admin.dbo.FRS_Facility_Detail"
                        + " where FRS_Fac_ID_Detail = " + ID + " ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public Boolean getFacilityWisePTS(int ID, int ffi, int ffid)
        {
            Start_Method();
            st = "select * from Facility_Pts"
                        + " where id = " + ID + " and FRS_Fac_ID = " + ffi + " and FRS_Fac_ID_Detail = " + ffid + "";
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            dt = new DataTable();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                st = "";
                return false;
            }
        }
        public int updateFacPts(int ID, string client_no, string cpno, int fbfac1, int FacType, int FRS_Fac_ID, int FRS_Fac_ID_Detail, int pts)
        {
            int vRes;
            Start_Method();
            st = " Update Facility_Pts "
                + " Set FRS_Fac_ID_Detail = " + FRS_Fac_ID_Detail + ", FRS_Fac_Pts = " + pts + "   "
                + " Where ID = " + ID + " and Fac_id = " + fbfac1 + " and FRS_Fac_ID = " + FRS_Fac_ID + "";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertFacPts(int ID, string client_no, string cpno, int fbfac1, int FacType, int FRS_Fac_ID, int FRS_Fac_ID_Detail, int pts)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Facility_Pts( ID, Client_no, Brwr_key, cpno, Fac_ID, Fac_Type, FRS_Fac_ID, FRS_Fac_ID_Detail, FRS_Fac_Pts) "
                    + "VALUES(" + ID + "," + client_no + ",''," + cpno + "," + fbfac1 + "," + FacType + "," + FRS_Fac_ID + "," + FRS_Fac_ID_Detail + "," + pts + ")";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable getUser_Com(string cpno, string u_id)
        {
            this.cpno = cpno;
            this.u_id = u_id;
            Start_Method();
            st = "Select * FROM tbl_eIOM_Recom where cpno= " + cpno + " and u_id = " + u_id + "";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int updateHistWithBanks(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Hist_With_Banks= '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertHistWithBanks(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Hist_With_Banks) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateIndusProf(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Indus_Prof= '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertIndusProf(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Indus_Prof) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateMgmtComm(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Mgmt_Com= '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertMgmtComm(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Mgmt_Com) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateoth_info(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set oth_info= '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertoth_info(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, oth_info) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updatefin_Perf_Com(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set fin_Perf_Comp = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertfin_Perf_Com(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, fin_Perf_Comp) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateBg_Prop_Part_Dir(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Bg_Prop_Part_Dir = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertBg_Prop_Part_Dir(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Bg_Prop_Part_Dir) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateCurrent_Request_Justification(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Current_Request_Justification = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertCurrent_Request_Justification(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Current_Request_Justification) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateComp_prof(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Comp_prof = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertComp_prof(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Comp_prof) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateeCIB_comm(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set eCIB_comm = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int inserteCIB_comm(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, eCIB_comm) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateCIIRS_Rating_comm(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set CIIRS_Rating_comm = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertCIIRS_Rating_comm(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, CIIRS_Rating_comm) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updatecredit_opinion(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set credit_opinion = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertcredit_opinion(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, credit_opinion) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateRat_param_com(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Rat_param_com = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertRat_param_com(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Rat_param_com) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateExp_Def_com(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Exp_Def_com = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertExp_Def_com(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Exp_Def_com) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateOther_Request(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Other_Request = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertOther_Request(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Other_Request) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateTrans_Terms(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Trans_Terms = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertTrans_Terms(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Trans_Terms) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateProj_Details(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Proj_Details = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertProj_Details(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Proj_Details) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateEnv_Cons(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Env_Cons = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertEnv_Cons(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Env_Cons) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateComp_Land(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Comp_Land = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertComp_Land(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Comp_Land) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateInv_cons(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Inv_cons = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertInv_cons(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Inv_cons) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateSwot_Analysis(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Swot_Analysis = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertSwot_Analysis(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Swot_Analysis) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updatePricing(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Pricing = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertPricing(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Pricing) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateSec_Doc(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Sec_Doc = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertSec_Doc(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Sec_Doc) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateSec_Analysis(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Sec_Analysis = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertSec_Analysis(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Sec_Analysis) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateAudit_Obs(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Audit_Obs = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertAudit_Obs(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Audit_Obs) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateRel_oth_fin(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Rel_oth_fin = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertRel_oth_fin(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Rel_oth_fin) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateRel_Stra(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set Rel_Stra = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertRel_Stra(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, Rel_Stra) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updatePR_Compl(string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = " Update Hist_Comments "
                + " Set PR_Compl = '" + TransDet + "' "
                + " where cpno = '" + cpno + "'";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertPR_Compl(string client_no, string cpno, string TransDet)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + "INTO Hist_Comments(Client_no, cpno, PR_Compl) "
                    + "Values(" + client_no + "," + cpno + ",'" + TransDet + "') ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable get_CollDet(string cpno, string dep_flag)
        {
            Start_Method();
            if (dep_flag == "1")
                st = "Select Row_Number() over (order by coll.Coll_ID)[SNo],Client_No,cpno,coll.Coll_Name,coll.Coll_ID,cd.ID ,Col_Val_exist,Col_ID_exist "
                        + " from eCLP_MIS.dbo.Collateral_Detail_Exist cd "
                        + " inner join CIIRS_Admin.dbo.Collateral_types_RWA coll on coll.Coll_ID = Col_ID_exist "
                        + " where cpno = " + cpno + " "
                        + " order by coll.Coll_ID ";
            else
                st = "Select Row_Number() over (order by coll.Coll_ID)[SNo],Client_No,cpno,coll.Coll_Name,coll.Coll_ID,cd.ID, Col_Val_exist,Col_ID_exist "
                        + " from Collateral_Detail_Exist cd "
                        + " inner join CIIRS_Admin.dbo.Collateral_types_RWA coll on coll.Coll_ID = Col_ID_exist "
                        + " where cpno = " + cpno + " "
                        + " order by coll.Coll_ID ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int DeleteSecurity(string id, string cpno, string dep_flag)
        {
            int vRes;
            Start_Method();
            if (dep_flag == "1")
                st = "Delete from eCLP_MIS.dbo.Collateral_Detail_Exist where id = " + id + " and cpno = " + cpno + " ";
            else
                st = "Delete from Collateral_Detail_Exist where id = " + id + " and cpno = " + cpno + " ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable get_CollDet_Prop(string cpno, string dep_flag)
        {
            Start_Method();
            if (dep_flag == "1")
                st = "Select Row_Number() over (order by cpno)[SNo],Client_No,cpno,coll.Coll_Name,coll.Coll_ID,cd.ID,Col_Val_Prop "
                        + " from eCLP_MIS.dbo.Collateral_Detail_Prop cd "
                        + " inner join CIIRS_Admin.dbo.Collateral_types_RWA coll on coll.Coll_ID = Col_ID_Prop "
                        + " where cpno = " + cpno + " "
                        + " order by cpno ";
            else
                st = "Select Row_Number() over (order by cpno)[SNo],Client_No,cpno,coll.Coll_Name,coll.Coll_ID,cd.ID,Col_Val_Prop "
                        + " from Collateral_Detail_Prop cd "
                        + " inner join CIIRS_Admin.dbo.Collateral_types_RWA coll on coll.Coll_ID = Col_ID_Prop "
                        + " where cpno = " + cpno + " "
                        + " order by cpno ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int DeleteSecurity_Prop(string id, string cpno, string dep_flag)
        {
            int vRes;
            Start_Method();
            if (dep_flag == "1")
                st = "Delete from eCLP_MIS.dbo.Collateral_Detail_Prop where id = " + id + " and cpno = " + cpno + " ";
            else
                st = "Delete from Collateral_Detail_Prop where id = " + id + " and cpno = " + cpno + " ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable get_FacAgProp(string cpno, string dep_flag)
        {
            Start_Method();
            if (dep_flag == "1")
            {
                st = "Select Row_Number() over (order by id)[SNo],fbfac1,NTT "
                        + " from eCLP_MIS.dbo.Approva r "
                        + " inner join NAT  on NAT.CDN = r.fbfac1 "
                        + " where cpno = " + cpno + " ";
            }
            else
            {
                st = "Select Row_Number() over (order by id)[SNo],fbfac1,NTT "
                           + " from Recommendation r "
                           + " inner join NAT  on NAT.CDN = r.fbfac1 "
                           + " where cpno = " + cpno + " ";
            }
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataSet get_SecParam(string cpno)
        {
            Start_Method();
            st = " Select * from CIIRS_Admin.dbo.FRS_Security_Master "
                + " order by FRS_Sec_ID "
                + " "
                + " Select * from CIIRS_Admin.dbo.FRS_Security_Detail "
                + " where FRS_Sec_ID = 1 "
                + " order by FRS_Sec_ID_Detail "
                + " "
                + " Select * from CIIRS_Admin.dbo.FRS_Security_Detail "
                + " where FRS_Sec_ID = 2 "
                + " order by FRS_Sec_ID_Detail "
                + " "
                + " Select * from CIIRS_Admin.dbo.FRS_Security_Detail "
                + " where FRS_Sec_ID = 3 "
                + " order by FRS_Sec_ID_Detail "
                + " "
                + " Select * from CIIRS_Admin.dbo.FRS_Security_Detail "
                + " where FRS_Sec_ID = 4 "
                + " order by FRS_Sec_ID_Detail "
                + " "
                + " Select * from CIIRS_Admin.dbo.FRS_Security_Detail "
                + " where FRS_Sec_ID = 5 "
                + " order by FRS_Sec_ID_Detail "
                + " ";
            ds = new DataSet();
            End_Method();
            return ds;
        }
        public DataTable get_ColList()
        {
            Start_Method();
            st = " Select * from CIIRS_Admin.dbo.Collateral_Types_Rwa  "
                + " where coll_status = 1 "
                + " order by coll_name ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_existSecDet(string cpno, string dep_flag, string ID)
        {
            Start_Method();
            if (dep_flag == "1")
                st = " Select * from eCLP_MIS.dbo.Collateral_Detail_Exist  "
                    + " where cpno = " + cpno + " and ID = " + ID + " ";
            else
                st = " Select * from Collateral_Detail_Exist "
                + " where cpno = " + cpno + " and ID = " + ID + " ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataSet SecParametersData(string cpno, int Col_id)
        {
            Start_Method();
            st = "Select * from Security_Pts "
                        + " where FRS_Sec_ID = 1 and cpno = " + cpno + " and Col_id = " + Col_id + " "
                        + " "
                        + " Select * from Security_Pts "
                        + " where FRS_Sec_ID = 2 and cpno = " + cpno + " and Col_id = " + Col_id + " "
                        + " "
                        + " Select * from Security_Pts "
                        + " where FRS_Sec_ID = 3 and cpno = " + cpno + " and Col_id = " + Col_id + " "
                        + " "
                        + " Select * from Security_Pts "
                        + " where FRS_Sec_ID = 4 and cpno = " + cpno + " and Col_id = " + Col_id + " "
                        + " "
                        + " Select * from Security_Pts "
                        + " where FRS_Sec_ID = 5 and cpno = " + cpno + " and Col_id = " + Col_id + " "
                        + " ";
            ds = new DataSet();
            End_Method();
            return ds;
        }
        public DataTable getSecPTS(int ID)
        {
            Start_Method();
            st = "select * from CIIRS_Admin.dbo.FRS_Security_Detail"
                        + " where FRS_Sec_ID_Detail = " + ID + " ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int updateSecPts(string cpno, int Col_id, int FRS_Sec_ID, int FRS_Sec_ID_Detail, int pts)
        {
            int vRes;
            Start_Method();
            st = " Update Security_Pts "
                + " Set FRS_Sec_ID_Detail = " + FRS_Sec_ID_Detail + ", FRS_Sec_Pts = " + pts + "   "
                + " Where cpno = " + cpno + " and Col_ID = " + Col_id + " and FRS_Sec_ID = " + FRS_Sec_ID + "";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertSecPts(string client_no, string cpno, int Col_ID, int FRS_Sec_ID, int FRS_Sec_ID_Detail, int pts)
        {
            int vRes;
            Start_Method();
            st = "INSERT "
                    + " INTO Security_Pts( Client_no,Brwr_key,  cpno, Col_ID, Col_Type, FRS_Sec_ID, FRS_Sec_ID_Detail, FRS_Sec_Pts) "
                    + " VALUES(" + client_no + ",''," + cpno + "," + Col_ID + ",1," + FRS_Sec_ID + "," + FRS_Sec_ID_Detail + "," + pts + ")";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable Get_brwrRatingDet(string client_no)
        {
            Start_Method();
            st = "select * from brwr_rating"
                        + " where client_no = " + client_no + " and authorized_by is not null ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable Get_Def_HisDet(string client_no)
        {
            Start_Method();
            st = "select * from Default_History"
                        + " where client_no = " + client_no + " "
                        + " order by H_id desc ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_busRcvdData(string cpno, string dep_flag, string yr)
        {
            Start_Method();
            if (dep_flag == "1")
                st = "select * from eCLP_MIS.dbo.prudreg_new"
                            + " where cpno = " + cpno + " and Bus_Rcvd_Year = Convert(Varchar(20),'" + yr + "',101)";
            else
                st = "select * from prudreg_new"
                            + " where cpno = " + cpno + " and Bus_Rcvd_Year = Convert(Varchar(20),'" + yr + "',101)";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int updatePrudReg(string dep_flag, string cpno, string yr, string ai, string ae, string all, string ag, string ad, string an, string adcs, string acs, string ar, string ay, string aau, string ao, string other, string aio, string aeo, string com, string rroc, string eocps, string af)
        {
            int vRes;
            Start_Method();
            if (dep_flag == "1")
                st = " Update eCLP_MIS.dbo.prudreg_new "
                        + " SET Amount_Import = " + ai + ", Amount_Export =" + ae + ", Amount_LBDLBP =" + all + ", Amount_Guarantee =" + ag + ", Amount_Deposit =" + ad + ", Amount_Netincome =" + an + ", Amount_DCSummation =" + adcs + ","
                        + " Amount_CreditSummation =" + acs + ", Amount_Remittances =" + ar + ", Amount_Yield =" + ay + ", Amount_Avg_Util =" + aau + ", Amount_Other =" + ao + ", Amount_Future =" + af + ", Other ='" + other + "', System_Date =GetDate(), "
                        + " Amount_Import_Other =" + aio + ", Amount_Export_Other =" + aeo + ",  Commission =" + com + ", Relationship_Roc =" + rroc + ", Earning_on_Cross_Sell_Products =" + eocps + ""
                        + " where cpno = " + cpno + " and Bus_Rcvd_Year = Convert(Varchar(20),'" + yr + "',101)";
            else
                st = " Update prudreg_new "
                    + " SET Amount_Import = " + ai + ", Amount_Export =" + ae + ", Amount_LBDLBP =" + all + ", Amount_Guarantee =" + ag + ", Amount_Deposit =" + ad + ", Amount_Netincome =" + an + ", Amount_DCSummation =" + adcs + ","
                        + " Amount_CreditSummation =" + acs + ", Amount_Remittances =" + ar + ", Amount_Yield =" + ay + ", Amount_Avg_Util =" + aau + ", Amount_Other =" + ao + ", Amount_Future =" + af + ", Other ='" + other + "', System_Date =GetDate(), "
                        + " Amount_Import_Other =" + aio + ", Amount_Export_Other =" + aeo + ",  Commission =" + com + ", Relationship_Roc =" + rroc + ", Earning_on_Cross_Sell_Products =" + eocps + ""
                        + " where cpno = " + cpno + " and Bus_Rcvd_Year = Convert(Varchar(20),'" + yr + "',101)";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int insertPrudReg(string client_no, string dep_flag, string cpno, string yr, string ai, string ae, string all, string ag, string ad, string an, string adcs, string acs, string ar, string ay, string aau, string ao, string other, string aio, string aeo, string com, string rroc, string eocps, string af)
        {
            int vRes;
            Start_Method();
            if (dep_flag == "1")
                st = " Insert "
                        + " INTO eCLP_MIS.dbo.prudreg_new(Client_No, brwr_key, cpno, Bus_Rcvd_Year, Amount_Import, Amount_Export, Amount_LBDLBP, Amount_Guarantee, Amount_Deposit, Amount_Netincome, Amount_DCSummation, Amount_CreditSummation, "
                        + " Amount_Remittances, Amount_Yield, Amount_Avg_Util, Amount_Other, Amount_Future, Other, System_Date, Amount_Import_Other, Amount_Export_Other, Commission, Relationship_ROC, Earning_on_Cross_sell_products) "
                        + " VALUES(" + client_no + ",''," + cpno + "," + yr + "," + ai + "," + ae + "," + all + "," + ag + "," + ad + "," + an + "," + adcs + "," + acs + "," + ar + "," + ay + "," + aau + "," + ao + "," + af + ",'" + other + "',GetDate()," + aio + "," + aeo + "," + com + "," + rroc + "," + eocps + ")";
            else
                st = " Insert "
                        + " INTO prudreg_new(Client_No, brwr_key, cpno, Bus_Rcvd_Year, Amount_Import, Amount_Export, Amount_LBDLBP, Amount_Guarantee, Amount_Deposit, Amount_Netincome, Amount_DCSummation, Amount_CreditSummation, "
                        + " Amount_Remittances, Amount_Yield, Amount_Avg_Util, Amount_Other, Amount_Future, Other, System_Date, Amount_Import_Other, Amount_Export_Other, Commission, Relationship_ROC, Earning_on_Cross_sell_products) "
                        + " VALUES(" + client_no + ",''," + cpno + "," + yr + "," + ai + "," + ae + "," + all + "," + ag + "," + ad + "," + an + "," + adcs + "," + acs + "," + ar + "," + ay + "," + aau + "," + ao + "," + af + ",'" + other + "',GetDate()," + aio + "," + aeo + "," + com + "," + rroc + "," + eocps + ")";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable get_AcctTurnoverData(string cpno, string dep_flag)
        {
            Start_Method();
            if (dep_flag == "1")
                st = "select * from eCLP_MIS.dbo.tbl_eCLP_Acct_Turnover"
                            + " where cpno = " + cpno + "";
            else
                st = "select * from tbl_eCLP_Acct_Turnover"
                            + " where cpno = " + cpno + "";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int insertUserLogs(string u_id, string mu, string Log_ID, string client_no, string cpno, string ai, string ae, string all, string ag, string ad, string an, string adcs, string acs, string ar, string ay, string aau, string ao, string other, string aio, string aeo, string com, string rroc, string eocps, string af)
        {
            int vRes;
            Start_Method();
            st = " Insert "
                + " INTO CIIRS_Admin.dbo.User_Logs_Details( U_ID, Module_Update, Module_Update_Time, Log_ID, Client_No, CpNo) "
                + " VALUES(" + u_id + ",'" + mu + "',GetDate()," + Log_ID + "," + client_no + "," + cpno + ")";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable get_ClientFYs(string client_no, string dep_flag)
        {
            Start_Method();
            if (dep_flag == "1")
                st = " Select client_no,Convert(Varchar(20),yr,101) [yr],Year(yr)[yr_ord],brwr_key, case when audit =1 then Convert(Varchar(20),yr,101) +' - '+ 'Audited'  "
                    + " when audit =2 then Convert(Varchar(20),yr,101) +' - '+ 'Un-Audited' "
                    + " when audit =3 then Convert(Varchar(20),yr,101) +' - '+ 'Audited By Unrated Company' end [audit], "
                    + " Case when authorized_by_CD is not null then 'Authorized' else 'Not Authorized' end [Authorization] "
                    + " from Brwr_Rating "
                    + " where client_no = " + client_no + ""
                    + " order by yr_ord desc ";
            else
                st = " Select client_no,Convert(Varchar(20),yr,101) [yr],Year(yr)[yr_ord],brwr_key, case when audit =1 then Convert(Varchar(20),yr,101) +' - '+ 'Audited'  "
                    + " when audit =2 then Convert(Varchar(20),yr,101) +' - '+ 'Un-Audited' "
                    + " when audit =3 then Convert(Varchar(20),yr,101) +' - '+ 'Audited By Unrated Company' end [audit], "
                    + " Case when authorized_by is not null then 'Authorized' else 'Not Authorized' end [Authorization] "
                    + " from Brwr_Rating "
                    + " where client_no = " + client_no + ""
                    + " order by yr_ord desc ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_BalSheetData(string client_no, string yr)
        {
            Start_Method();
            st = " select * from bal_sheet "
                            + " where client_no = " + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101)";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_AuditorNames()
        {
            Start_Method();
            //st = " select * from CIIRS_Admin.dbo.tbl_Auditor where status <> 0 order by A_Name ";
            st = " select * from CIIRS_Admin.dbo.tbl_Auditor order by A_Name ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_CashFlowData(string client_no, string yr)
        {
            Start_Method();
            st = " select * from Cash_Flows "
                            + " where client_no = " + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101)";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int UpdateCashFlow(string yr, string client_no, string asset_op_lease, string Dep_Expense, string Fin_Charges, string Int_Income, string Gain_Loss_FixAssets, string Fx_Gain_Loss, string Prov_Gratuity, string Non_Cash_Charges, string Sub_total, string CFG_bef_WCC, string Stock_Inv, string Store_Spares, string Trade_Debts, string Loan_Adv, string Short_Term_Prep, string Oth_Recv, string Oth_Curr_Assets, string sub_tot_Inc_Dec_CA, string Ac_payable, string Oth_Curr_Liab, string Acc_Exp, string Others, string sub_tot_Inc_Dec_CL, string tot_chng_WCC, string Cash_from_Opns, string Tax_paid, string Fin_Cost, string Int_Rcv, string Wppf, string Gratuity_Paid, string Lng_Term_Dep, string Others_payments, string sub_tot_oth_payments, string Cash_Flow_Opns, string IA_Disposal_Fixed_Assets, string IA_Fixed_Cap_Expend, string IA_Cap_work_prog, string IA_Lng_Term_Dep, string IA_Lng_Term_Inv, string IA_Oth_Lng_Term_Assets, string IA_Short_Term_Inv, string IA_Oth_Cash_Inflow, string IA_Oth_Cash_outflow, string IA_Net_Cash_Flow, string FA_Issue_Share_Cap, string FA_Repayments, string FA_Lng_Term_Fin, string FA_Loan_Associate_Comp, string FA_Loan_from_Dir, string FA_Short_Term_Fin, string FA_Fin_Lease_Liab, string FA_Sale_Lease_Back, string FA_Div_Paid, string FA_Oth_Cash_Inflow, string FA_Oth_Cash_outflow, string FA_Net_Cash_Flow, string Net_Inc_Dec_Cash_Flow, string Cash_begin_year, string Cash_end_year)
        {
            int ind;
            this.client_no = client_no;
            //db = new dbAccess();
            //string strConnString = db.Cn.ConnectionString; // get it from Web.config file  
            SqlTransaction objTrans = null;
            using (SqlConnection objConn = new SqlConnection(db.Cn.ConnectionString))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction("Trans");
                SqlCommand objCmd1 = new SqlCommand("update Bal_sheet set asset_op_lease = " + asset_op_lease + " where client_no =" + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101)", objConn, objTrans);
                SqlCommand objCmd2 = new SqlCommand("Update Cash_Flows Set Dep_Expense = " + Dep_Expense + ",Fin_Charges = " + Fin_Charges + ", Int_Income = " + Int_Income + ",Gain_Loss_FixAssets = " + Gain_Loss_FixAssets + ",Fx_Gain_Loss=" + Fx_Gain_Loss + ",Prov_Gratuity=" + Prov_Gratuity + ",Non_Cash_Charges =" + Non_Cash_Charges + ",Sub_total=" + Sub_total + ",CFG_bef_WCC= " + CFG_bef_WCC + ",Stock_Inv =" + Stock_Inv + ",Store_Spares=" + Store_Spares + ",Trade_Debts=" + Trade_Debts + ",Loan_Adv=" + Loan_Adv + ",Short_Term_Prep=" + Short_Term_Prep + ",Oth_Recv=" + Oth_Recv + ",Oth_Curr_Assets=" + Oth_Curr_Assets + ",sub_tot_Inc_Dec_CA=" + sub_tot_Inc_Dec_CA + ",Ac_payable=" + Ac_payable + ",Oth_Curr_Liab=" + Oth_Curr_Liab + ",Acc_Exp=" + Acc_Exp + ",Others=" + Others + ",sub_tot_Inc_Dec_CL=" + sub_tot_Inc_Dec_CL + ",tot_chng_WCC=" + tot_chng_WCC + ",Cash_from_Opns=" + Cash_from_Opns + ",Tax_paid=" + Tax_paid + ",Fin_Cost=" + Fin_Cost + ",Int_Rcv=" + Int_Rcv + ",Wppf=" + Wppf + ",Gratuity_Paid=" + Gratuity_Paid + ",Lng_Term_Dep=" + Lng_Term_Dep + ",Others_payments=" + Others_payments + ",sub_tot_oth_payments = " + sub_tot_oth_payments + ",Cash_Flow_Opns=" + Cash_Flow_Opns + ",IA_Disposal_Fixed_Assets=" + IA_Disposal_Fixed_Assets + ",IA_Fixed_Cap_Expend=" + IA_Fixed_Cap_Expend + ",IA_Cap_work_prog=" + IA_Cap_work_prog + ",IA_Lng_Term_Dep=" + IA_Lng_Term_Dep + ",IA_Lng_Term_Inv=" + IA_Lng_Term_Inv + ",IA_Oth_Lng_Term_Assets=" + IA_Oth_Lng_Term_Assets + ",IA_Short_Term_Inv=" + IA_Short_Term_Inv + ",IA_Oth_Cash_Inflow=" + IA_Oth_Cash_Inflow + ",IA_Oth_Cash_outflow=" + IA_Oth_Cash_outflow + ",IA_Net_Cash_Flow=" + IA_Net_Cash_Flow + ",FA_Issue_Share_Cap=" + FA_Issue_Share_Cap + ",FA_Repayments=" + FA_Repayments + ",FA_Lng_Term_Fin=" + FA_Lng_Term_Fin + ",FA_Loan_Associate_Comp=" + FA_Loan_Associate_Comp + ",FA_Loan_from_Dir=" + FA_Loan_from_Dir + ",FA_Short_Term_Fin=" + FA_Short_Term_Fin + ",FA_Fin_Lease_Liab=" + FA_Fin_Lease_Liab + ",FA_Sale_Lease_Back=" + FA_Sale_Lease_Back + ",FA_Div_Paid=" + FA_Div_Paid + ",FA_Oth_Cash_Inflow=" + FA_Oth_Cash_Inflow + ",FA_Oth_Cash_outflow=" + FA_Oth_Cash_outflow + ",FA_Net_Cash_Flow=" + FA_Net_Cash_Flow + ",Net_Inc_Dec_Cash_Flow=" + Net_Inc_Dec_Cash_Flow + ",Cash_begin_year=" + Cash_begin_year + ",Cash_end_year=" + Cash_end_year + " where client_no =" + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101)", objConn, objTrans);
                try
                {
                    objCmd1.ExecuteNonQuery();
                    objCmd2.ExecuteNonQuery();
                    // Throws exception due to foreign key constraint  
                    objTrans.Commit();
                    ind = 1;
                }
                catch (Exception ex)
                {
                    objTrans.Rollback();
                    objTrans.Dispose();
                    ind = 0;
                }
                finally
                {
                    objConn.Close();
                }
                return ind;
            }
        }
        public DataTable getAuthoriz(string client_no, string yr, string dep_flag)
        {
            Start_Method();
            if (dep_flag == "0")
                st = "select * from brwr_rating"
                            + " where client_no = " + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101) and authorized_by is not null ";
            else
                st = "select * from brwr_rating"
                            + " where client_no = " + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101) and authorized_by_CD is not null ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int updateSupInfo(string yr, string client_no, string sup_op_fxd_ast, string sup_land, string sup_fxd_acq, string sup_bldng, string sup_fxd_rev, string sup_mch, string tot_add, string sup_of_eqp, string inc_dep_amr, string sup_ac_gen, string sup_csh_sal, string sup_vhcl, string sup_gan_sal, string sup_oth_eqp, string sup_adj, string sup_wip, string tot_ded, string sup_acm_dep, string tot_nfxd_ast, string tot_net_fxd_ast, string chk, string chk2, string lng_trm_debt, string sup_tax_rate)
        {
            int vRes;
            Start_Method();
            st = " Update update Bal_sheet "
                + " Set sup_op_fxd_ast=" + sup_op_fxd_ast + ",sup_land=" + sup_land + ",sup_fxd_acq=" + sup_fxd_acq + ",sup_bldng=" + sup_bldng + ",sup_fxd_rev=" + sup_fxd_rev + ",sup_mch = " + sup_mch + ",tot_add =" + tot_add + ",sup_of_eqp = " + sup_of_eqp + ",inc_dep_amr =" + inc_dep_amr + ",sup_ac_gen = " + sup_ac_gen + ",sup_csh_sal=" + sup_csh_sal + ",sup_vhcl = " + sup_vhcl + ",sup_gan_sal=" + sup_gan_sal + ",sup_oth_eqp=" + sup_oth_eqp + ",sup_adj=" + sup_adj + ",sup_wip=" + sup_wip + ",tot_ded=" + tot_ded + ",sup_acm_dep=" + sup_acm_dep + ",tot_nfxd_ast =" + tot_nfxd_ast + ",tot_net_fxd_ast =" + tot_net_fxd_ast + ",chk =" + chk + ",chk2=" + chk2 + ",lng_trm_debt =" + lng_trm_debt + ",sup_tax_rate =" + sup_tax_rate + " "
                + " where client_no =" + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101) ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateBalSheet(string yr, string client_no, string cash_mkt_sec, string net_fx_ast, string sht_trm_inv, string lng_trm_dep, string receivables, string def_cost, string inventory, string lng_trm_inv, string store_spares, string lng_trm_loan, string adv_utakings, string oth_lng_trm_ast, string prpaid_exp_dep, string adv_dep, string tax_payments, string oth_recv, string tot_cur_asts, string tot_lng_trm_asts, string tot_asts, string lng_trm_sen_debt, string ac_payable, string lng_fin_leas_oblig, string acrd_mrkp_loan, string lng_red_cap, string acrd_exp, string lng_dir_loan, string tax_payable, string tot_lng_trm_debt, string dir_unsub_loan, string lng_trm_dep2, string wppf, string oth_lng_trm_liab, string adv_asst_utakings, string lng_trm_laib, string oth_cur_liab, string lab_pen_prov, string lng_trm_debt, string oth_def_prov, string tot_cur_liab, string tot_def_laib, string min_int, string def_tax, string Sub_tot, string tot_lng_trm_laib, string tot_liab, string surp_rev_fix_ass, string paidup_capital, string reserves, string ret_earn, string dir_sub_loan, string total_Networth, string tot_liab_eqty)
        {
            int vRes;
            Start_Method();
            st = " Update update Bal_sheet "
                + " Set cash_mkt_sec =" + cash_mkt_sec + ",net_fx_ast =" + net_fx_ast + ",sht_trm_inv =" + sht_trm_inv + ",lng_trm_dep =" + lng_trm_dep + ",receivables =" + receivables + ",def_cost  = " + def_cost + ",inventory  =" + inventory + ",lng_trm_inv  = " + lng_trm_inv + ",store_spares  =" + store_spares + ",lng_trm_loan  = " + lng_trm_loan + ",adv_utakings =" + adv_utakings + ",oth_lng_trm_ast  = " + oth_lng_trm_ast + ",prpaid_exp_dep =" + prpaid_exp_dep + ",adv_dep =" + adv_dep + ",tax_payments =" + tax_payments + ",oth_recv =" + oth_recv + ",tot_cur_asts =" + tot_cur_asts + ",tot_lng_trm_asts =" + tot_lng_trm_asts + ",tot_asts  =" + tot_asts + ",lng_trm_sen_debt  =" + lng_trm_sen_debt + ",ac_payable  =" + ac_payable + ",lng_fin_leas_oblig =" + lng_fin_leas_oblig + ",acrd_mrkp_loan  =" + lng_trm_debt + ",lng_red_cap  =" + lng_red_cap + ",acrd_exp=" + acrd_exp + ",lng_dir_loan =" + lng_dir_loan + ",tax_payable =" + tax_payable + ",tot_lng_trm_debt =" + tot_lng_trm_debt + ",dir_unsub_loan =" + dir_unsub_loan + ",lng_trm_dep2 =" + lng_trm_dep2 + ",wppf =" + wppf + ",oth_lng_trm_liab =" + oth_lng_trm_liab + ",adv_asst_utakings  =" + adv_asst_utakings + ",lng_trm_laib  =" + lng_trm_laib + ",oth_cur_liab  =" + oth_cur_liab + ",lab_pen_prov  =" + lab_pen_prov + ",lng_trm_debt  =" + lng_trm_debt + ",oth_def_prov =" + oth_def_prov + ",tot_cur_liab =" + tot_cur_liab + ",tot_def_laib =" + tot_def_laib + ",min_int =" + min_int + ",def_tax =" + def_tax + ",Sub_tot =" + Sub_tot + ",tot_lng_trm_laib =" + tot_lng_trm_laib + ",tot_liab =" + tot_liab + ",surp_rev_fix_ass =" + surp_rev_fix_ass + ",paidup_capital =" + paidup_capital + ",reserves =" + reserves + ",ret_earn =" + ret_earn + ",dir_sub_loan =" + dir_sub_loan + ",total_Networth =" + total_Networth + ",tot_liab_eqty =" + tot_liab_eqty + " "
                + " where client_no =" + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101) ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateBalSheet_notes(string yr, string client_no, string Asset_Commitments, string asset_gurantees, string Asset_Contingencies, string tot_asts_adj, string op_lease_payments, string Liab_Guarantees, string Liab_Commitments, string Liab_Contingencies, string Contingent_Liabilities, string tot_liab_adj)
        {
            int vRes;
            Start_Method();
            st = " Update update Bal_sheet "
                + " Set Asset_Commitments =" + Asset_Commitments + ",asset_gurantees =" + asset_gurantees + ",Asset_Contingencies =" + Asset_Contingencies + ",tot_asts_adj =" + tot_asts_adj + ",op_lease_payments =" + op_lease_payments + ",Liab_Guarantees  = " + Liab_Guarantees + ",Liab_Commitments  =" + Liab_Commitments + ",Liab_Contingencies = " + Liab_Contingencies + ",Contingent_Liabilities  =" + Contingent_Liabilities + ",tot_liab_adj  = " + tot_liab_adj + " "
                + " where client_no =" + client_no + " and yr = Convert(Varchar(20), '" + yr + "',101) ";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public int updateIncStatData(string client_no, string yr, string inc_net_sal, string inc_int, string inc_cst_gsold, string inc_opr_les_exp, string gross_prft, string inc_adm_gen_exp, string prft_bef_tax, string inc_dep_amr, string inc_tax_cur_pbl, string inc_oth_non_csh, string inc_def_tax, string inc_oth_inc, string inc_pri_yr, string o_prft, string prft_extra, string inc_gain_sal_good, string inc_oth_non_opr_inc, string inc_oth_exp, string inc_int_inc, string inc_pri_yr_adj, string prft_bef_itax, string inc_pri_yr_adj_loss, string net_inc, string Bad_Debts_Amount, string Purchases_Credit_Amount, string inc_op_net_wrth, string tot_add, string tot_net_inc, string inc_dividend, string inc_sal_eqt, string inc_ast_rev, string tot_end_wrth)
        {
            int vRes;
            Start_Method();//formatnumber(ccur(inc_op_net_wrth) + ccur(tot_add) - ccur(inc_dividend),3)
            st = " Update"
                + " Set inc_net_sal =" + inc_net_sal + ",inc_int =" + inc_int + ",inc_cst_gsold =" + inc_cst_gsold + ",inc_opr_les_exp =" + inc_opr_les_exp + ",gross_prft =" + gross_prft + ",inc_adm_gen_exp  = " + inc_adm_gen_exp + ",prft_bef_tax  =" + prft_bef_tax + ",inc_dep_amr  = " + inc_dep_amr + ",inc_tax_cur_pbl  =" + inc_tax_cur_pbl + ",inc_oth_non_csh  = " + inc_oth_non_csh + ",inc_def_tax =" + inc_def_tax + ",inc_oth_inc  = " + inc_oth_inc + ",inc_pri_yr =" + inc_pri_yr + ",o_prft =" + o_prft + ",prft_extra =" + prft_extra + ",inc_gain_sal_good =" + inc_gain_sal_good + ",inc_oth_non_opr_inc =" + inc_oth_non_opr_inc + ",inc_oth_exp =" + inc_oth_exp + ",inc_int_inc  =" + inc_int_inc + ",inc_pri_yr_adj  =" + inc_pri_yr_adj + ",prft_bef_itax  =" + prft_bef_itax + ",inc_pri_yr_adj_loss =" + inc_pri_yr_adj_loss + ",net_inc  =" + net_inc + ",Bad_Debts_Amount  =" + Bad_Debts_Amount + ",Purchases_Credit_Amount=" + Purchases_Credit_Amount + ",inc_op_net_wrth =" + inc_op_net_wrth + ",tot_add =" + tot_add + ",tot_net_inc =" + tot_net_inc + ",inc_dividend =" + inc_dividend + ",inc_sal_eqt =" + inc_sal_eqt + ",inc_ast_rev =" + inc_ast_rev + ",tot_end_wrth =" + tot_end_wrth + " "
                        + " where client_no = " + client_no + " and yr = Convert(Varchar(20),'" + yr + "',120)";
            this.db.Cmd.CommandText = st;
            //this.db.Da.SelectCommand = this.db.Cmd;
            //dt = new DataTable();
            vRes = db.cmd.ExecuteNonQuery();
            db.CloseConn();
            st = "";
            return vRes;
        }
        public DataTable getUser_Det(string u_id)
        {
            Start_Method();
            st = " Select * from Administrator where u_id = " + u_id + " and Status = 1 ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int AddNewFinancials(string client_no, string yr, int audit, int brwr_type, string dat, long sales, long exp, int emp)
        {

            int ind;
            this.client_no = client_no;
            //db = new dbAccess();
            //string strConnString = db.Cn.ConnectionString; // get it from Web.config file  
            SqlTransaction objTrans = null;
            // db.OpenConn();
            using (SqlConnection objConn = new SqlConnection(db.Cn.ConnectionString.ToString()))
            {
                objConn.Open();
                objTrans = objConn.BeginTransaction("Trans");

                SqlCommand objCmd1 = new SqlCommand("Insert into bal_sheet "
                                                    + " (Client_No, brwr_key, yr, audit, auditor, auditor_id, cash_mkt_sec, sht_trm_inv, receivables, inventory, store_spares, adv_utakings, prpaid_exp_dep, tax_payments, oth_recv, net_fx_ast, lng_trm_dep, def_cost, "
                                                    + " lng_trm_inv, lng_trm_loan, oth_lng_trm_ast, liabilities, sht_trm_debt, ac_payable, acrd_mrkp_loan, acrd_exp, tax_payable, wppf, adv_asst_utakings, oth_cur_liab, lng_trm_debt, lng_trm_sen_debt, oth_lng_trm_liab, "
                                                    + " lab_pen_prov, oth_def_prov, min_int, def_tax, surp_rev_fix_ass, paidup_capital, reserves, lng_trm_dep2, ret_earn, sup_op_fxd_ast, sup_fxd_rev, sup_fxd_acq, sup_csh_sal, sup_gan_sal, sup_adj, sup_chk, sup_dbt_cratio, "
                                                    + " sup_pr_repay, sup_land, sup_bldng, sup_mch, sup_of_eqp, sup_ac_gen, sup_vhcl, sup_oth_eqp, sup_wip, sup_acm_dep, sup_tax_rate, sup_dep, inc_net_sal, inc_cst_gsold, inc_adm_gen_exp, inc_dep_amr, inc_oth_non_csh, "
                                                    + " inc_oth_inc, inc_gain_sal_good, inc_oth_non_opr_inc, inc_oth_exp, inc_int_inc, inc_int, inc_opr_les_exp, inc_tax_cur_pbl, inc_def_tax, inc_pri_yr, inc_exord_inc, inc_exord_tax, inc_pri_yr_adj, inc_op_net_wrth, "
                                                    + " inc_bon_shr_issu, inc_sal_eqt, inc_ast_rev, inc_fx_trn_gain, inc_dividend, inc_bon_shr, lng_fin_leas_oblig, lng_red_cap, lng_dir_loan, leg_stat, cib, credit_rat, exp_yrs_ceo, succ_plan, sale_conc, bus_yrs, competitors, "
                                                    + " investigation, dispute, default_bank, buyer_prod, prd_pot, user_srv, util_cap, age_mach, default_exp, recov_amt, resh_rest, waiv_mcb, waiv_nonmcb, bnk_rel, avail_info, source_inc, dir_sub_loan, dir_unsub_loan, aud_rat, "
                                                    + " cib_date, exp_yrs_cfo, exp_yrs_coo, def_bnk2, cib_rep_stat, rat_agency, fin_stat, fin_stat_reason, inc_pri_yr_adj_loss, inc_exord_loss, source_inc2, default_bank2, dispute2, credit_rat_st, credit_rat_outlook, default_exp2, "
                                                    + " mkt_share, prdct_qlty, prdct_postn, int_compt, gecp, ext_rat_stat, rat_date, rel_date, yr_end, sheet_chk_BS, sheet_chk_IS, sheet_chk_SI, sheet_chk_CI, prft_bef_tax, csh_frm_optns, sup_adj_detail, asset_op_lease, "
                                                    + " asset_gurantees, Asset_Commitments, Asset_Contingencies, op_lease_payments, Liab_Guarantees, Liab_Commitments, Liab_Contingencies, Contingent_Liabilities, tot_asts_adj, tot_liab_adj, adv_dep, gross_prft, o_prft, "
                                                    + " prft_bef_itax, prft_extra, net_inc, tot_cur_asts, tot_lng_trm_asts, tot_asts, tot_cur_liab, tot_lng_trm_debt, lng_trm_laib, tot_def_laib, Sub_tot, tot_lng_trm_laib, tot_liab, total_Networth, tot_liab_eqty, tot_add, tot_ded, tot_nfxd_ast, "
                                                    + " tot_net_fxd_ast, EQM_ROI, EQM_GoSI, EQM_ILTLFP, EQM_OI, EQM_GLRI, EQM_OAE, EQM_FC, EQM_IoI, EQM_CT, EQM_PT, EQM_GIncLoss, EQM_NExp, EQM_NetPrLossBefTax, EQM_NIncLoss, EQM_TotalTax, rating_id_lt, "
                                                    + " rating_id_st, Total_No_Shares, Share_price, Bad_Debts_Amount, Purchases_Credit_Amount) "

                                                    + " VALUES        (" + client_no + ",'',Convert(DateTime,'" + yr + "')," + audit + ",'','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','') ", objConn, objTrans);

                SqlCommand objCmd2 = new SqlCommand("Insert into brwr_rating "
                                                    + " (client_no, brwr_key, yr, audit, Fin_Input_Date, brwr_type, Client_Sales, Client_Exp, nat_business, asgnd_rat, cur_drvd_rat, class_status, class_Date, justfication, assgnd_rat_appby, dwn_grd, agg_score, bas_rat,  "
                                                    + "  Final_Rating, Status, u_id, authorized_by, authorized_time, authorized_by_CD, authorized_time_CD, H_Date, Latest_Class_Status, Latest_class_Date, agg_score_A, bas_rat_A, Override_By, Override_Date, Override_Authority,  "
                                                    + " Override_Reason, Meeting_No, Meeting_Date, Approval_No, Approval_Date, Bus_type, No_Emp, Clnt_prof_type, Chng_Client_Def_Req_By, Chng_Client_Def_App_By, Chng_Client_Def_App_Date, Auditor_Rating) "
                                                    + " VALUES        (" + client_no + ",'',Convert(DateTime,'" + yr + "')," + audit + ",'" + dat + "'," + brwr_type + "," + sales + "," + exp + ",'','','','','','','','','','','','','','','','','','','','','','','','','','','','','','',''," + emp + ",'','','','','') ", objConn, objTrans);

                SqlCommand objCmd3 = new SqlCommand("Insert into brwr_pts "
                                                    + " (client_no, brwr_key, yr, audit, brwr_id, own_str_pts, own_str_text_pts, own_str_pts_tbl, rel_pts, ac_behav_pts, ac_behav_text_pts, ac_behav_pts_tbl, sup_byr_pts, yrs_bus_pts, pers_bank_pts, coll_cov_pts, "
                                                    + " Suc_Plan_Pts, Suc_Plan_Pts_tbl, Suc_Plan_Text_Pts, bus_out_pts, bus_out_pts_tbl, bus_out_text_pts, sup_byr_comm_pts, Cr_Rel_bnk, Bank_Rel_Pts, Bank_Rel_Pts_tbl, Bank_Rel_Text_Pts, Spon_Exp_pts, Spon_Rep_pts, "
                                                    + " Spon_Exp_Text_Pts, Spon_Rep_Text_Pts, Spon_Exp_Pts_Tbl, Spon_Rep_Pts_Tbl, CIB_pts, CIB_Pts_tbl, CIB_Text_pts, Country_Risk_pts, Country_Risk_Pts_tbl, Country_Risk_Text_pts, Leg_Disp_Pts, Leg_Disp_Pts_tbl, "
                                                    + " Owner_Rep_Pts, Owner_Rep_Pts_tbl, Owner_Rep_Text_Pts, Ind_Att_pts, Ind_Att_Pts_tbl, Ind_Att_text_pts, Leg_Disp_Text_Pts, adj_net_worth, adj_net_worth_pts, current_ratio, current_ratio_pts, net_profit, net_profit_pts, "
                                                    + " adj_leverage, adj_leverage_pts, fin_trend, fin_trend_pts, Deg_Com_Lvg, Deg_Com_Lvg_Pts, Debt_Srv_Cov, Debt_Srv_Cov_Pts, Ret_Cap_Emp, Ret_Cap_Emp_Pts, c0_sale_yr, c1_sale_yr, c2_sale_yr, c3_sale_yr, c0_net_inc, "
                                                    + " c1_net_inc, c2_net_inc, c3_net_inc, c0_Adj_Cash_Flow, c1_Adj_Cash_Flow, c2_Adj_Cash_Flow, c3_Adj_Cash_Flow, c0_FCFF, c1_FCFF, c2_FCFF, c3_FCFF, fin_rep_pts, fin_rep_text_pts, fin_rep_pts_tbl, Bus_yrs, Bus_yrs_pts, "
                                                    + " Bus_Bank_pts, Bus_Bank_Pts_tbl, Bus_Bank_Text_Pts, mgmt_pts, mgmt_pts_tbl, mgmgt_text_pts, Mgmt_Exp_Pts, Mgmt_Exp_Pts_tbl, Mgmt_Exp_Text_Pts, Mgmt_Rep_pts, Mgmt_Proc_Pts_tbl, Mgmt_Proc_pts, "
                                                    + " Mgmt_Proc_Text_Pts, Rec_Trn_Ov, Rec_Trn_Ov_Pts, Bus_Type, Bus_Type_Pts, Tec_Qual_Pts, Tec_Qual_Pts_tbl, Tec_Qual_Text_Pts, Dep_Sup_Cus_Pts, Dep_Sup_Cus_Pts_tbl, Dep_Sup_Cus_Text_Pts, Will_Pro_Info_Pts, "
                                                    + " Will_Pro_Info_Pts_tbl, Will_Pro_Info_Text_Pts, Net_CF, Net_CF_Pts, Client_Net_Worth, Client_NW_Pts, Debt_Srv_CF, Debt_Srv_CF_Pts, Tot_Debt_Burden, Tot_Debt_Pts, Vel_Trans, Vel_Trans_Pts, Dis_Def, Dis_Def_Pts, "
                                                    + " Lev_Over_Sight_Pts, Lev_Over_Sight_Pts_tbl, Lev_Over_Sight_Text_Pts, Age, Age_Pts, Dep_Sin_Sou_Inc_Pts, Dep_Sin_Sou_Inc_Text_Pts, Dep_Sin_Sou_Inc_Pts_Tbl, Own_Rent_Land_Pts, Own_Rent_Land_Text_Pts, "
                                                    + " Own_Rent_Land_Pts_Tbl, Security_Pts, Security_Text_Pts, Security_Pts_Tbl, Acd_Qual_Pts, Acd_Qual_Pts_Tbl, Acd_Qual_Text_Pts, Borw_Exp_Oth_Banks_pts, Borw_Exp_Oth_Banks_Pts_Tbl, Borw_Exp_Oth_Banks_Text_pts, "
                                                    + " page1_score, page2_score, Agg_Score, bas_rat, rel_date_pts, Debt_Input_Ratio, Debt_Input_Ratio_Pts, Output_Input_Ratio, Output_Input_Ratio_Pts, Debt_Ind_Ratio, Debt_Ind_Ratio_Pts, Debt_Eqty_Pts, Margin_On_Advance, "
                                                    + " EQM_Resch_tbl_pts, EQM_Dedicated_Resch_Desk_Pts, EQM_Dedicated_Resch_Desk_Text_Pts, EQM_Trend_Margin_pts_tbl, EQM_Trend_Margin_Pts, EQM_Trend_Margin_Text_Pts, EQM_Trad_Pat_pts_tbl, "
                                                    + " EQM_Trading_Pattern_Pts, EQM_Trading_Pattern_Text_Pts, EQM_Rec_TrnOver_Pts_tbl, EQM_Rec_TrnOver_Pts, EQM_Rec_TrnOver_Text_Pts, ICR, ICR_Pts, NAE, NAE_Pts, CR, CR_Pts, GR, GR_Pts, DB, DB_Pts, "
                                                    + " O_Type_Score, CIIRS_Override, BtoB) "
                                                    + " VALUES (" + client_no + ",'',Convert(DateTime,'" + yr + "')," + audit + ",'','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','') ", objConn, objTrans);


                if (brwr_type == 3 || brwr_type == 5)
                {
                    objCmd4 = new SqlCommand("Insert into SME_Financials "
                                            + " (client_no, brwr_key, yr, audit, Sum_12Mths_Cr, Sum_12Mths_Dr, Sum_NonBus_Cr, Sum_NonBus_Dr, Auth_Fund_Lmt, Auth_NonFund_Lmt, tot_cf, tot_fund_nonfund, Net_Worth, Cash, Govt_Sec, Bnk_Bal, "
                                            + " Shares_Main_Index, Bonds, Mut_Fund_Unit, Cert_Inv, tot_NetWorth, Debt_Burden, Other_Debt_Burden, Tot_Burden, Princ_Paid_Yr, Int_Paid_Yr, tot_debt_srv, tot_sales, tot_liq_collateral, tot_vol_bus, tot_vol_Stock_Exch, "
                                            + " ac_rec, ac_payable, Agri_Tot_NetSurplus, Agri_Var_InputCost, Agri_Ind_Cost, Agri_Tot_Cur_Asts, Agri_Tot_Cur_Liab, Agri_Sec_Value, Agri_Limit_Amt, Debt_Eqty_Pts, Agri_LT_Fin) "
                                            + " VALUES        (" + client_no + ",'',Convert(DateTime,'" + yr + "')," + audit + ",'','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','','') ", objConn, objTrans);

                }
                try
                {

                    objCmd1.ExecuteNonQuery();
                    objCmd2.ExecuteNonQuery();
                    objCmd3.ExecuteNonQuery();
                    if (brwr_type == 3 || brwr_type == 5)
                    {
                        objCmd4.ExecuteNonQuery();
                    }
                    // Throws exception due to foreign key constraint  
                    objTrans.Commit();
                    ind = 1;
                }
                catch (Exception ex)
                {
                    objTrans.Rollback();
                    objTrans.Dispose();
                    ind = 0;
                }
                finally
                {
                    objConn.Close();
                }
                return ind;
            }
        }

        public DataTable test()
        {
            Start_Method();
            st = " Select p.client_no,name,case when br1.brwr_type = 1 then 'Corporate' "
                 + " when br1.brwr_type = 2 then 'Commercial' "
                + "when br1.brwr_type = 3 then 'SE' "
                + " when br1.brwr_type = 5 then 'Agri' "
                + " when br1.brwr_type = 8 then 'ME' end[Segment], "
                + " case when((br1.class_status = 1 and br1.latest_class_status is null) or(br1.latest_class_status = 1)) then Convert(varchar(20), br1.Final_Rating) "
                + " when((br1.class_status = 2 and br1.latest_class_status is null) or(br1.latest_class_status = 2)) then '9b' "
                + " when((br1.class_status = 4 and br1.latest_class_status is null) or(br1.latest_class_status = 4)) then '10' "
                + " when((br1.class_status = 5 and br1.latest_class_status is null) or(br1.latest_class_status = 5)) then '11' "
                + " when((br1.class_status = 6 and br1.latest_class_status is null) or(br1.latest_class_status = 6)) then '12' "
                + " when((br1.class_status = 3 and br1.latest_class_status is null) or(br1.latest_class_status = 3)) then '9c' else '' end[Final Rating], "
                  + " br.yr,net_profit,current_ratio,Debt_Srv_Cov,                                                                                                                                                        "
                        + " case when not tot_asts = 0 then net_inc/ tot_asts end[ROA],                                                                                                                                                                      "
                        + " case when not tot_liab = 0 then Cash_Flow_Opns/ tot_liab end[Cash Flow Coverage Ratio],                                                                                                                                          "
                        + " case when not inc_int = 0 then(o_prft + inc_dep_amr) / inc_int end[EBITDA to Interest Expense Ratio],                                                                                                                            "
                        + " case when not tot_cur_liab = 0 then Cash_Flow_Opns/ tot_cur_liab end[Cash Flow to Current Liabilities],                                                                                                                          "
                        + " case when not tot_lng_trm_laib = 0 then Cash_Flow_Opns/ tot_lng_trm_laib end[Cash Flow to Long Term Liabilities],                                                                                                                "
                        + " case when not tot_asts = 0 then Cash_Flow_Opns/ tot_asts end[Asset Efficiency Ratio],                                                                                                                                            "
                        + " case when not Fin_Cost = 0 then(Cash_Flow_Opns + Fin_Cost + Tax_paid) / Fin_Cost end[Cashflow Interest Coverage Ratio],                                                                                                          "
                        + " case when not tot_liab = 0 then net_inc/ tot_liab end[Net Profit to Total Debt],                                                                                                                                                 "
                        + " case when not inc_net_sal = 0 then(Cash_Flow_Opns / inc_net_sal) * 100 end[OperatingCashFlowtoSales] ,                                                                                                                           "
                        + " REPLACE(CAST(fin_rep_text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS fin_rep_text_pts, "
                        + " REPLACE(CAST(own_str_text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS own_str_text_pts,                                                          "
                        + " REPLACE(CAST(ac_behav_text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS ac_behav_text_pts,                                                    "
                        + " REPLACE(CAST(Mgmt_Exp_Text_Pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS Mgmt_Exp_Text_Pts,                                                     "
                        + " REPLACE(CAST(Ind_Att_text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS Ind_Att_text_pts,                                                        "
                        + " REPLACE(CAST(Spon_Exp_Text_Pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS Spon_Exp_Text_Pts,                                                   "
                        + " REPLACE(CAST(CIB_Text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS CIB_Text_pts,                                                                     "
                        + " Bank_Rel_Text_Pts,2019 - Year(p.bus_yrs)[Years_IN_bus] ,                                                                                                                                                                         "
                        + " REPLACE(CAST(fin_rep_text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), '''') AS fin_rep_text_pts,                                                   "
                        + " REPLACE(CAST(bus_out_text_pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS bus_out_text_pts,                                                        "
                        + " REPLACE(CAST(Mgmgt_Text_Pts as NVARCHAR(MAX)), CHAR(13) + CHAR(10), ' ') AS Mgmgt_Text_Pts "
                        + "   from profile p                                                                                                                                                                                                                 "
                        + " left join (Select Max(yr)yr, client_no from Brwr_Rating where authorized_by is not null group by client_no) br on br.client_no = p.Client_no                                                                                     "
                        + " left join Brwr_Rating br1 on br1.client_no = br.client_no and br1.yr = br.yr                                                                                                                                                     "
                        + " left join Brwr_pts bp on br.client_no = bp.client_no and br.yr = bp.yr                                                                                                                                                           "
                        + " left join Bal_Sheet bs on bs.client_no = bp.client_no and bs.yr = bp.yr                                                                                                                                                          "
                        + " left join Cash_Flows cf on cf.client_no = bp.client_no and cf.yr = bp.yr                                                                                                                                                         "
                        + " WHERE(Temenos_ID IN(10134518, 10208127, 10208666, 10214042, 10221037, 10221300, 10223885, 10223888, 10223891, 10223894, 10225500, 10227748, 10230295, 10231253, 10232634, 10265542, 10282951, 10380197,                          "
                        + "                          10383575, 10675524, 10746197, 10974408, 10975227, 10978926, 10980462, 10981410, 10981437, 10981439, 10981441, 10981444, 10981446, 10981447, 10981448, 10981450, 10981451, 10981455, 10981457, 10981464, "
                        + "                          10982857, 10983341, 10983371, 10983397, 10983401, 10983449, 10983455, 10983457, 10983463, 10983493, 10983495, 10983497, 10983501, 11009428, 11101806, 11107020, 11107021, 11107022, 11107029, 11107035, "
                        + "                          11107036, 11107075, 11107098, 11175212, 11232993, 11346032, 11485990, 11486138, 11486393, 11486396, 11489286, 11490672, 11490834, 11491970, 11493386, 11493599, 11493606, 11494282, 11495335, 11495708, "
                        + "                          11497104, 11500336, 11501654, 11503874, 11505840, 11506920, 11508073, 11510341, 11510594, 11511074, 11511213, 11511403, 11511413, 11513129, 11513719, 11514677, 11514712, 11516786, 11517085, 11517269, "
                        + "                          11517702, 11518001, 11518239, 11518251, 11518255, 11518256, 11518259, 11518270, 11518271, 11518272, 11518273, 11518275, 11518283, 11518290, 11518298, 11518299, 11518301, 11518303, 11518305, 11518341, "
                        + "                          11518342, 11518429, 11518462, 11518478, 11518575, 11518591, 11518610, 11518615, 11518660, 11518839, 11519878, 11520888, 11520969, 11521150, 11521293, 11521572, 11521815, 11522337, 11538638, 11595167, "
                        + "                          11595211, 11595228, 11595314, 11596020, 11596112, 11596383, 11596748, 11597225, 11597371, 11597450, 11597734, 11597815, 11597828, 11611125, 11617428, 11618940, 11618941, 11619957, 11622078, 11623190, "
                        + "                          11623294, 11624481, 11624823, 11626399, 11628013, 11628767, 11630614, 11631609, 11633143, 11633652, 11633923, 11634305, 11634627, 11634628, 11634629, 11634634, 11634636, 11634638, 11634641, 11634642, "
                        + "                          11634648, 11634649, 11634672, 11634679, 11634707, 11634714, 11634774, 11634845, 11635194, 11635357, 11635405, 11635428, 11635430, 11635431, 11635454, 11635655, 11635714, 11636699, 11638566, 11639318, "
                        + "                          11641914, 11643697, 11647446, 11671850, 11690414, 11703939, 11740193, 11759411, 11763608, 11798541, 11811745, 11867569, 11870636, 11881715, 11883146, 11889615, 11900070, 11924199, 11954723, 12067160, "
                        + "                          12194210, 12419055, 12441302, 12470773, 12509622, 12510427, 12542447, 12650191, 12654111, 12690884, 12706542, 12708749, 12746456, 12755892, 12770416, 12804435, 12985892, 13004200, 13016601, 13079172, "
                        + "                          13176081, 13410968, 13417515, 13427584, 13564122, 13624215, 13627208, 13674465, 13706382, 13712818, 13871186, 13982676, 14029711, 14095838, 14134911, 14223863, 14239409, 14371142, 14391490, 14427672, "
                        + "                          14437063, 14466299, 14476695, 14491430, 14589127, 14740877, 15113813, 15263798, 15271431, 15334467, 15353032, 15416758, 15417711, 15450995, 15467677, 15469502, 15551178, 15610599, 10000601, 10000630, "
                        + "                          10007459, 10021533, 10021863, 10036214, 10127234, 10128493, 10128677, 10130942, 10133546, 10139162, 10497974, 10515132, 10515161, 10516459, 10516472, 10535746, 11194605, 11195738, 11195857, 11197180, "
                        + "                          11199576, 11207104, 11207324, 11207828, 11208227, 11208604, 11208754, 11209620, 11209630, 11210033, 11210100, 11211108, 11211846, 11211992, 11212214, 11212430, 11213348, 11215638, 11215640, 11215658, "
                        + "                          11219777, 11220194, 11220704, 11227114, 11228649, 11228736, 11234074, 11234075, 11236957, 11238499, 11240956, 11240964, 11243102, 11243285, 11258038, 11372361, 11528235, 11528238, 11559890, 11561366, "
                        + "                          11593372, 11701125, 11708256, 11711685, 11714378, 11720853, 11722674, 11737649, 11758626, 11783776, 11789906, 11829746, 11832162, 11832584, 11849864, 11877062, 11927081, 11964746, 11968055, 12009647, "
                        + "                          12394865, 12406633, 12434194, 12475893, 12476591, 12547625, 12550405, 12600015, 12619973, 12686918, 12699089, 12711102, 12743163, 12799030, 12918528, 12927214, 12938518, 12982255, 12992194, 13005373, "
                        + "                          13119827, 13145595, 13459339, 13474351, 13479280, 13661483, 13675256, 13689429, 13690260, 13705245, 13756437, 13775961, 13817744, 13973113, 14207946, 14255006, 14269990, 14415318, 14419515, 14453767, "
                        + "                          14512251, 14538326, 14581640, 14599132, 15095241, 15099707, 15274248, 15372634, 15479672, 15551286, 10176094, 10275274, 10330783, 10333615, 10334603, 10354049, 10357749, 10358008, 10526926, 10871279, "
                        + "                          10874887, 10894044, 10894162, 10894339, 10894400, 10895600, 10896384, 10898671, 10900058, 10901730, 10906408, 10908260, 10915368, 10932912, 10933279, 10956439, 10990780, 10997368, 11049086, 11140618, "
                        + "                          11149937, 11573910, 11966041, 12666675, 12814622, 12822807, 12829884, 13184069, 13339942, 13462823, 13911759, 14128131, 14140610, 14206658, 14463563, 15143571, 15382158, 15476363, 10210997, 10244319, "
                        + "                          10246661, 10246669, 10256342, 10257113, 10469908, 10543042, 10544135, 10657207, 10684839, 10689179, 10713588, 10730421, 10731008, 10734620, 10734958, 10736238, 10737615, 10739390, 10741279, 10741921, "
                        + "                          10746193, 10746489, 10747804, 10751947, 10754929, 10756557, 10771344, 10774414, 10777372, 10778244, 10778245, 10778501, 10779383, 10787173, 10792706, 10794018, 10803983, 10809586, 10813265, 10817212, "
                        + "                          10817499, 10819227, 10822752, 10823230, 10824473, 10826022, 10835117, 10837297, 10939490, 11014499, 11014973, 11028262, 11031208, 11174604, 11295926, 11295938, 11295965, 11295977, 11295989, 11295992, "
                        + "                          11295993, 11295994, 11295997, 11296006, 11296008, 11296138, 11296297, 11296460, 11296772, 11301915, 11301951, 11302279, 11305158, 11305200, 11307477, 11307517, 11307982, 11313988, 11641714, 11645539, "
                        + "                          11680104, 11738318, 11793858, 11807541, 11840499, 11876150, 11911865, 11954143, 12038340, 12414938, 12435284, 12441249, 12491242, 12516303, 12519539, 12526910, 12549529, 12616206, 12635813, 12670021, "
                        + "                          12791298, 12807214, 12821813, 12837407, 12848990, 12920596, 12931182, 12952050, 13033621, 13040086, 13088264, 13091816, 13099093, 13108458, 13127237, 13148885, 13179102, 13251080, 13294236, 13319324, "
                        + "                          13364384, 13392949, 13393514, 13443622, 13481183, 13525771, 13599517, 13659997, 13712359, 13723561, 13941467, 13950632, 14027137, 14074864, 14165440, 14167263, 14201905, 14294283, 14420614, 14431906, "
                        + "                          14506923, 14675839, 14750602, 14817879, 14933442, 14950123, 15139700, 15357010, 10386688, 10433572, 10441650, 10473952, 10557593, 10563968, 10565020, 10606677, 10608121, 10630461, 10631675, 10634863, "
                        + "                          10655139, 10669533, 10669556, 10761197, 10761329, 10763882, 10771655, 10781326, 10971866, 10972617, 11002240, 11285625, 11292206, 11320374, 11320378, 11320385, 11320390, 11320809, 11321287, 11322657, "
                        + "                          11325104, 11335290, 11335293, 11344038, 11344040, 11351055, 11355546, 11362057, 11372397, 11372399, 11372402, 11372403, 11372408, 11372409, 11372412, 11372414, 11372415, 11372416, 11372417, 11372418, "
                        + "                          11372421, 11372430, 11372432, 11372433, 11372434, 11372436, 11373704, 11374670, 11375204, 11375510, 11378227, 11381402, 11381816, 11382597, 11382945, 11384340, 11384680, 11387717, 11387727, 11387729, "
                        + "                          11388160, 11388289, 11388310, 11389684, 11389697, 11390799, 11393041, 11398547, 11398612, 11398696, 11398843, 11404845, 11405586, 11412327, 11412967, 11416752, 11419712, 11436248, 11453332, 11453335, "
                        + "                          11464011, 11475850, 11480599, 11481294, 11482065, 11649200, 11653616, 11669183, 11727195, 11734299, 11737908, 11737928, 11739138, 11743595, 11769687, 11808353, 11860318, 11898094, 11909560, 11922785, "
                        + "                          11948988, 11992733, 12004836, 12101032, 12123779, 12129293, 12164694, 12319479, 12368018, 12381733, 12440173, 12489499, 12560744, 12562365, 12706883, 12711250, 12807578, 12861100, 12954867, 12963747, "
                        + "                          12973906, 12980611, 12992879, 13039350, 13128542, 13142409, 13152240, 13208335, 13260303, 13499359, 13617889, 13776886, 13815068, 13921924, 13947165, 13983285, 14049310, 14070812, 14106102, 14132219, "
                        + "                          14216128, 14229308, 14327077, 14336872, 14480251, 14562208, 14663061, 14710503, 14720076, 14865548, 15063998, 15111010, 15461914, 15522132, 11107037, 11193311, 11597548, 11982825, 12106697, 12135959, "
                        + "                          12146921, 12147226, 12149109, 12150894, 12157590, 12157700, 12158144, 12158261, 12159629, 12190633, 12191942, 12192198, 12192207, 12192506, 12192710, 12193281, 12193390, 12193534, 12194185, 12194287, "
                        + "                          12195115, 12195116, 12195363, 12195558, 12195907, 12195982, 12196320, 12196378, 12196549, 12197165, 12201086, 12201126, 12201766, 12201872, 12202781, 12204672, 12204863, 12206922, 12213478, 12213903, "
                        + "                          12226195, 12231487, 12231941, 12246816, 12264243, 12267940, 12272016, 12272580, 12290150, 12291530, 12295861, 12296958, 12298068, 12300033, 12300708, 12300837, 12301876, 12301994, 12303323, 12304127, "
                        + "                          12305692, 12306997, 12307518, 12308095, 12308614, 12308922, 12309376, 12309379, 12309726, 12309737, 12309744, 12309745, 12310195, 12311970, 12312552, 12312574, 12312575, 12312626, 12312752, 12313025, "
                        + "                          12321730, 12392111, 12500951, 12504351, 12516181, 12518986, 12622825, 12629466, 12710457, 12802973, 12803161, 12821507, 12836888, 12906064, 12963521, 13055843, 13078252, 13098097, 13109523, 13213703, "
                        + "                          13245485, 13269839, 13294813, 13318543, 13371815, 13435346, 13470435, 13548930, 13555861, 13785004, 13806771, 13840218, 13901063, 13972010, 13982776, 14025071, 14042526, 14114521, 14206428, 14278751, "
                        + "                          14442388, 14492754, 14594593, 14597531, 14622644, 14674266, 14807866, 14976310, 15026545, 15252444, 10212772, 10550306, 10783704, 10792347, 10826042, 10981460, 10983499, 11028630, 11074068, 11104269, "
                        + "                          11107070, 11118774, 11119319, 11119601, 11175529, 11213123, 11220033, 11234076, 11238470, 11238527, 11241166, 11262720, 11264145, 11461412, 11480049, 11520052, 11528091, 11598139, 11619104, 11631326, "
                        + "                          11634639, 11634690, 11643698, 11709224, 11783841, 11887575, 11919069, 12106380, 12136134, 12559917, 12569102, 12698272, 13004233, 13015267, 13062529, 13271822, 13488448, 13664756, 13772218, 13817520, "
                        + "                          13866534, 14833444, 14950318, 15238555, 15298743, 15329452, 10000639, 10123377, 10130979, 10214066, 10266305, 10611317, 10779127, 10792295, 10792305, 10792325, 10792345, 10805780, 10843600, 11061775, "
                        + "                          11107140, 11136351, 11208652, 11208768, 11209459, 11211941, 11257314, 11260605, 11319647, 11338619, 11341758, 11437143, 11502167, 11515514, 11518282, 11518661, 11595200, 11595283, 11595322, 11595330, "
                        + "                          11595484, 11595611, 11595746, 11595772, 11596392, 11596858, 11596864, 11597586, 11624704, 11634575, 11635639, 11711794, 11873058, 12453825, 12517852, 13068359, 13868522, 14479742, 15578205, 12136441, "
                        + "                          12196135, 12300025, 12158213, 12477165, 12193540, 13698738, 12304554, 12196564, 12307155, 12308485, 12138350, 12195045, 12144193, 12307370, 12230853, 13113801, 12134790)) and br1.brwr_type in (1,2) ";
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            dt = new DataTable();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            st = "";
            return dt;
        }
        public DataTable get_UserBranches(int Dep_id)
        {
            Start_Method();
            st = " Select case when LEN(BrCd) = 1 then CONCAT('000',BrCd) "
                    + " when LEN(BrCd) = 2 then CONCAT('00', BrCd) "
                    + " when LEN(BrCd) = 3 then CONCAT('0', BrCd) "
                    + " when LEN(BrCd) = 4 then CONCAT('', BrCd) end[ConcBrcd], * "
                    + " from Brdirt where Area like '%" + Dep_id + "%' ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_AcctTypes()
        {
            Start_Method();
            st = " Select * from Acct_Types ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_GroupName()
        {
            Start_Method();
            st = "Select * from Groups order by Group_ID ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_BusinessGroup()
        {
            Start_Method();
            st = " Select * from Business_Group where id <> 900 ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public int get_maxClientID()
        {
            Start_Method();
            st = " Select max(client_no)client_no from profile ";
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            dt = new DataTable();
            db.CloseConn();
            try
            {
                this.db.Da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            st = "";
            return Convert.ToInt32(dt.Rows[0]["client_no"].ToString());
        }
        public DataTable get_industries(int brwr_type)
        {
            Start_Method();
            if (brwr_type == 1)
                st = " Select * from Indus_class ";
            else
                st = " Select * from sub_sectors ";
            dt = new DataTable();
            End_Method();
            return dt;
        }//
        public DataTable get_Countries()
        {
            Start_Method();
            st = " select * from CIIRS_Admin.dbo.countries order by Country_ID asc ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_City()
        {
            Start_Method();
            st = " select * from CIIRS_Admin.dbo.cities where Country_ID= 1 order by city_name asc ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        public DataTable get_OType()
        {
            Start_Method();
            st = " select * from constype order by initial asc ";
            dt = new DataTable();
            End_Method();
            return dt;
        }
        protected void Start_Method()
        {
            db.OpenConn();
            this.db.Cmd.CommandType = CommandType.Text;
            this.db.Cmd.CommandTimeout = 0;
        }
        protected void End_Method()
        {
            this.db.Cmd.CommandText = st;
            this.db.Da.SelectCommand = this.db.Cmd;
            db.CloseConn();
            try
            {
                this.db.Da.Fill(dt);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            st = "";
        }
    }
}