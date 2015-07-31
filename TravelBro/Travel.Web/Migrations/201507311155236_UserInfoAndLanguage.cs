namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserInfoAndLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AddColumn("dbo.AspNetUsers", "Patronymic", c => c.String());
            AddColumn("dbo.AspNetUsers", "LanguageId", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "LanguageId");
            AddForeignKey("dbo.AspNetUsers", "LanguageId", "dbo.Languages", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "LanguageId", "dbo.Languages");
            DropIndex("dbo.AspNetUsers", new[] { "LanguageId" });
            DropColumn("dbo.AspNetUsers", "LanguageId");
            DropColumn("dbo.AspNetUsers", "Patronymic");
            DropColumn("dbo.AspNetUsers", "Surname");
            DropColumn("dbo.AspNetUsers", "Name");
            DropTable("dbo.Languages");
        }
    }
}
