using System.Data.Entity.Migrations;

namespace Chicken.DAL.Migrations
{
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Photo_Id = c.Int(),
                        Post_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Photos", t => t.Photo_Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .Index(t => t.Photo_Id)
                .Index(t => t.Post_Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhotoId = c.String(),
                        Photo75Url = c.String(),
                        Photo130Url = c.String(),
                        Photo604Url = c.String(),
                        Photo807Url = c.String(),
                        Photo1280Url = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostId = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Text = c.String(),
                        CommentsCount = c.Int(nullable: false),
                        LikesCount = c.Int(nullable: false),
                        Avatar = c.String(),
                        IsSpam = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CommentId = c.String(),
                        Text = c.String(),
                        Date = c.DateTime(nullable: false),
                        ProfileId = c.Int(nullable: false),
                        Post_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Posts", t => t.Post_Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.Post_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProfileId = c.Int(nullable: false),
                        Avatar = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ScreenName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "User_Id", "dbo.Users");
            DropForeignKey("dbo.Comments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Attachments", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.Attachments", "Photo_Id", "dbo.Photos");
            DropIndex("dbo.Comments", new[] { "User_Id" });
            DropIndex("dbo.Comments", new[] { "Post_Id" });
            DropIndex("dbo.Attachments", new[] { "Post_Id" });
            DropIndex("dbo.Attachments", new[] { "Photo_Id" });
            DropTable("dbo.Devices");
            DropTable("dbo.Users");
            DropTable("dbo.Comments");
            DropTable("dbo.Posts");
            DropTable("dbo.Photos");
            DropTable("dbo.Attachments");
        }
    }
}
