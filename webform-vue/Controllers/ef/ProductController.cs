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
		public ProductDto ProductLoad(int productId)
		{
			EfContext context = new EfContext();

			var qry = from p in context.Products
				join cp in context.CatalogProducts on p equals cp.Product
				group cp by p into grp
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
					}).OrderBy(o => o.ProductResourceId).ToList(),
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

			return qry.FirstOrDefault(e => e.ProductId == productId);
		}

		public class ProductDto
		{
			public int ProductId { get; set; }
			public string ProductName { get; set; }
			public string ProductDesc { get; set; }
			public string ProductRichDesc { get; set; }
			public int ProductTypeId { get; set; }
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
	}
}