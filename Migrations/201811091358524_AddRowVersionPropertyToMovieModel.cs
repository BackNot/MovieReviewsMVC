namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRowVersionPropertyToMovieModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "RowVersion");
        }
    }
}
