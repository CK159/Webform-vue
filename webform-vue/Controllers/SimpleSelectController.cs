using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebformVue.ApiObjects;
using WebformVue.Util;

namespace WebformVue
{
	[RoutePrefix("api/SimpleSelect")]
	public class SimpleSelectController : ApiController
	{
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("GetCodeAttributes")]
		public List<CodeAttrDto> GetCodeAttributes(int? CodeId)
		{
			return (from c in Loader.LoadFromFile<List<CodeAttributeEntry>>("code-attribute")
				join a in Loader.LoadFromFile<List<AttributeEntry>>("attribute") on c.AttributeId equals a.AttributeId
				where CodeId == null || c.CodeId == CodeId
				orderby a.AttributeName
				select new CodeAttrDto
				{
					CodeAttributeId = c.CodeAttributeId,
					AttributeId = a.AttributeId,
					AttributeName = a.AttributeName
				}).ToList();
		}

		public class CodeAttrDto
		{
			public int CodeAttributeId { get; set; }
			public int AttributeId { get; set; }
			public string AttributeName { get; set; }
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("GetCodeAttributeValues")]
		public List<CodeAttrValDto> GetCodeAttributeValues(int? CodeAttributeId)
		{
			return (from cav in Loader.LoadFromFile<List<CodeAttributeValueEntry>>("code-attribute-value")
				join av in Loader.LoadFromFile<List<AttributeValueEntry>>("attribute-value") on cav.AttributeValueId equals av.AttributeValueId
				where CodeAttributeId == null || cav.CodeAttributeId == CodeAttributeId
				orderby av.ValueName
				select new CodeAttrValDto
				{
					CodeAttributeValueId = cav.CodeAttributeValueId,
					AttributeId = av.AttributeId,
					AttributeValueId = av.AttributeValueId,
					ValueName = av.ValueName
				}).ToList();
		}

		public class CodeAttrValDto
		{
			public int CodeAttributeValueId { get; set; }
			public int AttributeId { get; set; }
			public int AttributeValueId { get; set; }
			public string ValueName { get; set; }
		}
	}
}