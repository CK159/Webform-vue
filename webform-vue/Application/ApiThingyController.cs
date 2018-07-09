using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebformVue
{
	[RoutePrefix("ApiThingyController.cs")]
	public class ApiThingyController : ApiController
	{
		[HttpGet]
		[HttpPost]
		[Route("PreviewDetail/Preview")]
		public List<PreviewDetailDTO> PreviewDetailPreview()
		{
			List<PreviewDetailDTO> entity = LoadFromFile<List<PreviewDetailEntity>>("preview-detail")
				.Select(PreviewDetailDTO.FromEntity).ToList();
			
			return entity;
		}

		[HttpGet]
		[HttpPost]
		[Route("PreviewDetail/Load")]
		public PreviewDetailEntity PreviewDetailDetail([FromBody]int PreviewDetailId)
		{
			var x = LoadFromFile<List<PreviewDetailEntity>>("preview-detail");
			var y = x.FirstOrDefault(e => e.PreviewDetailId == PreviewDetailId);
			return y;
			
			return LoadFromFile<List<PreviewDetailEntity>>("preview-detail").FirstOrDefault(e => e.PreviewDetailId == PreviewDetailId);
		}
		
		private string GetPath(string file)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "\\nosql-db\\" + file + ".json";
		}
		
		private T LoadFromFile<T>(string file) where T : new()
		{
			string path = GetPath(file);
			
			if (File.Exists(path))
			{
				return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
			}

			return new T();
		}
	}

	//Used for showing in the preview table
	public class PreviewDetailDTO
	{
		public int PreviewDetailId { get; set; }
		public string Name { get; set; }
		public bool Active { get; set; }
		public DateTime Date { get; set; }
		public string Categories { get; set; }
		public string Codes { get; set; }

		public static PreviewDetailDTO FromEntity(PreviewDetailEntity entity)
		{
			PreviewDetailDTO dto = new PreviewDetailDTO
			{
				PreviewDetailId = entity.PreviewDetailId,
				Name = entity.Name,
				Active = entity.Active,
				Date = entity.Date,
				Categories = "TODO",
				Codes = "TODO"
			};

			return dto;
		}
	}

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
		int CategoryId { get; set; }
		string CategoryName { get; set; }
	}

	public class CodeEntry
	{
		public int CodeId { get; set; }
		public string CodeValue { get; set; }
		public List<int> CodeAttributeIds { get; set; }
	}

	public class AttributeEntry
	{
		public int AttributeID { get; set; }
		public string AttributeName { get; set; }
		public List<int> AttributeValueIds { get; set; }
	}

	public class AttributeValueEntry
	{
		public int AttributeValueId { get; set; }
		public string ValueName { get; set; }
	}
}