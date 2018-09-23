using System;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace WebformVue
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			// Web API configuration and services

			// Web API routes
			config.MapHttpAttributeRoutes();

			// Return JSON when viewing API with a web browser
			// https://stackoverflow.com/a/20556625
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings
				.Add(new System.Net.Http.Formatting.RequestHeaderMapping(
					"Accept",
					"text/html",
					StringComparison.InvariantCultureIgnoreCase,
					true,
					"application/json"
				));

			// I reject your convention-based routing and substitute my own.
			/*config.Routes.MapHttpRoute(
			    "DefaultApi",
			    "api/{controller}/{id}",
			    new {id = RouteParameter.Optional}
			);*/

			GlobalConfiguration.Configuration
				.ParameterBindingRules
				.Insert(0, SimplePostVariableParameterBinding.HookupParameterBinding);

			//Set up custom error handler 
			config.Services.Replace(typeof(IExceptionHandler),
				new GlobalLocalLiveDebugMessagingDetailsExceptionHandlerMKⅫ());
		}
	}
}