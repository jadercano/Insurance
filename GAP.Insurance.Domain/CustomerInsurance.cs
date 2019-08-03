using System;
using System.Collections.Generic;

namespace GAP.Insurance.Domain
{
    public partial class CustomerInsurance
    {
        public Guid CustomerId { get; set; }
        public Guid InsuranceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Insurance Insurance { get; set; }
    }
}
