using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public class HeaderViewModel
    {
        public String CourseName { get; set; }
        public String Description { get; set; }
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
    }
}