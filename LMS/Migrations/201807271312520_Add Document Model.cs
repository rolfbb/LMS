namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocumentModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Documents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        OwnerId = c.Int(),
                        CourseId = c.Int(),
                        ModuleId = c.Int(),
                        ActivityId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activities", t => t.ActivityId)
                .ForeignKey("dbo.Courses", t => t.CourseId)
                .ForeignKey("dbo.Modules", t => t.ModuleId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.CourseId)
                .Index(t => t.ModuleId)
                .Index(t => t.ActivityId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Documents", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Documents", "ModuleId", "dbo.Modules");
            DropForeignKey("dbo.Documents", "CourseId", "dbo.Courses");
            DropForeignKey("dbo.Documents", "ActivityId", "dbo.Activities");
            DropIndex("dbo.Documents", new[] { "User_Id" });
            DropIndex("dbo.Documents", new[] { "ActivityId" });
            DropIndex("dbo.Documents", new[] { "ModuleId" });
            DropIndex("dbo.Documents", new[] { "CourseId" });
            DropTable("dbo.Documents");
        }
    }
}
