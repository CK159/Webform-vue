using System;
using System.Collections.Generic;

namespace WebformVue.ApiObjects
{
	#region DTO

	//Used for showing in the preview table
	public class PreviewDetailDTO
	{
		public int PreviewDetailId { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public DateTime Date { get; set; }
		public List<string> Categories { get; set; }
		public List<string> Codes { get; set; }
	}

	#endregion

	#region Entity

	public class PreviewDetailEntity
	{
		public int PreviewDetailId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool Active { get; set; }
		public DateTime Date { get; set; }
		public List<int> CategoryIds { get; set; }
		public List<int> CodeIds { get; set; }
	}

	public class CategoryEntity
	{
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public bool Active { get; set; }
	}

	public class CodeEntry
	{
		public int CodeId { get; set; }
		public string CodeValue { get; set; }
	}

	public class CodeAttributeEntry
	{
		public int CodeAttributeId { get; set; }
		public int CodeId { get; set; }
		public int AttributeId { get; set; }
	}

	public class CodeAttributeValueEntry
	{
		public int CodeAttributeValueId { get; set; }
		public int CodeAttributeId { get; set; }
		public int AttributeValueId { get; set; }
	}

	public class AttributeEntry
	{
		public int AttributeId { get; set; }
		public string AttributeName { get; set; }
	}

	public class AttributeValueEntry
	{
		public int AttributeValueId { get; set; }
		public string ValueName { get; set; }
		public int AttributeId { get; set; }
	}

	#endregion
}