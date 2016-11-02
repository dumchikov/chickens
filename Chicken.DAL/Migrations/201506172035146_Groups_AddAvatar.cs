namespace Chicken.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Groups_AddAvatar : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "Avatar", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Groups", "Avatar");
        }
    }
}
