namespace Chicken.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Grouptable_AddIsActiveColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "IsActive");
        }
    }
}
