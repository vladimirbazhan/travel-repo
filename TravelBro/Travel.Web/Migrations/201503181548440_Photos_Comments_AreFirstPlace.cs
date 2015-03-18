namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Photos_Comments_AreFirstPlace : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.PhotosToComments", newName: "CommentsToPhotos");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CommentsToPhotos", newName: "PhotosToComments");
        }
    }
}
