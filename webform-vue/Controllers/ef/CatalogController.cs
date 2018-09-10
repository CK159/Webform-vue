using System.Linq;
using System.Web.Http;
using DataModel;
using WebformVue.ApiObjects;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("api/Catalog")]
	public class CategoryController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Preview")]
		public PagedResult<IQueryable<CatalogPreviewDTO>> CatalogPreview(
			int? currentPage,
			int? pageSize,
			string catalogName,
			string catalogDesc,
			int? productId,
			int? storeId,
			bool? active)
		{
			// 0. Load data / data source
			EfContext context = new EfContext();
			
			// 1. Project
			var qry = (from c in context.Catalogs select c);

			// 2. Filter
			if (!string.IsNullOrEmpty(catalogName))
				qry = qry.Where(e => e.CatalogName.Contains(catalogName) || e.InternalName.Contains(catalogName));
			
			if (!string.IsNullOrEmpty(catalogDesc))
				qry = qry.Where(e => e.CatalogDesc.Contains(catalogDesc));

			if (productId != null)
				qry = qry.Where(e => e.CatalogProducts.Any(f => f.Product.ProductId == productId));
			
			if (storeId != null)
				qry = qry.Where(e => e.Store.StoreId == storeId);

			if (active != null)
				qry = qry.Where(e => e.Active == active);

			// 3. Select to DTO
			var items = qry.Select(q => new CatalogPreviewDTO
			{
				CatalogId = q.CatalogId,
				CatalogName = q.CatalogName,
				InternalName = q.InternalName,
				StoreId = q.Store.StoreId,
				StoreName = q.Store.StoreName,
				Active = q.Active,
				ProductCount = q.CatalogProducts.Select(e => e.Product).Distinct().Count()
			});

			// 4. Sort
			items = items.OrderBy(o => o.CatalogName);

			// 5. Page
			return PagedResult<PreviewDetailDTO>.AutoPage(items, currentPage, pageSize);
		}

		public class CatalogPreviewDTO
		{
			public int CatalogId { get; set; }
			public string CatalogName { get; set; }
			public string InternalName { get; set; }
			public int StoreId { get; set; }
			public string StoreName { get; set; }
			public int ProductCount { get; set; }
			public bool Active { get; set; }
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Load")]
		public Catalog CatalogLoad(int catalogId)
		{
			return new EfContext().Catalogs.FirstOrDefault(e => e.CatalogId == catalogId);
		}
	}
}