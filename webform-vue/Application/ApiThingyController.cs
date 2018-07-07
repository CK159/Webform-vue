using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebformVue
{
	[RoutePrefix("ApiThingyController.cs")]
	public class ApiThingyController : ApiController
	{
		private string PdPath => GetPath("preview-detail");

		[HttpGet]
		[HttpPost]
		[Route("PreviewDetail/Preview")]
		public List<PreviewDetailDTO> PreviewDetailPreview()
		{
			return LoadFromFile<List<PreviewDetailDTO>>(PdPath);
		}
		
		/*Product[] products = { 
			new Product { Id = 1, Name = "Tomato Soup", Category = "Groceries", Price = 1 }, 
			new Product { Id = 2, Name = "Yo-yo", Category = "Toys", Price = 3.75M }, 
			new Product { Id = 3, Name = "Hammer", Category = "Hardware", Price = 16.99M } 
		};

		public IEnumerable<Product> GetAllProducts()
		{
			return products;
		}
		
		public Product GetProductById(int id)
		{
			var product = products.FirstOrDefault(p => p.Id == id);
			if (product == null)
			{
				throw new HttpResponseException(HttpStatusCode.NotFound);
			}
			return product;
		}
		
		public IEnumerable<Product> GetProductsByCategory(string category)
		{
			return products.Where(
			p => string.Equals(p.Category, category,
			StringComparison.OrdinalIgnoreCase));
		}*/
		
		private string GetPath(string file)
		{
			return AppDomain.CurrentDomain.BaseDirectory + "\\nosql-db\\" + file + ".json";
		}
		
		private T LoadFromFile<T>(string path) where T : new()
		{
			if (File.Exists(path))
			{
				return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
			}

			return new T();
		}
	}

	/*public class Product
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }
		public string Category { get; set; }
	}*/

	public class PreviewDetailDTO
	{
		public string Name { get; set; }
	}
}