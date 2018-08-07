namespace LMS.Migrations
{
    using LMS.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Net.Mail;

    internal sealed class Configuration : DbMigrationsConfiguration<LMS.Models.ApplicationDbContext>
    {
        public Configuration()
        {
			AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LMS.Models.ApplicationDbContext db)
        {
            AddRoles(db);

            var userStore = new UserStore<ApplicationUser>(db);
            var userManager = new UserManager<ApplicationUser>(userStore);

            AddTeachers(db, userManager);

            Course[] courses = AddCourses(db);

            AddStudents(db, courses, userManager);

            Module[] modules = AddModules(db, courses);
            ActivityType[] activityTypes = AddActivityTypes(db);
            Activity[] activities = AddActivities(db, activityTypes, modules);
            Activity[] activities2 = AddActivities2(db, activityTypes, modules);
        }

        private static Course[] AddCourses(ApplicationDbContext db)
        {
            var courses = new[] {
                new Course { Name = "Java", Description = "programming course" , StartDate=DateTime.Parse("2017-03-10"), EndDate=DateTime.Parse("2017-08-10") },
                new Course { Name = "Asp.NET", Description = "programming course" , StartDate=DateTime.Parse("2017-10-25"), EndDate=DateTime.Parse("2018-03-10") },
                new Course { Name = "Java English", Description = "programming course" , StartDate=DateTime.Parse("2018-04-03"), EndDate=DateTime.Parse("2018-08-10") },
                new Course { Name = "IT Support", Description = "IT course" , StartDate=DateTime.Parse("2018-04-03"), EndDate=DateTime.Parse("2018-10-10") },
                new Course { Name = "Java", Description = "programming course" , StartDate=DateTime.Parse("2018-03-27"), EndDate=DateTime.Parse("2018-08-10") },
                new Course { Name = "Asp.NET", Description = "programming course" , StartDate=DateTime.Parse("2018-04-03"), EndDate=DateTime.Parse("2018-08-10") }
            };

            db.Courses.AddOrUpdate(c => new { c.Name, c.StartDate }, courses);
            db.SaveChanges();
            return courses;
        }

        private static Module[] AddModules(ApplicationDbContext db, Course[] courses)
        {
            var modules = new[] {
                new Module { Name = "Spring", Description = "A framework" , StartDate=DateTime.Parse("2017-03-10"), EndDate=DateTime.Parse("2017-04-10"), CourseId = courses[0].Id },
                new Module { Name = "C#", Description = "Object Orientation, LINQ" , StartDate=DateTime.Parse("2017-10-25"), EndDate=DateTime.Parse("2017-11-29"), CourseId = courses[1].Id },
                new Module { Name = "MVC", Description = "Model View Controller" , StartDate=DateTime.Parse("2017-12-01"), EndDate=DateTime.Parse("2018-01-01"), CourseId = courses[1].Id },
                new Module { Name = "Bootstrap 3", Description = "Dynamic Web pages" , StartDate=DateTime.Parse("2018-05-10"), EndDate=DateTime.Parse("2018-05-16"), CourseId = courses[5].Id },
                new Module { Name = "Scrum", Description = "Agile Teamwork" , StartDate=DateTime.Parse("2018-07-20"), EndDate=DateTime.Parse("2018-08-10"), CourseId = courses[5].Id },
                new Module { Name = "MVC", Description = "Model View Controller" , StartDate=DateTime.Parse("2018-05-17"), EndDate=DateTime.Parse("2018-06-15"), CourseId = courses[5].Id },
                new Module { Name = "Java EE", Description = "Java Enterprice Edition" , StartDate=DateTime.Parse("2018-03-10"), EndDate=DateTime.Parse("2018-04-10"), CourseId = courses[5].Id },
            };

            db.Modules.AddOrUpdate(m => new { m.Name, m.StartDate }, modules);
            db.SaveChanges();
            return modules;
        }

        private static void AddStudents(ApplicationDbContext db, Course[] courses, UserManager<ApplicationUser> userManager)
        {
            var studentEmails = new[] { "Student.Studentsson@lexicon.se", "Dennis.Nilsson@lexicon.se", "Rolf.Bjarenstam@lexicon.se", "Ali.Salhab@lexicon.se", "Reza.Dagleh@lexicon.se" };

            foreach (var email in studentEmails)
            {
                if (db.Users.Any(u => u.UserName == email)) continue;
                var user = new ApplicationUser { UserName = email, Email = email, Name = GetNameFromEmail(email), CourseId = courses[5].Id };

                var result = userManager.Create(user, "aliali");
                userManager.AddToRole(user.Id, "Student");

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

        private static ActivityType[] AddActivityTypes(ApplicationDbContext db)
        {
            var activityTypes = new[] {
                new ActivityType { Description = "CourseActivity"},
                new ActivityType { Description = "Assignment"},
            };

            db.ActivityTypes.AddOrUpdate(a => a.Description, activityTypes);
            db.SaveChanges();
            return activityTypes;
        }
        private static Random gen = new Random();

        private static Activity[] AddActivities2(ApplicationDbContext db, ActivityType[] activityTypes, Module[] modules)
        {
            TimeSpan twoDays = new TimeSpan(48, 0, 0);
            var activitiesList = new List<Activity>();
            foreach (var module in modules) {
                var actStartDate = module.StartDate;
                var actEndDate = actStartDate.AddDays(2);
                var activityCount = 0;
                while (actEndDate < module.EndDate) {
                    Activity activity = new Activity {
                        Name = "Activity" + activityCount,
                        Description = "Descr " + module.Name,
                        TypeId = activityTypes[gen.Next(activityTypes.Length)].Id,
                        ModuleId = module.Id,
                        StartDate = actStartDate,
                        EndDate = actEndDate
                    };
                    activitiesList.Add(activity);
                    actStartDate = actEndDate.AddDays(1);
                    actEndDate = actStartDate.AddDays(gen.Next(3));
                    activityCount++;
                }
            }
            var activities = activitiesList.ToArray();
            db.Activities.AddOrUpdate(a => new { a.Name, a.ModuleId },activities );
            db.SaveChanges();
            return activities;
        }


        private static Activity[] AddActivities(ApplicationDbContext db, ActivityType[] activityTypes, Module[] modules)
        {
            var activities = new[] {
                new Activity { Name = "Java", Description = "Java Code-Along", TypeId = activityTypes[1].Id, ModuleId = modules[0].Id,  StartDate=DateTime.Parse("2017-03-10"), EndDate=DateTime.Parse("2017-08-10")},
                new Activity { Name = "C#", Description = "C# Code-Along", TypeId = activityTypes[1].Id, ModuleId = modules[6].Id,  StartDate=DateTime.Parse("2017-03-10"), EndDate=DateTime.Parse("2017-08-10")},
            };

            db.Activities.AddOrUpdate(a => new { a.Name, a.StartDate }, activities);
            db.SaveChanges();
            return activities;
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

