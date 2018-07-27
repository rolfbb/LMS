using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int TypeId { get; set; }
        public int ModuleId { get; set; }

        // Relational properties
        public virtual Module Module { get; set; }
        public virtual ActivityType Type { get; set; }
    }
}