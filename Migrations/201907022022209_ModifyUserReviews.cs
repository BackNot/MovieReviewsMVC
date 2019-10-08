namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyUserReviews : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "MovieName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "MovieName");
        }
    }
}
