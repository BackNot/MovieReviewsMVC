namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDataAnnotationsOnMovieModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "MovieName", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "PathToImage", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Description", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Category", c => c.String(nullable: false));
            AlterColumn("dbo.Movies", "Review", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Review", c => c.String());
            AlterColumn("dbo.Movies", "Category", c => c.String());
            AlterColumn("dbo.Movies", "Description", c => c.String());
            AlterColumn("dbo.Movies", "PathToImage", c => c.String());
            AlterColumn("dbo.Movies", "MovieName", c => c.String());
        }
    }
}
