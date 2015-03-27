namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransTypeAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TransTypes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Routes", "TransTypeId", c => c.Int(nullable: false));
            CreateIndex("dbo.Routes", "TransTypeId");
            AddForeignKey("dbo.Routes", "TransTypeId", "dbo.TransTypes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routes", "TransTypeId", "dbo.TransTypes");
            DropIndex("dbo.Routes", new[] { "TransTypeId" });
            DropColumn("dbo.Routes", "TransTypeId");
            DropTable("dbo.TransTypes");
        }
    }
}
