namespace LMS.Migrations
{
	using LMS.Models;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.EntityFramework;
	using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext db)
        {
			var roleStore = new RoleStore<IdentityRole>(db);
			var roleManager = new RoleManager<IdentityRole>(roleStore);

			var roleNames = new[] { "Teacher","Student" };
			foreach (var roleName in roleNames)
			{
				if (db.Roles.Any(r => r.Name == roleName)) continue;
				var role = new IdentityRole { Name = roleName };
				var result = roleManager.Create(role);
				if (!result.Succeeded)
				{
					throw new Exception(string.Join("\n", result.Errors));
				}
			}

			var userStore = new UserStore<ApplicationUser>(db);
			var userManager = new UserManager<ApplicationUser>(userStore);

			var emails = new[] { "Teacher@lexicon.se", "Student@lexicon.se" };

			foreach (var email in emails)
			{
				if (db.Users.Any(u => u.UserName == email)) continue;
				var user = new ApplicationUser { UserName = email, Email = email};
				var result = userManager.Create(user, "aliali");
				if (!result.Succeeded)
				{
					throw new Exception(string.Join("\n", result.Errors));
				}
			}

			var TeacherUser = userManager.FindByName("Teacher@lexicon.se");
			userManager.AddToRole(TeacherUser.Id, "Teacher");
			var StudentUser = userManager.FindByName("Student@lexicon.se");
			userManager.AddToRole(StudentUser.Id, "Student");

			var courses = new[] {
				new Course {Id =1, Name = "Java", Description = "programming course" , StartDate=DateTime.Parse("2017-03-10"),EndDate=DateTime.Parse("2017-08-10")},
				new Course {Id =2, Name = "Asp.NET", Description = "programming course" , StartDate=DateTime.Parse("2017-10-25"),EndDate=DateTime.Parse("2018-03-10")},
                new Course {Id =3, Name = "Java English", Description = "programming course" , StartDate=DateTime.Now,EndDate=DateTime.Parse("2018-08-10")},
				new Course {Id =4, Name = "IT Support", Description = "IT course" , StartDate=DateTime.Now,EndDate=DateTime.Parse("2018-10-10")},
                new Course {Id =5, Name = "Java", Description = "programming course" , StartDate=DateTime.Now,EndDate=DateTime.Parse("2018-08-10")},
                new Course {Id =6, Name = "Asp.NET", Description = "programming course" , StartDate=DateTime.Now,EndDate=DateTime.Parse("2018-08-10")}
            };

			//db.Courses.AddOrUpdate(s => new { s.Name, s.Description }, courses);
            db.Courses.AddOrUpdate(s => s.Id, courses);

            db.Modules.AddOrUpdate(m => m.Id,
               new Module()
               {
                   Id = 1,
                   Name = "Spring",
                   Description = "A framework",
                   StartDate = DateTime.Parse("2017-03-10"),
                   EndDate = DateTime.Parse("2018-03-10"),
                   CourseId = courses[0].Id
               },
               new Module()
               {
                   Id = 2,
                   Name = "C#",
                   Description = "Object Orientation, LINQ",
                   StartDate = DateTime.Parse("2017-10-25"),
                   EndDate = DateTime.Parse("2017-11-29"),
                   CourseId = courses[1].Id
               },
               new Module()
               {
                   Id = 3,
                   Name = "MVC",
                   Description = "Model View Controller",
                   StartDate = DateTime.Parse("2017-12-01"),
                   EndDate = DateTime.Parse("2018-01-01"),
                   CourseId = courses[1].Id
               }
               );
		}
	}
}