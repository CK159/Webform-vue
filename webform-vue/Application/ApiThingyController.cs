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
		[HttpGet, HttpPost]
		[Route("PreviewDetail/Preview")]
		public List<PreviewDetailDTO> PreviewDetailPreview()
		{
			List<PreviewDetailDTO> entity = LoadFromFile<List<PreviewDetailEntity>>("preview-detail")
				.Select(PreviewDetailDTO.FromEntity).ToList();
			
			return entity;
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Load")]
		public PreviewDetailEntity PreviewDetailLoad(int PreviewDetailId)
		{
			return LoadFromFile<List<PreviewDetailEntity>>("preview-detail").FirstOrDefault(e => e.PreviewDetailId == PreviewDetailId);
		}
		
		//TODO: Find how to make this a generic method that supports validation and error messages
		[HttpGet, HttpPost]
		[Route("PreviewDetail/Save")]
		public PreviewDetailEntity PreviewDetailSave(PreviewDetailEntity entity)
		{
			var all = LoadFromFile<List<PreviewDetailEntity>>("preview-detail");

			if (entity.PreviewDetailId <= 0)
			{
				//New record
				entity.PreviewDetailId = (all.OrderByDescending(i => i.PreviewDetailId).FirstOrDefault()?.PreviewDetailId ?? 0) + 1;
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
			
			SaveToFile(all, "preview-detail");
			return entity;
		}
		
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Delete")]
		public void PreviewDetailDelete(int PreviewDetailId)
		{
			var all = LoadFromFile<List<PreviewDetailEntity>>("preview-detail").Where(e => e.PreviewDetailId != PreviewDetailId);
			SaveToFile(all, "preview-detail");
		}
		
		private string GetPath(string filename)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "\\nosql-db\\" + filename + ".json";
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
		
		private void SaveToFile(object items, string filename)
		{
			string path = GetPath(filename);
			File.WriteAllText(path, JsonConvert.SerializeObject(items));
		}
	}

	#region DTO
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
	#endregion
}