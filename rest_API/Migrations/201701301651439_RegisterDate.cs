namespace rest_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RegisterDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "RegisterDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "Register_date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Register_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Users", "RegisterDate");
        }
    }
}
