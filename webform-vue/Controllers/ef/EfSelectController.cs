using System.Linq;
using System.Web.Http;
using DataModel;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("api/select")]
	public class SimpleEfSelectController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("catalogSelect")]
		public SimpleSelect GetCatalogSelect(int? CatalogId, bool? Single)
		{
			return new EfContext().Catalogs
				.Where(e => (!(Single ?? false) && e.Active) || e.CatalogId == CatalogId)
				.OrderBy(e => e.CatalogName)
				.ToSimpleSelect(a => a.CatalogId, a => a.CatalogName);
		}
	}
}