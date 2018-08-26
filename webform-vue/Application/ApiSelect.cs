using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebformVue.ApiObjects;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("api")]
	public class ApiSelectController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("category/select")]
		public List<CategoryEntity> GetCategorySelect(int? CategoryID, bool? Single)
		{
			return Loader.LoadFromFile<List<CategoryEntity>>("category")
				.Where(e => (!Single.GetValueOrDefault() && e.Active) || e.CategoryId == CategoryID)
				.OrderBy(e => e.CategoryName).ToList();
		}
	}
}