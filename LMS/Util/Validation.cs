using LMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Util
{
    public class Validation
    {
        public static bool DateRangeValidation(Controller ctr,DateRange dateRange,DateRange subRange) {
            bool validationOk = true;
            if (subRange.StartDate < dateRange.StartDate)
            {
                ctr.ModelState.AddModelError("StartDate", "Earliest allowed start date is " + dateRange.StartDate);
                validationOk = false;
            }
            if (subRange.EndDate > dateRange.EndDate)
            {
                ctr.ModelState.AddModelError("EndDate", "Latest allowed end date is " + dateRange.StartDate);
                validationOk = false;
            }
            if (subRange.StartDate > dateRange.EndDate)
            {
                ctr.ModelState.AddModelError("EndDate", "End date can't be earlier than start date");
                validationOk = false;
            }
            return validationOk;
        }        
    }
}