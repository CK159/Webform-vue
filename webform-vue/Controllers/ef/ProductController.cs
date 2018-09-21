using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using DataModel;
using WebformVue.ApiObjects;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("api/Product")]
	public class ProductController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Preview")]
		public PagedResult<IQueryable<ProductPreviewDto>> ProductPreview(
			int? currentPage,
			int? pageSize,
			string productName,
			string productDesc,
			int? catalogId,
			bool? active)
		{
			// 0. Load data / data source
			EfContext context = new EfContext();

			// 1. Project
			var qry = (from p in context.Products
				from catprod in context.CatalogProducts.Where(e => e.Product == p).DefaultIfEmpty()
				group catprod.Catalog by p
				into grp
				select new
				{
					Product = grp.Key,
					Catalogs = grp
				});

			// 2. Filter
			if (!string.IsNullOrEmpty(productName))
				qry = qry.Where(e => e.Product.ProductName.Contains(productName));

			if (!string.IsNullOrEmpty(productDesc))
				qry = qry.Where(e =>
					e.Product.ProductDesc.Contains(productDesc) || e.Product.ProductRichDesc.Contains(productDesc));

			if (catalogId != null)
				qry = qry.Where(e => e.Catalogs.Where(w => w != null).Any(f => f.CatalogId == catalogId));

			if (active != null)
				qry = qry.Where(e => e.Product.Active == active);

			// 3. Select to DTO
			var items = qry.Select(q => new ProductPreviewDto
			{
				ProductId = q.Product.ProductId,
				ProductName = q.Product.ProductName,
				Active = q.Product.Active,
				Catalogs = q.Catalogs
					.Where(w => w != null)
					.Select(c => c.CatalogName)
					.ToList()
			});

			// 4. Sort
			items = items.OrderBy(o => o.ProductName);

			// 5. Page
			return PagedResult<PreviewDetailDTO>.AutoPage(items, currentPage, pageSize);
		}

		public class ProductPreviewDto
		{
			public int ProductId { get; set; }
			public string ProductName { get; set; }
			public bool Active { get; set; }
			public List<string> Catalogs { get; set; }
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Load")]
		public ProductManager.ProductDto ProductLoad(int productId)
		{
			return ProductManager.GetProductDto().FirstOrDefault(e => e.ProductId == productId);
		}

		[HttpGet, HttpPost]
		[Route("Save")]
		public ProductManager.ProductDto ProductSave(ProductManager.ProductDto dto)
		{
			EfContext context = new EfContext();
			Product product = ProductManager.ProductDto2Product(dto, context);
			ProductManager.ProductDto2Resources(dto, product, context);
			ProductManager.ProductDto2Catalogs(dto, product, context);
			context.SaveChanges();

			return ProductManager.GetProductDto().FirstOrDefault(e => e.ProductId == product.ProductId);
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Delete")]
		public void ProductSave(int productId)
		{
			EfContext context = new EfContext();
			Product p = context.Products.Find(productId);

			context.ProductResources.RemoveRange(context.ProductResources.Where(e => e.Product.ProductId == p.ProductId));
			context.CatalogProducts.RemoveRange(context.CatalogProducts.Where(e => e.Product.ProductId == p.ProductId));
			context.Products.Remove(p);
			context.SaveChanges();
		}

		//TODO: way of annotating default value on DTOs themselves and having generic method to return default
		[HttpGet, HttpPost]
		[Route("New")]
		public Dictionary<string, object> ProductNew()
		{
			Dictionary<string, object> dict = new Dictionary<string, object>();
			var context = new EfContext();
			
			dict.Add("Product", new ProductManager.ProductDto
			{
				Active = true,
				ProductTypeId = context.ProductTypes.FirstOrDefault(x => x.ProductTypeCode == "PHYS")?.ProductTypeId ?? 0,
				Resources = new List<ProductResourceDto>(),
				Catalogs = new List<ProductManager.ProductCatalogDto>()
			});
			dict.Add("Resource", new ProductResourceDto
			{
				File = new UploadableFileDto()
			});
			dict.Add("Catalog", new ProductManager.ProductCatalogDto());
			
			return dict;
		}
	}
}