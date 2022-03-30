using System;
using System.Drawing;
using System.Windows.Forms;

namespace TaskProject
{
    public partial class FormMain : Form
    {

        ICustomerRepository db; 

        public FormMain()
        {
            InitializeComponent();
            PopulateDBComboBox();

            dgvCustomers.Visible = false;
            dgvInvoices.Visible = false;

            EnableAddButton();

        }

        #region Methods
        private void PopulateDBComboBox()
        {
            cbxDB.Items.Insert(0, "Select");
            cbxDB.Items.Insert(1, "MSSQL");
            cbxDB.Items.Insert(2, "MySQL");
            cbxDB.SelectedIndex = 0;
            cbxDB.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void EnableAddButton()
        {
            btnAddCustomer.Visible = dgvCustomers.Visible;
        }

        private void BindGridWithCustomer()
        {
            if (cbxDB.SelectedIndex > 0)
            {
                dgvInvoices.Visible = false;
                dgvCustomers.Visible = true;

                dgvCustomers.DataSource = db.ShowAllCustomer();
            }
            else
            { 
                MessageBox.Show("Please select a database.", "Message");
            }

        }

        private void BindGridWithInvoices()
        {

            if (cbxDB.SelectedIndex > 0)
            {
                dgvCustomers.Visible = false;
                dgvInvoices.Visible = true;

                //if (cbxDB.SelectedIndex == 1)
                //{
                //    dgvInvoices.DataSource = db.ShowAllInvoicesMSSQL();
                //}
                //else if (cbxDB.SelectedIndex == 2)
                //{
                //    dgvInvoices.DataSource = db.ShowAllInvoicesMySQL();
                //}
            }
            else
                MessageBox.Show("Please select a database.", "Message");
        }

        #endregion Methods


        #region Events

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            if (cbxDB.SelectedIndex > 0)
            { 
                btnCustomer.Font = new Font(btnCustomer.Font.Name, btnCustomer.Font.Size, FontStyle.Bold);
                btnInvoice.Font = new Font(btnInvoice.Font.Name, btnInvoice.Font.Size, FontStyle.Regular);
            }

            BindGridWithCustomer();
            EnableAddButton();
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            if (cbxDB.SelectedIndex > 0)
            {
                btnCustomer.Font = new Font(btnCustomer.Font.Name, btnCustomer.Font.Size, FontStyle.Regular);
                btnInvoice.Font = new Font(btnInvoice.Font.Name, btnInvoice.Font.Size, FontStyle.Bold);
            }

            BindGridWithInvoices();
            EnableAddButton();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            FormCustomer frmCustomer = new FormCustomer(db);
            frmCustomer.ShowDialog();

            BindGridWithCustomer();
        }

        private void cbxDB_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvCustomers.Visible = false;
            dgvInvoices.Visible = false;

            btnCustomer.Font = new Font(btnCustomer.Font.Name, btnCustomer.Font.Size, FontStyle.Regular);
            btnInvoice.Font = new Font(btnInvoice.Font.Name, btnInvoice.Font.Size, FontStyle.Regular);

            if (cbxDB.SelectedIndex == 1)
                db = new DatabaseMssql();
            else if (cbxDB.SelectedIndex == 2)
                db = new DatabaseMySql();

            EnableAddButton();
        }

        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Customer customer = new Customer();
                if (dgvCustomers.Columns[e.ColumnIndex].Name == "btnDelete")
                {
                    bool isDeleted = false;
                    DialogResult confirm = MessageBox.Show("Are you sure to delete the customer?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirm == DialogResult.Yes)
                    {
                        customer.CustomerId = dgvCustomers.Rows[e.RowIndex].Cells["colCustomerId"].Value.ToString();

                        if (cbxDB.SelectedIndex == 1)
                        {
                            isDeleted = db.DeleteCustomer(customer);
                        }
                        else if (cbxDB.SelectedIndex == 2)
                        {
                            isDeleted = db.DeleteCustomer(customer);
                        }

                        if (isDeleted)
                        {
                            MessageBox.Show("Delete successfully.", "Info");
                            BindGridWithCustomer();
                        }

                    }
                }
                else if (dgvCustomers.Columns[e.ColumnIndex].Name == "btnUpdate")
                {
                    bool isUpdate = true;
                    customer.CustomerId = dgvCustomers.Rows[e.RowIndex].Cells["colCustomerId"].Value.ToString();
                    customer.CompanyName = (dgvCustomers.Rows[e.RowIndex].Cells["colCompanyName"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colCompanyName"].Value.ToString();
                    customer.ContactName = (dgvCustomers.Rows[e.RowIndex].Cells["colContactName"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colContactName"].Value.ToString();
                    customer.ContactTitle = (dgvCustomers.Rows[e.RowIndex].Cells["colContactTitle"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colContactTitle"].Value.ToString();
                    customer.Address = (dgvCustomers.Rows[e.RowIndex].Cells["colAddress"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colAddress"].Value.ToString();
                    customer.City = (dgvCustomers.Rows[e.RowIndex].Cells["colCity"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colCity"].Value.ToString();
                    customer.Region = (dgvCustomers.Rows[e.RowIndex].Cells["colRegion"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colRegion"].Value.ToString();
                    customer.PostalCode = (dgvCustomers.Rows[e.RowIndex].Cells["colPostalCode"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colPostalCode"].Value.ToString();
                    customer.Country = (dgvCustomers.Rows[e.RowIndex].Cells["colCountry"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colCountry"].Value.ToString();
                    customer.Phone = (dgvCustomers.Rows[e.RowIndex].Cells["colPhone"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colPhone"].Value.ToString();
                    customer.Fax = (dgvCustomers.Rows[e.RowIndex].Cells["colFax"].Value == null) ? string.Empty : dgvCustomers.Rows[e.RowIndex].Cells["colFax"].Value.ToString();

                    FormCustomer frmCustomer = new FormCustomer(db, customer, isUpdate);
                    frmCustomer.ShowDialog();

                    BindGridWithCustomer();
                }
                else 
                {

                    customer.CustomerId = dgvCustomers.Rows[e.RowIndex].Cells["colCustomerId"].Value.ToString();

                    dgvCustomers.Visible = false;
                    dgvInvoices.Visible = true;
                    dgvInvoices.DataSource = db.ShowInvoicesByCustomerIdMSSQL(customer);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error");
            }
            
        }

        #endregion Events
    }

}
