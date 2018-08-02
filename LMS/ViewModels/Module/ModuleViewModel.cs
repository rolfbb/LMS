using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LMS.Models;
namespace LMS.ViewModels.Module
{
    public class ModuleViewModel
    {
        public LMS.Models.Module Module { get; set; }
        public String EditDelDetailsId { get; set; }
        public String CollapseId { get; set; }
    }
}