using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels.Module
{
    public class ModuleTableViewModel
    {
        public IEnumerable<LMS.Models.Module> Modules { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}