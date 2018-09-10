using System.Linq;
using System.Web.Http;
using DataModel;
using WebformVue.ApiObjects;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("api/Store")]
	public class StoreController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Preview")]
		public PagedResult<IQueryable<StorePreviewDTO>> StorePreview(
			int? currentPage,
			int? pageSize,
			string storeName,
			int? storeStatusId,
			int? importantConfigId,
			string owner,
			int? catalogId,
			bool? active)
		{
			// 0. Load data / data source
			EfContext context = new EfContext();
			
			// 1. Project
			var qry = (from s in context.Stores
				from storecat in context.Catalogs.Where(e => e.Store == s).DefaultIfEmpty()
				group storecat by s into grp
				select new
				{
					Store = grp.Key,
					Catalogs = grp
				});

			// 2. Filter
			if (!string.IsNullOrEmpty(storeName))
				qry = qry.Where(e => e.Store.StoreName.Contains(storeName));

			if (storeStatusId != null)
				qry = qry.Where(e => e.Store.Status.StoreStatusId == storeStatusId);
			
			if (importantConfigId != null)
				qry = qry.Where(e => e.Store.ImportantConfigId == importantConfigId);
			
			if (!string.IsNullOrEmpty(owner))
				qry = qry.Where(e => e.Store.Owner.Contains(owner));

			if (catalogId != null)
				qry = qry.Where(e => e.Catalogs.Where(w => w != null).Any(f => f.CatalogId == catalogId));

			if (active != null)
				qry = qry.Where(e => e.Store.Active == active);

			// 3. Select to DTO
			var items = qry.Select(q => new StorePreviewDTO
			{
				StoreId = q.Store.StoreId,
				StoreName = q.Store.StoreName,
				StoreStatusName = q.Store.Status.StoreStatusName,
				ImportantConfigId = q.Store.ImportantConfigId,
				Owner = q.Store.Owner,
				CatalogCount = q.Catalogs.Count(w => w != null),
				Active = q.Store.Active
			});

			// 4. Sort
			items = items.OrderBy(o => o.StoreName);

			// 5. Page
			return PagedResult<PreviewDetailDTO>.AutoPage(items, currentPage, pageSize);
		}

		public class StorePreviewDTO
		{
			public int StoreId { get; set; }
			public string StoreName { get; set; }
			public string StoreStatusName { get; set; }
			public int ImportantConfigId { get; set; }
			public string Owner { get; set; }
			public int CatalogCount { get; set; }
			public bool Active { get; set; }
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Load")]
		public Store StoreLoad(int storeId)
		{
			return new EfContext().Stores.FirstOrDefault(e => e.StoreId == storeId);
		}
	}
}