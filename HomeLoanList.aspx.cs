﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Drawing;
using System.Net;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using doc_ver.Models;



namespace doc_ver
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["USER_ID"] == null)
            {
                Response.Redirect("login.aspx");

            }
            else
            {


                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://trustyloanapi.somee.com/api/");          

                    //GET Request
                    var responseTask = client.GetAsync("LoginDetails/");
                    responseTask.Wait();

                    var result = responseTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<LoginDetailsClass[]>();
                        readTask.Wait();

                        var logindetails1 = readTask.Result;

                        verified.DataSource = logindetails1;

                        verified.DataBind();

                    }

                }



                String constring = ConfigurationManager.ConnectionStrings["forchashConnectionString"].ConnectionString;
                SqlConnection sqlcon = new SqlConnection(constring);


                


                String squery2 = "select ID, FULL_NAME, PAN_NO from HomeLoan" +
                    " where OverAllStatus = 'Not Verified'";
                SqlCommand cmd2 = new SqlCommand(squery2, sqlcon);
                SqlDataAdapter sda2 = new SqlDataAdapter(cmd2);

                DataTable dt2 = new DataTable();

                sda2.Fill(dt2);
                NotVerified.DataSource = dt2;
                NotVerified.DataBind();

                sqlcon.Close();
                sda2.Dispose();


                String squery3 = "select ID, FULL_NAME, PAN_NO from HomeLoan" +
                    " where OverAllStatus = 'Pending'";
                SqlCommand cmd3 = new SqlCommand(squery3, sqlcon);
                SqlDataAdapter sda3 = new SqlDataAdapter(cmd3);

                DataTable dt3 = new DataTable();

                sda3.Fill(dt3);
                Pending.DataSource = dt3;
                Pending.DataBind();

                sqlcon.Close();
                sda3.Dispose();


            

            }
                }

                protected void verified_PageIndexChanging(object sender, GridViewPageEventArgs e)
                {
            

                }

                protected void showdiv1_Click(object sender, EventArgs e)
                {
            
                }

                protected void showdiv2_Click(object sender, EventArgs e)
                {

                }

                protected void showdiv3_Click(object sender, EventArgs e)
                {
          
                }

                protected void linkSelect_Click(object sender, EventArgs e)
                {
                    int selectedRow = verified.SelectedIndex;

                    //teaking the value of first column in selected row
                    string firstColumn = verified.Rows[selectedRow].Cells[2].Text;

                    Label1.Text = firstColumn;

                    Response.Redirect("HomeLoanPage.aspx?PanNum=" + firstColumn);

                }



                protected void Pending_SelectedIndexChanged(object sender, EventArgs e)
                {
                    string name = Pending.SelectedRow.Cells[2].Text;

                    Label1.Text = name;

                    Response.Redirect("HomeLoanPage.aspx?PanNum=" + name);
                }

                protected void verified_SelectedIndexChanged(object sender, EventArgs e)
                {
                    string name = verified.SelectedRow.Cells[0].Text;

                    Label1.Text = name;

                    Response.Redirect("HomeLoanPage.aspx?PanNum=" + name);
                }

                protected void NotVerified_SelectedIndexChanged(object sender, EventArgs e)
                {
                    string name = NotVerified.SelectedRow.Cells[0].Text;

                    Label1.Text = name;

                    Response.Redirect("HomeLoanPage.aspx?PanNum=" + name);
                }


        protected void BtnCustomSearch_Click(object sender, EventArgs e)
        {

            String constring = ConfigurationManager.ConnectionStrings["forchashConnectionString"].ConnectionString;
            SqlConnection sqlcon = new SqlConnection(constring);

            String squery = "select * from HomeLoan" +
                " where PAN_NO = '" + TextBox3.Text + "'";
            SqlCommand cmd = new SqlCommand(squery, sqlcon);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            DataTable dt = new DataTable();

            sda.Fill(dt);

            if (dt.Rows.Count > 0)
            {

                string name = TextBox3.Text;

                Response.Redirect("HomeLoanPage.aspx?PanNum=" + name);


            }
            else
            {
                AvailableOrNot.Text = "Enter Valid Credentials";
            }

        }
    }
}

