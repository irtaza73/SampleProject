using System.Collections.Generic;
using System.Data;

namespace TaskProject
{
    public interface ICustomerRepository
    {
        List<Customer> ShowAllCustomer();
        bool InsertCustomer(Customer customer);

        bool UpdateCustomer(Customer customer);

        bool DeleteCustomer(Customer customer);

        DataTable ShowInvoicesByCustomerIdMSSQL(Customer customer);

    }
}
