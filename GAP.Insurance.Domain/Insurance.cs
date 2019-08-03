using System;
using System.Collections.Generic;

namespace GAP.Insurance.Domain
{
    public partial class Insurance
    {
        public Insurance()
        {
            CustomerInsurance = new HashSet<CustomerInsurance>();
        }

        public Guid InsuranceId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CoverageType { get; set; }
        public decimal Coverage { get; set; }
        public double Cost { get; set; }
        public string RiskType { get; set; }
        public string Description { get; set; }

        public virtual ICollection<CustomerInsurance> CustomerInsurance { get; set; }
    }
}
