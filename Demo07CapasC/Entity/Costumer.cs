using System;

namespace Entity
{
    public class Customer
    {
        public int CustomerId { get; set; }   // customer_id
        public string Name { get; set; }      // name
        public string Address { get; set; }   // address (nullable)
        public string Phone { get; set; }     // phone (nullable)
        public bool Active { get; set; }      // active
    }
}
