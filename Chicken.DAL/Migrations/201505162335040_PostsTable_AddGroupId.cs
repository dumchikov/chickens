namespace Chicken.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostsTable_AddGroupId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "Group_Id", "dbo.Groups");
            DropIndex("dbo.Posts", new[] { "Group_Id" });
            RenameColumn(table: "dbo.Posts", name: "Group_Id", newName: "GroupId");

            AlterColumn("dbo.Posts", "GroupId", c => c.Int(nullable: true));
            CreateIndex("dbo.Posts", "GroupId");
            AddForeignKey("dbo.Posts", "GroupId", "dbo.Groups", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "GroupId", "dbo.Groups");
            DropIndex("dbo.Posts", new[] { "GroupId" });
            AlterColumn("dbo.Posts", "GroupId", c => c.Int());
            RenameColumn(table: "dbo.Posts", name: "GroupId", newName: "Group_Id");
            CreateIndex("dbo.Posts", "Group_Id");
            AddForeignKey("dbo.Posts", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
