using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.ViewModels.Documents
{
    public class UploadDocumentViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public int CourseId { get; set; }
        public int ModuleId { get; set; }
        public string UpdateTarget { get; set; }
    }
}