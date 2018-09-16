using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using DataModel;

namespace WebformVue
{
	public class FileController : ApiController
	{
		[HttpGet]
		[Route("api/File/{fileId}")]
		public HttpResponseMessage ProductLoad(int fileId)
		{
			EfContext context = new EfContext();

			var f = context.Files.FirstOrDefault(x => x.FileId == fileId);

			if (f?.Content == null)
				return new HttpResponseMessage(HttpStatusCode.NotFound);

			HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.OK);
			resp.Content = new ByteArrayContent(f.Content);
			resp.Content.Headers.ContentType = new MediaTypeHeaderValue(f.MimeType ?? "application/octet-stream");

			return resp;
		}
	}
}