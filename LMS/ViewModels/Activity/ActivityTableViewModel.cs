using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels.Activity
{
    public class ActivityTableViewModel
    {
        public IEnumerable<ActivityViewModel> ActivitiesVM { get; set; }
        public int ModuleId { get; set; }
        public int CourseId { get; set; }
    }
}