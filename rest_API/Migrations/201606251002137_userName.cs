namespace rest_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class userName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Invites", "Username", c => c.String());
            AlterColumn("dbo.Invites", "Friend_Username", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Invites", "Friend_Username", c => c.Int(nullable: false));
            AlterColumn("dbo.Invites", "Username", c => c.Int(nullable: false));
        }
    }
}
