using System;

namespace WebformVue
{
	public class FileManager
	{
		
	}

	public class UploadableFileDto
	{
		public int FileId { get; set; }
		public string FileName { get; set; }
		public string MimeType { get; set; }
		public byte[] Content { get; set; }
		public DateTime DateCreated { get; set; }
	}

	public class ProductResourceDto
	{
		public int ProductResourceId { get; set; }
		public int ProductId { get; set; }
		public UploadableFileDto File { get; set; }
		public int SortOrder { get; set; }
		public bool Active { get; set; }
		/*public string ResourceName { get; set; }
		public string ResourceInfo { get; set; }
		public string ResourceTypeName { get; set; }
		public string ResourceTypeCode { get; set; }*/
		public DateTime DateCreated { get; set; }
	}
}