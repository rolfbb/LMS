namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingthefilecontentextension : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Documents", "FileContent", c => c.Binary());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Documents", "FileContent", c => c.Byte(nullable: false));
        }
    }
}
