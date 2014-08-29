namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Drop_Place_Id_FK_in_Routes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Routes", "Place_Id", "dbo.Places");
            DropIndex("dbo.Routes", new[] { "Place_Id" });
            DropColumn("dbo.Routes", "Place_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routes", "Place_Id", c => c.Int());
            CreateIndex("dbo.Routes", "Place_Id");
            AddForeignKey("dbo.Routes", "Place_Id", "dbo.Places", "Id");
        }
    }
}
