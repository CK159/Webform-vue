namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateCreated : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Catalogs", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AddColumn("dbo.CatalogProducts", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Products", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AddColumn("dbo.ProductResources", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AddColumn("dbo.Files", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
            AlterColumn("dbo.Stores", "StoreName", c => c.String());
            AlterColumn("dbo.Stores", "DateCreated", c => c.DateTime(nullable: false, precision: 7, storeType: "datetime2", defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Stores", "DateCreated", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Stores", "StoreName", c => c.Int(nullable: false));
            DropColumn("dbo.Files", "DateCreated");
            DropColumn("dbo.ProductResources", "DateCreated");
            DropColumn("dbo.Products", "DateCreated");
            DropColumn("dbo.CatalogProducts", "DateCreated");
            DropColumn("dbo.Catalogs", "DateCreated");
        }
    }
}
