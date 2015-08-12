namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PasswordChanged : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PasswordChangedUtc", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "PasswordChangedUtc");
        }
    }
}
