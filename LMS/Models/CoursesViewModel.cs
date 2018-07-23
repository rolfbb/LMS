using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class CoursesViewModel
    {
        public IEnumerable<Course> Courses { get; set; }
        public HeaderViewModel Header { get; set; }
        public bool ActiveCourses { get; set; }
    }
}