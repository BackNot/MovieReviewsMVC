namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMovieStarsToDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "Stars", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Stars", c => c.String());
        }
    }
}
