﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Online_Shopping_Backup
{
    public partial class order_single : System.Web.UI.Page
    {
        SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["ShoppingConnectionString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = new DataTable();

                conn.Open();
                SqlCommand sqlCmd = new SqlCommand("SELECT * from login_data WHERE id = @id", conn);
                SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);
                string user_id = Request.Cookies["u_id"].Value;
                sqlCmd.Parameters.AddWithValue("@id", user_id);
                sqlDa.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    txt_username.Text = dt.Rows[0]["name"].ToString(); //Where ColumnName is the Field from the DB that you want to display
                    txt_Address.Text = dt.Rows[0]["address"].ToString();
                    txt_email.Text = dt.Rows[0]["Email"].ToString();
                    txt_mobile_no.Text = dt.Rows[0]["mobile_no"].ToString();
                }
                conn.Close();
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string product_id = Request.QueryString["product_id"];
            string user_id = Request.Cookies["u_id"].Value;
            string payment_type = ddl_payment_option.SelectedValue;
            int quantity = int.Parse(txt_quantity.Text);

            DataTable dt = new DataTable();
            SqlCommand sqlCmd = new SqlCommand("SELECT Product_name, Our_Prize FROM main_product WHERE P_ID = @P_ID", conn);
            sqlCmd.Parameters.AddWithValue("@P_ID", product_id);
            SqlDataAdapter sqlDa = new SqlDataAdapter(sqlCmd);
            sqlDa.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                string product_name = dt.Rows[0]["Product_name"].ToString();
                decimal total = Convert.ToDecimal(dt.Rows[0]["Our_Prize"]) * quantity;

                string query = "insert into main_order (user_id, user_name, product_id, product_name, quantity, total, email_address, mobile_no, payment_type, address) values(@user_id, @user_name, @product_id, @product_name, @quantity, @total, @e_mail, @mobile_no, @payment_type, @address)";
                conn.Open();
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user_id", user_id);
                cmd.Parameters.AddWithValue("@user_name", txt_username.Text);
                cmd.Parameters.AddWithValue("@product_id", product_id);
                cmd.Parameters.AddWithValue("@product_name", product_name);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@total", total);
                cmd.Parameters.AddWithValue("@e_mail", txt_email.Text);
                cmd.Parameters.AddWithValue("@mobile_no", txt_mobile_no.Text);
                cmd.Parameters.AddWithValue("@payment_type", payment_type);
                cmd.Parameters.AddWithValue("@address", txt_Address.Text);

                cmd.ExecuteNonQuery();
                conn.Close();
                Response.Redirect("~/Pages/Thank_you.aspx");
            }
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
