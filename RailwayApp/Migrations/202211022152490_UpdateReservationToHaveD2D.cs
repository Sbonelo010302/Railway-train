namespace RailwayApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReservationToHaveD2D : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reservations", "DayToDayTrainRouteId", c => c.Int());
            CreateIndex("dbo.Reservations", "DayToDayTrainRouteId");
            AddForeignKey("dbo.Reservations", "DayToDayTrainRouteId", "dbo.DayToDayTrainRoutes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "DayToDayTrainRouteId", "dbo.DayToDayTrainRoutes");
            DropIndex("dbo.Reservations", new[] { "DayToDayTrainRouteId" });
            DropColumn("dbo.Reservations", "DayToDayTrainRouteId");
        }
    }
}
