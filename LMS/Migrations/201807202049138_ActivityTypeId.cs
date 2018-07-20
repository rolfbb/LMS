namespace LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ActivityTypeId : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Activities", name: "TypeId", newName: "ActivityTypeId");
            RenameIndex(table: "dbo.Activities", name: "IX_TypeId", newName: "IX_ActivityTypeId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Activities", name: "IX_ActivityTypeId", newName: "IX_TypeId");
            RenameColumn(table: "dbo.Activities", name: "ActivityTypeId", newName: "TypeId");
        }
    }
}
