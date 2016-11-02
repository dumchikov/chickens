using System.Data.Entity.Migrations;

namespace Chicken.DAL.Migrations
{
    public partial class Posts_AddIsNewColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "IsNew", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Posts", "IsNew");
        }
    }
}
