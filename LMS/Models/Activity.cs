using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LMS.Models
{
    public class Activity : DateRange
    {
        public int Id { get; set; }
        [Required, Display(Name = "Activity")]
        public string Name { get; set; }
        [Required,DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name="Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        
        public int TypeId { get; set; }
        public int ModuleId { get; set; }

        // Relational properties
        public virtual Module Module { get; set; }
        public virtual ActivityType Type { get; set; }
        public virtual ICollection<Document> Documents { get; set; }
    }
}