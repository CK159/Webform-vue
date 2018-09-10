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
		public PagedResult<IQueryable<ProductPreviewDTO>> ProductPreview(
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
				group catprod.Catalog by p into grp
				select new
				{
					Product = grp.Key,
					Catalogs = grp
				});

			// 2. Filter
			if (!string.IsNullOrEmpty(productName))
				qry = qry.Where(e => e.Product.ProductName.Contains(productName));
			
			if (!string.IsNullOrEmpty(productDesc))
				qry = qry.Where(e => e.Product.ProductDesc.Contains(productDesc) || e.Product.ProductRichDesc.Contains(productDesc));

			if (catalogId != null)
				qry = qry.Where(e => e.Catalogs.Where(w => w != null).Any(f => f.CatalogId == catalogId));

			if (active != null)
				qry = qry.Where(e => e.Product.Active == active);

			// 3. Select to DTO
			var items = qry.Select(q => new ProductPreviewDTO
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

		public class ProductPreviewDTO
		{
			public int ProductId { get; set; }
			public string ProductName { get; set; }
			public bool Active { get; set; }
			public List<string> Catalogs { get; set; }
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Load")]
		public Product ProductLoad(int productId)
		{
			return new EfContext().Products.FirstOrDefault(e => e.ProductId == productId);
		}
	}
}