using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]    
        public DateTime EndDate { get; set; }

        public int CourseId { get; set; }

        // Relational properties
        public virtual Course Course { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}