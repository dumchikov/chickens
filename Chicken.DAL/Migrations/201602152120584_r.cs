namespace Chicken.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class r : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Posts", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Posts", new[] { "AccountId" });
            AlterColumn("dbo.Posts", "AccountId", c => c.Int());
            CreateIndex("dbo.Posts", "AccountId");
            AddForeignKey("dbo.Posts", "AccountId", "dbo.Accounts", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Posts", new[] { "AccountId" });
            AlterColumn("dbo.Posts", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "AccountId");
            AddForeignKey("dbo.Posts", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
    }
}
