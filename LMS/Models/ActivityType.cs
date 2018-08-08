using System.Collections.Generic;

namespace LMS.Models
{
	public class ActivityType
	{
		public string Description { get; set; }
		public int Id { get; set; }

		public virtual ICollection<Activity> Activitys { get; set; }
	}
}