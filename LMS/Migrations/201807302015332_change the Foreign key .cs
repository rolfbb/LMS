namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetheForeignkey : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Documents", "UserId", c => c.Int());
            DropColumn("dbo.Documents", "OwnerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Documents", "OwnerId", c => c.Int());
            DropColumn("dbo.Documents", "UserId");
        }
    }
}
