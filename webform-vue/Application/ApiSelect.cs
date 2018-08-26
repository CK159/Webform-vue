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
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("code/select")]
		public SimpleSelectResult GetCodeSelect(int? CodeId, bool? Single)
		{
			return new SimpleSelectResult(Loader.LoadFromFile<List<CodeEntry>>("code")
				.Where(e => !Single.GetValueOrDefault() || e.CodeId == CodeId)
				.OrderBy(e => e.CodeValue)
				.ToDictionary(a => (int?)a.CodeId, a => a.CodeValue));
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("codeAttribute/select")]
		public SimpleSelectResult GetCodeAttributeSelect(int? CodeId, int? CodeAttributeId, bool? Single)
		{
			var x = (from c in Loader.LoadFromFile<List<CodeAttributeEntry>>("code-attribute")
				join a in Loader.LoadFromFile<List<AttributeEntry>>("attribute") on c.AttributeId equals a.AttributeId
				select new
				{
					c.CodeId,
					c.CodeAttributeId,
					a.AttributeName
				});
			
			return new SimpleSelectResult(x
				.Where(e => 
					(CodeAttributeId != null && e.CodeAttributeId == CodeAttributeId)
					|| (!Single.GetValueOrDefault() && (CodeId == null || e.CodeId == CodeId))
				)
				.OrderBy(e => e.CodeAttributeId)
				.ToDictionary(a => (int?)a.CodeAttributeId, a => a.AttributeName));
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("codeAttributeValue/select")]
		public SimpleSelectResult GetCodeAttributeValueSelect(int? CodeAttributeId, int? CodeAttributeValueId, bool? Single)
		{
			var x = (from cav in Loader.LoadFromFile<List<CodeAttributeValueEntry>>("code-attribute-value")
				join av in Loader.LoadFromFile<List<AttributeValueEntry>>("attribute-value") on cav.AttributeValueId equals av.AttributeValueId
				select new
				{
					cav.CodeAttributeId,
					cav.CodeAttributeValueId,
					av.ValueName
				});
			
			return new SimpleSelectResult(x
				.Where(e => 
					(CodeAttributeValueId != null && e.CodeAttributeValueId == CodeAttributeValueId)
					|| (!Single.GetValueOrDefault() && (CodeAttributeId == null || e.CodeAttributeId == CodeAttributeId))
				)
				.OrderBy(e => e.CodeAttributeId)
				.ToDictionary(a => (int?)a.CodeAttributeValueId, a => a.ValueName));
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("attribute/select")]
		public SimpleSelectResult GetAttributeSelect(int? AttributeId, bool? Single)
		{
			return new SimpleSelectResult(Loader.LoadFromFile<List<AttributeEntry>>("attribute")
				.Where(e => !Single.GetValueOrDefault() || e.AttributeId == AttributeId)
				.OrderBy(e => e.AttributeName)
				.ToDictionary(a => (int?)a.AttributeId, a => a.AttributeName));
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("attributeValue/select")]
		public SimpleSelectResult GetAttributeValueSelect(int? AttributeId, int? AttributeValueId, bool? Single)
		{
			return new SimpleSelectResult(Loader.LoadFromFile<List<AttributeValueEntry>>("attribute-value")
				.Where(e => 
					(AttributeValueId != null && e.AttributeValueId == AttributeValueId)
					|| (!Single.GetValueOrDefault() && (AttributeId == null || e.AttributeId == AttributeId))
				)
				.OrderBy(e => e.ValueName)
				.ToDictionary(a => (int?)a.AttributeValueId, a => a.ValueName));
		}
	}
}