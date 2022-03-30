using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace TaskProject
{
    public class DatabaseMssql : ICustomerRepository
    {
        private string connectionStringMSSQL = ConfigurationManager.ConnectionStrings["connectionStringMSSQL"].ConnectionString;

        public List<Customer> ShowAllCustomer() 
        {
            List<Customer> lstCustomers = new List<Customer>();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionStringMSSQL))
                {
                    sqlCon.Open();

                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM dbo.Customers", sqlCon);

                    DataTable dt = new DataTable();
                    sda.Fill(dt);

                    lstCustomers = FillCustomerList(dt);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }

            return lstCustomers;

        }

        public bool InsertCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionStringMSSQL))
                {
                    sqlCon.Open();
                    string query = @"INSERT INTO customers (CustomerId, CompanyName, ContactName, ContactTitle, Address, City, Region, PostalCode, Country, Phone, Fax) 
                                VALUES (@customerId,@companyName, @contactName, @contactTitle, @address, @city, @region, @postalCode, @country, @phone, @fax)";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);

                    cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@companyName", customer.CompanyName);
                    cmd.Parameters.AddWithValue("@contactName", customer.ContactName);
                    cmd.Parameters.AddWithValue("@contactTitle", customer.ContactTitle);
                    cmd.Parameters.AddWithValue("@address", customer.Address);
                    cmd.Parameters.AddWithValue("@city", customer.City);
                    cmd.Parameters.AddWithValue("@region", customer.Region);
                    cmd.Parameters.AddWithValue("@postalCode", customer.PostalCode);
                    cmd.Parameters.AddWithValue("@country", customer.Country);
                    cmd.Parameters.AddWithValue("@phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@fax", customer.Fax);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
                return false;
            }
        }

        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionStringMSSQL))
                {
                    sqlCon.Open();
                    string query = @"Update customers 
                                set CompanyName = @companyName, 
                                    ContactName = @contactName, 
                                    ContactTitle = @contactTitle, 
                                    Address = @address, 
                                    City = @city, 
                                    Region = @region,
                                    PostalCode = @postalCode, 
                                    Country = @country, 
                                    Phone = @phone,
                                    Fax = @fax
                                where customerId = @customerId";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);

                    cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);
                    cmd.Parameters.AddWithValue("@companyName", customer.CompanyName);
                    cmd.Parameters.AddWithValue("@contactName", customer.ContactName);
                    cmd.Parameters.AddWithValue("@contactTitle", customer.ContactTitle);
                    cmd.Parameters.AddWithValue("@address", customer.Address);
                    cmd.Parameters.AddWithValue("@city", customer.City);
                    cmd.Parameters.AddWithValue("@region", customer.Region);
                    cmd.Parameters.AddWithValue("@postalCode", customer.PostalCode);
                    cmd.Parameters.AddWithValue("@country", customer.Country);
                    cmd.Parameters.AddWithValue("@phone", customer.Phone);
                    cmd.Parameters.AddWithValue("@fax", customer.Fax);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
                return false;
            }
        }

        public bool DeleteCustomer(Customer customer)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionStringMSSQL))
                {
                    sqlCon.Open();
                    string query = @"Delete from customers where customerId = @customerId";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);

                    cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);

                    cmd.ExecuteNonQuery();
                    return true;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
                return false;
            }
        }

        public DataTable ShowAllInvoicesMSSQL()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionStringMSSQL))
                {
                    sqlCon.Open();
                    SqlDataAdapter sda = new SqlDataAdapter("SELECT * FROM dbo.Invoices", sqlCon);
                    sda.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }

            return dt;

        }

        public DataTable ShowInvoicesByCustomerIdMSSQL(Customer customer)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connectionStringMSSQL))
                {
                    sqlCon.Open();

                    string query = @"SELECT * FROM dbo.Invoices where customerId = @customerId";
                    SqlCommand cmd = new SqlCommand(query, sqlCon);

                    cmd.Parameters.AddWithValue("@customerId", customer.CustomerId);

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }

            return dt;

        }

        private List<Customer> FillCustomerList(DataTable dt)
        {
            return dt.AsEnumerable().Select(row =>
                                    new Customer
                                    {
                                        CustomerId = row.Field<string>("CustomerId"),
                                        CompanyName = row.Field<string>("CompanyName"),
                                        ContactName = row.Field<string>("ContactName"),
                                        ContactTitle = row.Field<string>("ContactTitle"),
                                        Address = row.Field<string>("Address"),
                                        City = row.Field<string>("City"),
                                        Region = row.Field<string>("Region"),
                                        PostalCode = row.Field<string>("PostalCode"),
                                        Country = row.Field<string>("Country"),
                                        Phone = row.Field<string>("Phone"),
                                        Fax = row.Field<string>("Fax")
                                    }).ToList();
        }

    }
}
