using System;
using System.Collections.Generic;
using System.Text;

namespace GAP.Insurance.TO
{
    /// <summary>
    /// Data container for an Insurance instance
    /// </summary>
    public class InsuranceTO
    {
        /// <summary>
        /// Insurance Id
        /// </summary>
        public Guid InsuranceId { get; set; }

        /// <summary>
        /// Insurance name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Insurance start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Insurance end date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Insurance coverage type
        /// </summary>
        public string CoverageType { get; set; }

        /// <summary>
        /// Insurance coverage
        /// </summary>
        public decimal Coverage { get; set; }

        /// <summary>
        /// Insurance cost
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Insurance risk type
        /// </summary>
        public string RiskType { get; set; }

        /// <summary>
        /// Insurance description
        /// </summary>
        public string Description { get; set; }
    }
}
