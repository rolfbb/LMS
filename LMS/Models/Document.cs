using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMS.Models
{
	public class Document
	{
		public int Id { get; set; }
		[Required,StringLength(30)]
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime TimeStamp { get; set; }


        public string UserId { get; set; }
		public int? CourseId { get; set; }
		public int? ModuleId { get; set; }
		public int? ActivityId { get; set; }
		public byte[] FileContent { get; set; }


		// Relational properties
		public virtual ApplicationUser User { get; set; }
		public virtual Course Course { get; set; }
		public virtual Module Module { get; set; }
		public virtual Activity Activity { get; set; }
	}
}