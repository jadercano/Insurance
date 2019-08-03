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
        [Column(TypeName = "varchar(50)")]
        [Required]
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
