namespace MovieReviews.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyUserReview : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserReviews", "Review", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserReviews", "Review");
        }
    }
}
