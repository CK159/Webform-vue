namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MimeType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Files", "MimeType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Files", "MimeType");
        }
    }
}
