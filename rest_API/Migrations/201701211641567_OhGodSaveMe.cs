namespace rest_API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OhGodSaveMe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Event_Going",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Events",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Start_Date = c.DateTime(nullable: false),
                        End_Date = c.DateTime(nullable: false),
                        start_Latitude = c.Double(nullable: false),
                        end_Latitude = c.Double(nullable: false),
                        start_Longitude = c.Double(nullable: false),
                        end_Longitude = c.Double(nullable: false),
                        Start_Time = c.String(),
                        End_Time = c.String(),
                        Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Friends",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Friend_Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EventId = c.Int(nullable: false),
                        Username = c.String(),
                        Friend_Username = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        Register_date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Users");
            DropTable("dbo.Invites");
            DropTable("dbo.Friends");
            DropTable("dbo.Events");
            DropTable("dbo.Event_Going");
        }
    }
}
