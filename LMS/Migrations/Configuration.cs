namespace LMS.Migrations
{
    using LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net.Mail;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext db)
        {
            AddRoles(db);

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            AddTeachers(db, userManager);

            Course[] courses = AddCourses(db);
            AddStudents(db, courses, userManager);

            AddModules(db, courses);
        }

        private static void AddModules(ApplicationDbContext db, Course[] courses)
        {
            db.Modules.AddOrUpdate(m => m.Id,
               new Module()
               {
                   Id = 1,
                   Name = "Spring",
                   Description = "A framework",
                   StartDate = DateTime.Parse("2017-03-10"),
                   EndDate = DateTime.Parse("2017-04-10"),
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
               },
               new Module()
               {
                   Id = 4,
                   Name = "Bootstrap 3",
                   Description = "Dynamic Web pages",
                   StartDate = DateTime.Parse("2018-05-10"),
                   EndDate = DateTime.Parse("2018-05-16"),
                   CourseId = courses[5].Id
               },
               new Module()
               {
                   Id = 5,
                   Name = "Scrum",
                   Description = "Agile Teamwork",
                   StartDate = DateTime.Parse("2018-07-20"),
                   EndDate = DateTime.Parse("2018-08-10"),
                   CourseId = courses[5].Id
               },
               new Module()
               {
                   Id = 6,
                   Name = "MVC",
                   Description = "Model View Controller",
                   StartDate = DateTime.Parse("2018-05-17"),
                   EndDate = DateTime.Parse("2018-06-15"),
                   CourseId = courses[5].Id
               },
                 new Module()
                 {
                     Id = 7,
                     Name = "Java EE",
                     Description = "Java Enterprise Edition",
                     StartDate = DateTime.Parse("2018-03-10"),
                     EndDate = DateTime.Parse("2018-04-10"),
                     CourseId = courses[0].Id
                 }
               );
        }

        private static void AddStudents(ApplicationDbContext db, Course[] courses, UserManager<ApplicationUser> userManager)
        {
            var studentEmails = new[] { "Student.Studentsson@lexicon.se", "Dennis.Nilsson@lexicon.se", "Rolf.Bjarenstam@lexicon.se", "Ali.Salhab@lexicon.se", "Reza.Dagleh@lexicon.se" };

            foreach (var email in studentEmails)
            {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email, Name = GetNameFromEmail(email), CourseId = courses[5].Id };

                var result = userManager.Create(user, "aliali");
                userManager.AddToRole(user.Id, "Teacher");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }
        }

        private static void AddTeachers(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            var teacherEmails = new[] { "Teacher.Teachman@lexicon.se", "John.Hellman@lexicon.se", "Roger.Andersson@lexicon.se", "Adrian.Luzano@lexicon.se", "Dimitris.Bjorlingh@lexicon.se" };

            foreach (var email in teacherEmails)
            {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email, Name = GetNameFromEmail(email) };

                var result = userManager.Create(user, "aliali");
                userManager.AddToRole(user.Id, "Teacher");

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }
        }

        private static string GetNameFromEmail(string email)
        {
            MailAddress addr = new MailAddress(email);
            string userName = addr.User;
            string[] splitArr = userName.Split('.');
            if (splitArr.Length != 2)
            {
                userName = splitArr[0];
            }
            else
            {
                userName = splitArr[0] + " " + splitArr[1];
            }
            return userName;
        }

        private static Course[] AddCourses(ApplicationDbContext db)
        {
            var courses = new[] {
                new Course {Id = 1, Name = "Java", Description = "programming course" , StartDate=DateTime.Parse("2017-03-10"), EndDate=DateTime.Parse("2017-08-10")},
                new Course {Id = 2, Name = "Asp.NET", Description = "programming course" , StartDate=DateTime.Parse("2017-10-25"), EndDate=DateTime.Parse("2018-03-10")},
                new Course {Id = 3, Name = "Java English", Description = "programming course" , StartDate=DateTime.Parse("2018-04-03"), EndDate=DateTime.Parse("2018-08-10")},
                new Course {Id = 4, Name = "IT Support", Description = "IT course" , StartDate=DateTime.Parse("2018-04-03"), EndDate=DateTime.Parse("2018-10-10")},
                new Course {Id = 5, Name = "Java", Description = "programming course" , StartDate=DateTime.Parse("2018-03-27"), EndDate=DateTime.Parse("2018-08-10")},
                new Course {Id = 6, Name = "Asp.NET", Description = "programming course" , StartDate=DateTime.Parse("2018-04-03"), EndDate=DateTime.Parse("2018-08-10")}
            };

            db.Courses.AddOrUpdate(c => c.Id, courses);
            return courses;
        }

        private static void AddRoles(ApplicationDbContext db)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleManager = new RoleManager<IdentityRole>(roleStore);

            var roleNames = new[] { "Teacher", "Student" };
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
        }
    }
}

