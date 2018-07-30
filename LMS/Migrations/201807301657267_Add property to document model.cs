namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addpropertytodocumentmodel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "FileContent", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Documents", "FileContent");
        }
    }
}
