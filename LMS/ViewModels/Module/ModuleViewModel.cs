using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LMS.Models;
using LMS.ViewModels.Activity;

namespace LMS.ViewModels.Module
{
    public class ModuleViewModel
    {
        public int Id { get; set; }
        //public LMS.Models.Module Module { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }     
        public String EditDelDetailsId { get; set; }
        public String CollapseId { get; set; }
        public String AssignmentStatus { get; set; }
        public int NrOfDocuments { get; set; }
        public bool DelayedAssignMent { get; set; }
        public List<ActivityViewModel> ActivitiesVM { get; set; }
    }
}