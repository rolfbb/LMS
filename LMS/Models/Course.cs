﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Relational properties
        public virtual ICollection<ApplicationUser> Students { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
    }
}