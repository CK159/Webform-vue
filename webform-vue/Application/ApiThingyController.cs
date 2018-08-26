using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebformVue.ApiObjects;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("ApiThingyController.cs")]
	public class ApiThingyController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Preview")]
		public PagedResult<IQueryable<PreviewDetailDTO>> PreviewDetailPreview(
			int? currentPage,
			int? pageSize,
			string name,
			int? categoryId,
			bool? active,
			DateTime? startDate,
			DateTime? endDate)
		{
			// 0. Load data / data source
			List<PreviewDetailEntity> previewDetail = Loader.LoadFromFile<List<PreviewDetailEntity>>("preview-detail");
			List<CategoryEntity> categories = Loader.LoadFromFile<List<CategoryEntity>>("category");

			// 1. Project
			var qry = (from pd in previewDetail
				from cat in categories.Where(e => pd.CategoryIds.Contains(e.CategoryId)).DefaultIfEmpty()
				group cat by pd into grp
				select new
				{
					previewDetail = grp.Key,
					categories = grp
				}).AsQueryable(); //Not needed if using entity framework - want to ensure pattern works with IQueryable

			// 2. Filter
			if (!string.IsNullOrEmpty(name))
				qry = qry.Where(e => e.previewDetail.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);

			if (categoryId != null)
				qry = qry.Where(e => e.categories.Where(w => w != null).Any(f => f.CategoryId == categoryId));

			if (active != null)
				qry = qry.Where(e => e.previewDetail.Active == active);

			if (startDate != null)
				qry = qry.Where(e => e.previewDetail.Date >= startDate);

			if (endDate != null)
				qry = qry.Where(e => e.previewDetail.Date <= endDate);

			// 3. Select to DTO
			var items = qry.Select(q => new PreviewDetailDTO
			{
				PreviewDetailId = q.previewDetail.PreviewDetailId,
				Name = q.previewDetail.Name,
				Active = q.previewDetail.Active,
				Date = q.previewDetail.Date,
				Categories = q.categories
					.Where(w => w != null) //TODO: Figure out why grouping results in 1 null category instead of 0 categories
					.Select(c => c.CategoryName)
					.ToList(),
				Codes = new List<string> {"TODO 1", "todo 2"}
			});

			// 4. Sort
			items = items.OrderBy(o => o.Name);

			// 5. Page
			return PagedResult<PreviewDetailDTO>.AutoPage(
				items,
				currentPage ?? 0,
				pageSize);
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Load")]
		public PreviewDetailEntity PreviewDetailLoad(int PreviewDetailId)
		{
			return Loader.LoadFromFile<List<PreviewDetailEntity>>("preview-detail")
				.FirstOrDefault(e => e.PreviewDetailId == PreviewDetailId);
		}

		//TODO: Find how to make this a generic method that supports validation and error messages
		[HttpGet, HttpPost]
		[Route("PreviewDetail/Save")]
		public PreviewDetailEntity PreviewDetailSave(PreviewDetailEntity entity)
		{
			var all = Loader.LoadFromFile<List<PreviewDetailEntity>>("preview-detail");

			if (entity.PreviewDetailId <= 0)
			{
				//New record
				entity.PreviewDetailId =
					(all.OrderByDescending(i => i.PreviewDetailId).FirstOrDefault()?.PreviewDetailId ?? 0) + 1;
				entity.Date = DateTime.Now;
				all.Add(entity);
			}
			else
			{
				int index = all.FindIndex(r => r.PreviewDetailId == entity.PreviewDetailId);
				if (index >= 0)
				{
					all[index] = entity;
				}
			}

			Loader.SaveToFile(all, "preview-detail");
			return entity;
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Delete")]
		public void PreviewDetailDelete(int PreviewDetailId)
		{
			var all = Loader.LoadFromFile<List<PreviewDetailEntity>>("preview-detail")
				.Where(e => e.PreviewDetailId != PreviewDetailId);
			Loader.SaveToFile(all, "preview-detail");
		}
	}
}