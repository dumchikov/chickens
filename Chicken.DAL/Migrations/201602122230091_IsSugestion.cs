namespace Chicken.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IsSugestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "Coins", c => c.Int(nullable: false));
            AddColumn("dbo.Posts", "IsSuggestion", c => c.Boolean(nullable: false));
            AddColumn("dbo.Posts", "AccountId", c => c.Int(nullable: false));
            CreateIndex("dbo.Posts", "AccountId");
            AddForeignKey("dbo.Posts", "AccountId", "dbo.Accounts", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "AccountId", "dbo.Accounts");
            DropIndex("dbo.Posts", new[] { "AccountId" });
            DropColumn("dbo.Posts", "AccountId");
            DropColumn("dbo.Posts", "IsSuggestion");
            DropColumn("dbo.Accounts", "Coins");
        }
    }
}
