using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LMS.ViewModels.Module;

namespace LMS.ViewModels.Course
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        [Required, Display(Name = "Course")]
        public string Name { get; set; }
        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public bool DelayedAssignMent { get; set; }
        public List<ModuleViewModel> ModulesVM { get; set; }
    }
}