using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace WebformVue
{
	public class GlobalLocalLiveDebugMessagingDetailsExceptionHandlerMKⅫ : ExceptionHandler
	{
		public override void Handle(ExceptionHandlerContext context)
		{
			context.Result = new StandardErrorResultMKⅫ
			{
				Request = context.ExceptionContext.Request,
				Ex = context.Exception,
				IncludeDetails = context.RequestContext.IsLocal
			};
		}

		private class StandardErrorResultMKⅫ : IHttpActionResult
		{
			public HttpRequestMessage Request { get; set; }
			public Exception Ex { get; set; }
			public bool IncludeDetails { get; set; }

			public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
			{
				List<string> messages = new List<string>();
				List<List<string>> details = new List<List<string>>();
				messages.Add(Ex.Message);

				if (IncludeDetails)
				{
					details.Add(Ex.StackTrace.Replace("\r", "").Split('\n').ToList());
					Exception tempEx = Ex.InnerException;

					while (tempEx != null)
					{
						messages.Add(tempEx.Message);
						details.Add(tempEx.StackTrace.Replace("\r", "").Split('\n').ToList());
						tempEx = tempEx.InnerException;
					}
				}

				HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.InternalServerError, new
				{
					Messages = messages,
					Details = details
				});

				return Task.FromResult(response);
			}
		}
	}
}