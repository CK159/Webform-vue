namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixFileName : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Files", "FileName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Files", "FileName", c => c.Int(nullable: false));
        }
    }
}
