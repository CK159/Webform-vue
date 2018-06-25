using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using Newtonsoft.Json;

namespace WebformVue
{
	public class ApiThingy : WebService
	{
		[WebMethod]
		[ScriptMethod(UseHttpGet=true ,ResponseFormat = ResponseFormat.Json)]
		public void GetRecordPreview()
		{
			Context.Response.Headers.Add("Content-Type", "application/json");
			Context.Response.Write(JsonConvert.SerializeObject(LoadFromFile()));
		}
		
		[WebMethod]
		[ScriptMethod(UseHttpGet=true ,ResponseFormat = ResponseFormat.Json)]
		public void GetRecordDetail(int id)
		{
			List<RecordDetail> records = LoadFromFile();
			RecordDetail record = records.SingleOrDefault(r => r.id == id);

			Context.Response.Headers.Add("Content-Type", "application/json");
			Context.Response.Write(JsonConvert.SerializeObject(record));
		}
		
		[WebMethod]
		[ScriptMethod(UseHttpGet=true ,ResponseFormat = ResponseFormat.Json)]
		public void SaveRecordDetail(string json)
		{
			RecordDetail record = JsonConvert.DeserializeObject<RecordDetail>(json);
			List<RecordDetail> records = LoadFromFile();

			if (record.id <= 0)
			{
				//New record
				//Not sure if I need all this....
				int maxId = records.OrderByDescending(i => i.id).FirstOrDefault()?.id ?? 0;
				record.id = maxId + 1;
				
				records.Add(record);
			}
			else
			{
				//Existing record. Find old entry and replace it with the new one
				int index = records.FindIndex(r => r.id == record.id);

				if (index >= 0)
				{
					records[index] = record;
				}
			}

			SaveToFile(records);
			Context.Response.Headers.Add("Content-Type", "application/json");
			Context.Response.Write("{\"id\": " + record.id + "}");
		}
		
		[WebMethod]
		[ScriptMethod(UseHttpGet=true ,ResponseFormat = ResponseFormat.Json)]
		public void DeleteRecordDetail(int id)
		{
			List<RecordDetail> records = LoadFromFile();
			
			records.RemoveAll(r => r.id == id);
			
			SaveToFile(records);
			Context.Response.Headers.Add("Content-Type", "application/json");
		}

		private List<RecordDetail> LoadFromFile()
		{
			string path = GetPath();

			if (File.Exists(path))
			{
				return JsonConvert.DeserializeObject<List<RecordDetail>>(File.ReadAllText(path));
			}

			return new List<RecordDetail>();
		}

		private void SaveToFile(List<RecordDetail> records)
		{
			string path = GetPath();

			File.WriteAllText(path, JsonConvert.SerializeObject(records));
		}

		private string GetPath()
		{
			return AppDomain.CurrentDomain.BaseDirectory + "\\records.json";
		}
	}

	public class RecordDetail
	{
		public int id { get; set; }
		public string name { get; set; }
		public string description { get; set; }
		public bool active { get; set; }
		public string date { get; set; }
	}
}