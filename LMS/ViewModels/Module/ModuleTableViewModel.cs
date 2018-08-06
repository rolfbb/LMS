using System;
using System.Collections.Generic;

namespace LMS.ViewModels.Module
{
    public class ModuleTableViewModel
    {
        public IEnumerable<ModuleViewModel> ModulesVM { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}