namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addvalidationfields : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Activities", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Activities", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Modules", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Modules", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Courses", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Courses", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Name", c => c.String());
            AlterColumn("dbo.Courses", "Description", c => c.String());
            AlterColumn("dbo.Courses", "Name", c => c.String());
            AlterColumn("dbo.Modules", "Description", c => c.String());
            AlterColumn("dbo.Modules", "Name", c => c.String());
            AlterColumn("dbo.Activities", "Description", c => c.String());
            AlterColumn("dbo.Activities", "Name", c => c.String());
        }
    }
}
