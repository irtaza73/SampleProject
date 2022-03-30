using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TaskProject
{
    public partial class FormCustomer : Form
    {
        ICustomerRepository db;

        private bool isUpdate;
        public FormCustomer()
        {
            InitializeComponent();
        }

        public FormCustomer(ICustomerRepository database)
        {
            InitializeComponent();

            db = database;
        }

        public FormCustomer(ICustomerRepository database, Customer customer, bool action)
        {
            InitializeComponent();

            db = database;

            txtCustomerId.Text = customer.CustomerId;
            txtCompanyName.Text = customer.CompanyName;
            txtContactName.Text = customer.ContactName;
            txtContactTitle.Text = customer.ContactTitle;
            txtAddress.Text = customer.Address;
            txtCity.Text = customer.City;
            txtRegion.Text = customer.Region;
            txtPostalCode.Text = customer.PostalCode;
            txtCountry.Text = customer.Country;
            txtPhone.Text = customer.Phone;
            txtFax.Text = customer.Fax;

            isUpdate = action;
            txtCustomerId.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            bool isValid = ValidateCustomerData();
            bool isSaved = false;

            if (isValid)
            {
                Customer customer = new Customer();

                customer.CustomerId = txtCustomerId.Text;
                customer.CompanyName = txtCompanyName.Text;
                customer.ContactName = txtContactName.Text;
                customer.ContactTitle = txtContactTitle.Text;
                customer.Address = txtAddress.Text;
                customer.City = txtCity.Text;
                customer.Region = txtRegion.Text;
                customer.PostalCode = txtPostalCode.Text;
                customer.Country = txtCountry.Text;
                customer.Phone = txtPhone.Text;
                customer.Fax = txtFax.Text;

                if (!isUpdate)
                {
                    isSaved = db.InsertCustomer(customer);
                }
                else
                {
                    isSaved = db.UpdateCustomer(customer);
                }

                if (isSaved)
                { 
                    MessageBox.Show("Save successfully.", "Info");
                    this.Close();
                }

            }

        }

        private bool ValidateCustomerData()
        {
            try
            {

                Regex reg = new Regex(@"^(?=.*[0-9])[- +()0-9]+$");
                

                if (string.IsNullOrEmpty(txtCustomerId.Text))
                {
                    MessageBox.Show("Please insert Customer Id.", "Error");
                    return false;
                }

                if (txtCustomerId.Text.Length > 5)
                {
                    MessageBox.Show("The length of Customer Id cannot be greater than 5.", "Error");
                    return false;
                }

                if (string.IsNullOrEmpty(txtCompanyName.Text))
                {
                    MessageBox.Show("Please insert Company Name.", "Error");
                    return false;
                }

                if (txtPostalCode.Text.Length > 10)
                {
                    MessageBox.Show("The length of Postal Code cannot be greater than 10.", "Error");
                    return false;
                }

                if (!string.IsNullOrEmpty(txtPhone.Text) && (!reg.IsMatch(txtPhone.Text)))
                {
                    MessageBox.Show("Phone number format is incorrect.", "Error");
                    return false;
                }

                if (!string.IsNullOrEmpty(txtFax.Text) && (!reg.IsMatch(txtFax.Text)))
                {
                    MessageBox.Show("Fax number format is incorrect.", "Error");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
                return false;
            }
                   
        }
    }
}
