namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeuseridvaluetononnullableandsetthevalidationfornametoberequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "Name", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Documents", "UserId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "UserId", c => c.Int());
            AlterColumn("dbo.Documents", "Name", c => c.String());
        }
    }
}
