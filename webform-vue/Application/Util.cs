using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace WebformVue.Util
{
	public class Loader
	{
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

		public static void SaveToFile(object items, string filename)
		{
			//https://stackoverflow.com/a/39037146
			using (var fs = File.Create(GetPath(filename)))
			using (var sw = new StreamWriter(fs))
			using (var jtw = new JsonTextWriter(sw)
			{
				Formatting = Formatting.Indented,
				Indentation = 1,
				IndentChar = '\t'
			})
			{
				new JsonSerializer().Serialize(jtw, items);
			}
		}
	}

	public class PagedResult<T>
	{
		public T result { get; set; }
		public int currentPage { get; set; }
		public int pages { get; set; }
		public int recordCount { get; set; }

		public PagedResult(T result, int currentPage, int pages, int recordCount)
		{
			this.result = result;
			this.currentPage = currentPage;
			this.pages = pages;
			this.recordCount = recordCount;
		}

		public static PagedResult<IQueryable<U>> AutoPage<U>(IQueryable<U> q, int currentPage, int? pageSize)
		{
			//TODO: Generic way to not evaluate query twice?
			//Like https://stackoverflow.com/questions/7767409/better-way-to-query-a-page-of-data-and-get-total-count-in-entity-framework-4-1
			int recordCount = q.Count();
			int pages = 1;
			
			if (pageSize != null)
			{
				int ps = pageSize.GetValueOrDefault();
				pages = (int) Math.Ceiling((double) recordCount / ps);
				//Paged result set
				q = q.Skip(currentPage * ps).Take(ps);
			}

			return new PagedResult<IQueryable<U>>(q, currentPage, pages, recordCount);
		}
	}
}