using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.TO
{
    /// <summary>
    /// Data container for a Customer instance
    /// </summary>
    public class CustomerInsuranceTO
    {
        public InsuranceTO Insurance { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Status { get; set; }
    }
}
