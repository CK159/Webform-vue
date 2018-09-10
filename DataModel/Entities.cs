using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace DataModel
{

    #region Lookups

    public class StoreStatus
    {
        public int StoreStatusId { get; set; }
        public string StoreStatusName { get; set; }
        public string StoreStatusCode { get; set; }
    }

    public class ProductType
    {
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductTypeCode { get; set; }
    }

    public class ResourceType
    {
        public int ResourceTypeId { get; set; }
        public string ResourceTypeName { get; set; }
        public string ResourceTypeCode { get; set; }
    }

    #endregion

    public class Store
    {
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public virtual StoreStatus Status { get; set; }
        public int ImportantConfigId { get; set; }
        public string Description { get; set; }
        public string Owner { get; set; }
        public bool Active { get; set; }
	    [Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }
        public string ProductRichDesc { get; set; }
        public virtual ProductType Type { get; set; }
        public bool Active { get; set; }
	    [Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime DateCreated { get; set; }
        public virtual ICollection<ProductResource> ProductResources { get; set; }
    }

    public class ProductResource
    {
        public int ProductResourceId { get; set; }
        public virtual Product Product { get; set; }
        public string ResourceName { get; set; }
        public string ResourceInfo { get; set; }
        public int SortOrder { get; set; }
        public virtual ResourceType Type { get; set; }
        public virtual File Resource { get; set; }
        public bool Active { get; set; }
	    [Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime DateCreated { get; set; }
    }

    public class Catalog
    {
        public int CatalogId { get; set; }
        public virtual Store Store { get; set; }
        public string CatalogName { get; set; }
        public string CatalogDesc { get; set; }
        public string InternalName { get; set; }
        public bool Active { get; set; }
	    public virtual ICollection<CatalogProduct> CatalogProducts { get; set; }
	    [Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime DateCreated { get; set; }
    }

    public class CatalogProduct
    {
        public int CatalogProductId { get; set; }
        public virtual Catalog Catalog { get; set; }
        public virtual Product Product { get; set; }
        public int SortOrder { get; set; }
	    [Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime DateCreated { get; set; }
    }

    public class File
    {
        public int FileId { get; set; }
        public int FileName { get; set; }
        public string MimeType { get; set; }
        public string FilePath { get; set; }
        public byte[] Content { get; set; }
	    [Required, Column(TypeName = "datetime2"), DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime DateCreated { get; set; }
    }
}