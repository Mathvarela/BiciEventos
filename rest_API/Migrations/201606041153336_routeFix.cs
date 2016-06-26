namespace rest_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class routeFix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Events", "Start_Time", c => c.String());
            AlterColumn("dbo.Events", "end_Longitude", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Events", "end_Longitude", c => c.String());
            DropColumn("dbo.Events", "Start_Time");
        }
    }
}
