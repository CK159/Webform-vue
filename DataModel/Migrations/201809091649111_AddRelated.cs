namespace DataModel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs");
            DropIndex("dbo.Products", new[] { "Catalog_CatalogId" });
            CreateTable(
                "dbo.CatalogProducts",
                c => new
                    {
                        CatalogProductId = c.Int(nullable: false, identity: true),
                        SortOrder = c.Int(nullable: false),
                        Catalog_CatalogId = c.Int(),
                        Product_ProductId = c.Int(),
                    })
                .PrimaryKey(t => t.CatalogProductId)
                .ForeignKey("dbo.Catalogs", t => t.Catalog_CatalogId)
                .ForeignKey("dbo.Products", t => t.Product_ProductId)
                .Index(t => t.Catalog_CatalogId)
                .Index(t => t.Product_ProductId);
            
            AddColumn("dbo.Stores", "DateCreated", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductResources", "Product_ProductId", c => c.Int());
            CreateIndex("dbo.ProductResources", "Product_ProductId");
            AddForeignKey("dbo.ProductResources", "Product_ProductId", "dbo.Products", "ProductId");
            DropColumn("dbo.Products", "Catalog_CatalogId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Catalog_CatalogId", c => c.Int());
            DropForeignKey("dbo.CatalogProducts", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductResources", "Product_ProductId", "dbo.Products");
            DropForeignKey("dbo.CatalogProducts", "Catalog_CatalogId", "dbo.Catalogs");
            DropIndex("dbo.ProductResources", new[] { "Product_ProductId" });
            DropIndex("dbo.CatalogProducts", new[] { "Product_ProductId" });
            DropIndex("dbo.CatalogProducts", new[] { "Catalog_CatalogId" });
            DropColumn("dbo.ProductResources", "Product_ProductId");
            DropColumn("dbo.Stores", "DateCreated");
            DropTable("dbo.CatalogProducts");
            CreateIndex("dbo.Products", "Catalog_CatalogId");
            AddForeignKey("dbo.Products", "Catalog_CatalogId", "dbo.Catalogs", "CatalogId");
        }
    }
}
