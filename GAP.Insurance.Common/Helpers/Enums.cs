using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GAP.Insurance.Common.Helpers
{
    /// <summary>
    /// Categories of the log entries
    /// </summary>
    public enum LogCategory
    {
        [Description("ERROR")]
        Error,
        [Description("WARN")]
        Warning,
        [Description("INFO")]
        Information,
        [Description("DEBUG")]
        Debug,
        [Description("FATAL")]
        Fatal
    }

    public enum CustomerInsuranceStatus
    {
        [Description("Active")]
        Active,
        [Description("Canceled")]
        Canceled
    }
}
