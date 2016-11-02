using System.Data.Entity.Migrations;

namespace Chicken.DAL.Migrations
{
    public partial class AddGroupsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AccessToken = c.String(),
                        GroupDomainName = c.String(),
                        OwnerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Posts", "Group_Id", c => c.Int());
            CreateIndex("dbo.Posts", "Group_Id");
            AddForeignKey("dbo.Posts", "Group_Id", "dbo.Groups", "Id");

            //Sql("INSERT INTO Groups (Name, AccessToken, GroupDomainName, OwnerId) VALUES ('Харьков', '4678659ecbffd3567799b72c33b3fc540da9dc49c02bf65ff0b3cf29a1894055d2b1013aafa39878b130b', 'koko_kharkov', 65470032)");
            //Sql("UPDATE Posts SET City_Id = 1");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Posts", new[] { "Group_Id" });
            DropColumn("dbo.Posts", "Group_Id");
            DropTable("dbo.Groups");
        }
    }
}
