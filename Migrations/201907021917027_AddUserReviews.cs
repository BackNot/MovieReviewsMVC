namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserReviews : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Movie_MovieId = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Movies", t => t.Movie_MovieId)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Movie_MovieId)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserReviews", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserReviews", "Movie_MovieId", "dbo.Movies");
            DropIndex("dbo.UserReviews", new[] { "User_Id" });
            DropIndex("dbo.UserReviews", new[] { "Movie_MovieId" });
            DropTable("dbo.UserReviews");
        }
    }
}
