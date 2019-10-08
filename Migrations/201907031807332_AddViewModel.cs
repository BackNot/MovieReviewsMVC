namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddViewModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserReviews", "MovieName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserReviews", "MovieName", c => c.String());
        }
    }
}
