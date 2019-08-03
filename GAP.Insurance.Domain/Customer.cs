using System;
using System.Collections.Generic;

namespace GAP.Insurance.Domain
{
    public partial class Customer
    {
        public Customer()
        {
            CustomerInsurance = new HashSet<CustomerInsurance>();
        }

        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public virtual ICollection<CustomerInsurance> CustomerInsurance { get; set; }
    }
}
