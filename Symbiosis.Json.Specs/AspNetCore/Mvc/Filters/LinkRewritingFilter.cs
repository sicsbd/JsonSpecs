using Microsoft.AspNetCore.Mvc.Routing;
using Symbiosis.Json.Specs.Infrastructure.Ion;
using Symbiosis.Json.Specs.Ion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Filters
{
	public class LinkRewritingFilter
		: IAsyncResultFilter
	{
		private readonly IUrlHelperFactory _urlHelperFactory;

		public LinkRewritingFilter(IUrlHelperFactory urlHelperFactory)
		{
			_urlHelperFactory = urlHelperFactory;
		}


		public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			var objectResult = context.Result as ObjectResult;

			var shouldSkip = objectResult?.Value == null || objectResult?.StatusCode != (int)HttpStatusCode.OK;

			if (shouldSkip)
			{
				await next();
				return;
			}

			var rewriter = new LinkRewriter(_urlHelperFactory.GetUrlHelper(context));

			RewriteAllLinks(objectResult.Value, rewriter);

			await next();
		}

		private static void RewriteAllLinks(object model, LinkRewriter rewriter)
		{
			if (model == null) return;

			var allProperties = model
				.GetType()
				.GetTypeInfo()
				.GetAllProperties()
				.Where(p => p.CanRead)
				.ToList();

			var linkProperties = allProperties
				.Where(p => p.CanWrite && p.PropertyType.Equals(typeof(Link)));

			foreach (var linkProperty in linkProperties)
			{
				var rewritten = rewriter.Rewrite(linkProperty.GetValue(model) as Link);
				if (rewritten == null) continue;
				linkProperty.SetValue(model, rewritten);

				//Special handling of the hidden Self Property

				if (linkProperty.Name.Equals(nameof(ApiResource.Self)))
				{
					allProperties.SingleOrDefault(p => p.Name.Equals(nameof(ApiResource.Href)))?.SetValue(model, rewritten.Href);

					allProperties.SingleOrDefault(p => p.Name.Equals(nameof(ApiResource.Method)))?.SetValue(model, rewritten.Href);

					allProperties.SingleOrDefault(p => p.Name.Equals(nameof(ApiResource.Relations)))?.SetValue(model, rewritten.Href);
				}
			}


			var arrayProperties = allProperties
				.Where(p => p.PropertyType.IsArray);
			RewriteLinksInArrays(arrayProperties, model, rewriter);

			//var collectionProperties = allProperties
			//	.Where(p => p.PropertyType.IsGenericType && p.PropertyType.Equals(typeof(ICollection<>)));
			//RewriteLinksInArrays(collectionProperties, model, rewriter);

			var objectProperties = allProperties
				.Except(linkProperties)
				.Except(arrayProperties)
				//.Except(collectionProperties)
				;
			RewriteLinksInNestedObjects(objectProperties, model, rewriter);
		}

		private static void RewriteLinksInNestedObjects(
		  IEnumerable<PropertyInfo> objectProperties,
		  object obj,
		  LinkRewriter rewriter)
		{
			foreach (var objectProperty in objectProperties)
			{
				if (objectProperty.PropertyType == typeof(string))
				{
					continue;
				}

				var typeInfo = objectProperty.PropertyType.GetTypeInfo();
				if (typeInfo.IsClass)
				{
					RewriteAllLinks(objectProperty.GetValue(obj), rewriter);
				}
			}
		}

		private static void RewriteLinksInArrays(
		    IEnumerable<PropertyInfo> arrayProperties,
		    object obj,
		    LinkRewriter rewriter)
		{
			foreach (var arrayProperty in arrayProperties)
			{
				var array = arrayProperty.GetValue(obj) as Array ?? new Array[0];

				foreach (var element in array)
				{
					RewriteAllLinks(element, rewriter);
				}
			}
		}

		//private static void RewriteLinksInCollections(
		//    IEnumerable<PropertyInfo> collectionProperties,
		//    object obj,
		//    LinkRewriter rewriter)
		//{
		//	foreach (var collectionProperty in collectionProperties)
		//	{
		//		var collectionTypes = collectionProperty
		//			.PropertyType
		//			.GetGenericArguments();

		//		var collection = collectionProperty.GetValue(obj) as typeof(ICollection<>).MakeGenericType(collectionTypes);

		//		foreach (var element in collection)
		//		{
		//			RewriteAllLinks(element, rewriter);
		//		}
		//	}
		//}
	}
}
