using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.ViewModels.Activity
{
    public class ActivityViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Activity")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }        
        public String AssignmentStatus { get; set; }
        public bool StudentMissedDeadline { get; set; }
        public bool StudentUploadedSolution { get; set; }
        public int NrOfDocuments { get; set; }
        public String EditDelDetailsId { get; set; }
        public String CollapseId { get; set; }
    }
}