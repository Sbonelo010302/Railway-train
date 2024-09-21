namespace RailwayApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDay2dayRoutes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Routes", "TrainId", "dbo.Trains");
            DropIndex("dbo.Routes", new[] { "TrainId" });
            CreateTable(
                "dbo.DayToDayTrainRoutes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Day = c.DateTime(nullable: false),
                        DepatureTime = c.String(nullable: false),
                        ArrivalTime = c.String(),
                        TrainId = c.Int(nullable: false),
                        RouteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Routes", t => t.RouteId)
                .ForeignKey("dbo.Trains", t => t.TrainId)
                .Index(t => t.TrainId)
                .Index(t => t.RouteId);
            
            AddColumn("dbo.Routes", "Hours", c => c.Int(nullable: false));
            AddColumn("dbo.Routes", "Minutes", c => c.Int(nullable: false));
            DropColumn("dbo.Routes", "TrainId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routes", "TrainId", c => c.Int());
            DropForeignKey("dbo.DayToDayTrainRoutes", "TrainId", "dbo.Trains");
            DropForeignKey("dbo.DayToDayTrainRoutes", "RouteId", "dbo.Routes");
            DropIndex("dbo.DayToDayTrainRoutes", new[] { "RouteId" });
            DropIndex("dbo.DayToDayTrainRoutes", new[] { "TrainId" });
            DropColumn("dbo.Routes", "Minutes");
            DropColumn("dbo.Routes", "Hours");
            DropTable("dbo.DayToDayTrainRoutes");
            CreateIndex("dbo.Routes", "TrainId");
            AddForeignKey("dbo.Routes", "TrainId", "dbo.Trains", "Id");
        }
    }
}
