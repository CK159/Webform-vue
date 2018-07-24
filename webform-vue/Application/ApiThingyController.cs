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
		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Preview")]
		public List<PreviewDetailDTO> PreviewDetailPreview(string name, int? categoryId, bool? active, DateTime? startDate, DateTime? endDate)
		{
			var entity = LoadFromFile<List<PreviewDetailEntity>>("preview-detail").AsEnumerable();

			if (!string.IsNullOrEmpty(name))
				entity = entity.Where(e => e.Name.IndexOf(name, StringComparison.OrdinalIgnoreCase) >= 0);

			if (categoryId != null)
				entity = entity.Where(e => e.CategoryIds.Contains(categoryId.GetValueOrDefault()));
			
			if (active != null)
				entity = entity.Where(e => e.Active == active);
			
			if (startDate != null)
				entity = entity.Where(e => e.Date >= startDate);
			
			if (endDate != null)
				entity = entity.Where(e => e.Date <= endDate);

			return entity.Select(PreviewDetailDTO.FromEntity).ToList();
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Load")]
		public PreviewDetailEntity PreviewDetailLoad(int PreviewDetailId)
		{
			return LoadFromFile<List<PreviewDetailEntity>>("preview-detail")
				.FirstOrDefault(e => e.PreviewDetailId == PreviewDetailId);
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

			SaveToFile(all, "preview-detail");
			return entity;
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("PreviewDetail/Delete")]
		public void PreviewDetailDelete(int PreviewDetailId)
		{
			var all = LoadFromFile<List<PreviewDetailEntity>>("preview-detail")
				.Where(e => e.PreviewDetailId != PreviewDetailId);
			SaveToFile(all, "preview-detail");
		}

		[HttpGet, HttpPost, MultiParameterSupport]
		[Route("Category/GetSelect")]
		public List<CategoryEntity> GetCategorySelect(int? CategoryID, bool? Single)
		{
			return LoadFromFile<List<CategoryEntity>>("category")
				.Where(e => (!Single.GetValueOrDefault() && e.Active) || e.CategoryId == CategoryID)
				.OrderBy(e => e.CategoryName).ToList();
		}

		#region Load Save

		private static string GetPath(string filename)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "\\nosql-db\\" + filename + ".json";
		}

		public static T LoadFromFile<T>(string file) where T : new()
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

		#endregion
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
			List<CategoryEntity> cats = ApiThingyController.LoadFromFile<List<CategoryEntity>>("category");
			PreviewDetailDTO dto = new PreviewDetailDTO
			{
				PreviewDetailId = entity.PreviewDetailId,
				Name = entity.Name,
				Active = entity.Active,
				Date = entity.Date,
				Categories = string.Join(", ", cats.Where(c => entity.CategoryIds.Any(e => e == c.CategoryId))
					.Select(s => s.CategoryName).ToList()),
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
		public int CategoryId { get; set; }
		public string CategoryName { get; set; }
		public bool Active { get; set; }
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