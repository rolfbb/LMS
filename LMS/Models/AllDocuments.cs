using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
	public class AllDocuments
	{
		public List<Document> StudentDoc { get; set; }
		public List<Document> TeacherDoc { get; set; }
		public List<ApplicationUser> UL { get; set; }
	}
}