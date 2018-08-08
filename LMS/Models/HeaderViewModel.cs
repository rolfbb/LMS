using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class HeaderViewModel
    {
        public bool Empty { get; set; }
        public String CourseName { get; set; }
        public int CourseId;
        public String Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}