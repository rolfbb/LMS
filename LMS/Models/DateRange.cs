using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public interface DateRange {
        DateTime StartDate { get; set; }
        DateTime EndDate { get; set; }
    }
}
