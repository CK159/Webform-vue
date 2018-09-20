using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Entity;
using System.Linq;
using DataModel;

namespace WebformVue
{
	public class ProductManager
	{
		public static IQueryable<ProductDto> GetProductDto()
		{
			EfContext context = new EfContext();

			var qry = from p in context.Products
				from cp in context.CatalogProducts.Where(e => e.Product == p).DefaultIfEmpty()
				group cp by p
				into grp
				select new ProductDto
				{
					ProductId = grp.Key.ProductId,
					ProductName = grp.Key.ProductName,
					ProductDesc = grp.Key.ProductDesc,
					ProductRichDesc = grp.Key.ProductRichDesc,
					ProductTypeId = grp.Key.Type.ProductTypeId,
					Active = grp.Key.Active,
					Resources = grp.Key.ProductResources.Select(r => new ProductResourceDto
					{
						ProductResourceId = r.ProductResourceId,
						ProductId = grp.Key.ProductId,
						Active = r.Active,
						DateCreated = r.DateCreated,
						SortOrder = r.SortOrder,
						File = new UploadableFileDto
						{
							FileId = r.Resource.FileId,
							FileName = r.Resource.FileName,
							MimeType = r.Resource.MimeType,
							DateCreated = r.Resource.DateCreated
						}
					}).OrderBy(o => o.SortOrder).ToList(),
					Catalogs = context.Catalogs.Select(c => new ProductCatalogDto
					{
						Assigned = grp.Any(g => g.Catalog == c),
						CatalogId = c.CatalogId,
						CatalogName = c.CatalogName,
						Active = c.Active,
						StoreId = c.Store.StoreId,
						StoreName = c.Store.StoreName
					}).OrderBy(o => o.CatalogName).ToList()
				};

			return qry;
		}

		public class ProductDto
		{
			public int ProductId { get; set; }
			public string ProductName { get; set; }
			public string ProductDesc { get; set; }
			public string ProductRichDesc { get; set; }
			public int? ProductTypeId { get; set; }
			public bool Active { get; set; }
			public List<ProductResourceDto> Resources { get; set; }
			public List<ProductCatalogDto> Catalogs { get; set; }
		}
		
		public class ProductCatalogDto
		{
			public bool Assigned { get; set; }
			public int CatalogId { get; set; }
			public string CatalogName { get; set; }
			public bool Active { get; set; }
			public int StoreId { get; set; }
			public string StoreName { get; set; }
		}

		public static Product ProductDto2Product(ProductDto dto, EfContext context)
		{
			Product entity = context.Products.Find(dto.ProductId);

			if (entity == null)
				context.Products.Add(entity = new Product());

			entity.ProductName = dto.ProductName;
			entity.ProductDesc = dto.ProductDesc;
			entity.ProductRichDesc = dto.ProductRichDesc;
			entity.Type = context.ProductTypes.Find(dto.ProductTypeId);
			entity.Active = dto.Active;

			return entity;
		}

		public static void ProductDto2Resources(ProductDto dto, Product product, EfContext context)
		{
			foreach (ProductResourceDto resDto in dto.Resources)
			{
				//Resource file entity TODO: Move to FileManager?
				File file = context.Files.Find(resDto.File.FileId);
				
				if (file == null)
					context.Files.Add(file = new File());

				file.FileName = resDto.File.FileName;
				file.MimeType = resDto.File.MimeType;
				
				//Content is only provided when new file created or existing file changed
				if (resDto.File.Content != null)
					file.Content = resDto.File.Content;
				
				//Resource Entity
				ProductResource entity = context.ProductResources.Find(resDto.ProductResourceId);
				
				if (entity == null)
					context.ProductResources.Add(entity = new ProductResource());

				entity.Product = product;
				entity.SortOrder = resDto.SortOrder;
				entity.Active = resDto.Active;
				entity.Resource = file;
			}
		}

		public static void ProductDto2Catalogs(ProductDto dto, Product product, EfContext context)
		{
			foreach (ProductCatalogDto catDto in dto.Catalogs)
			{
				Catalog baseCat = context.Catalogs.Find(catDto.CatalogId);
				
				//CatalogProduct doesn't have any navigation properties - find by Catalog and Product
				//TODO: Why doesn't (c => c.Catalog == baseCat && c.Product == product) work?
				CatalogProduct entity = context.CatalogProducts.FirstOrDefault(c =>
					c.Catalog.CatalogId == baseCat.CatalogId && c.Product.ProductId == product.ProductId);

				//Catalog not assigned
				if (!catDto.Assigned)
				{
					//Remove existing assignment
					if (entity != null)
						context.CatalogProducts.Remove(entity);
					
					continue;
				}
				
				//Catalog was assigned
				if (entity == null)
					context.CatalogProducts.Add(entity = new CatalogProduct());

				entity.Product = product;
				entity.Catalog = baseCat;
			}
		}
	}
}