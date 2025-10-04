using System.Collections.Generic;
using Data;
using Entity;

namespace Business
{
    public class BCustomer
    {
        public List<Customer> Read()
        {
            return new DCustomer().Read();
        }

        public void Create(Customer customer)
        {
            new DCustomer().Create(customer);
        }

        public void Update(Customer c) => new DCustomer().Update(c);
        public void Delete(int id) => new DCustomer().SoftDelete(id);
    }
}
