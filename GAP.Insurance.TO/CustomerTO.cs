using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Customer name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Customer email
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        /// <summary>
        /// Customer insurances
        /// </summary>
        public ICollection<CustomerInsuranceTO> Insurances { get; set; }
    }
}
