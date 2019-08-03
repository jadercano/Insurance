using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.TO
{
    /// <summary>
    /// Data container for a Customer instance
    /// </summary>
    public class CustomerTO
    {
        /// <summary>
        /// Customer Id
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Customer email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Customer insurances
        /// </summary>
        public ICollection<CustomerInsuranceTO> Insurances { get; set; }
    }
}
