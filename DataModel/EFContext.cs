using System.Data.Entity;

namespace DataModel
{
    public class EfContext : DbContext
    {
        public DbSet<StoreStatus> StoreStatus { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductResource> ProductResources { get; set; }
        public DbSet<Catalog> Catalogs { get; set; }
        public DbSet<File> Files { get; set; }
    }
}