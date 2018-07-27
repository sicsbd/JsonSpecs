using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;

#pragma warning disable CS0436 // Type conflicts with imported type
namespace Symbiosis.Json.Specs.Ion
{
	public class Link : Etaggable
	{
		[JsonProperty(Order = -4, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Href { get; set; }

		[JsonProperty(Order = -3, NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		[DefaultValue(HttpMethods.GET)]
		public string Method { get; set; }

		[JsonProperty(Order = -2, PropertyName = "rel", NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string[] Relations { get; set; }
		[JsonIgnore]
		public string RouteName { get; set; }
		[JsonIgnore]
		public object RouteValues { get; set; }

		public static Link To(string routeName, object routeValues)
			=> new Link
			{
				RouteName = routeName,
				RouteValues = routeValues,
				Method = HttpMethods.GET,
				Relations = null
			};

		public static Link ToCollection(string routeName, object routeValues = null)
		    => new Link
		    {
			    RouteName = routeName,
			    RouteValues = routeValues,
			    Method = HttpMethods.GET,
			    Relations = new string[] { "collection" }
		    };

		public static Link ToForm(
		    string routeName,
		    object routeValues = null,
		    string method = HttpMethods.POST,
		    params string[] relations)
		    => new Link
		    {
			    RouteName = routeName,
			    RouteValues = routeValues,
			    Method = method,
			    Relations = relations
		    };
	}
}

#pragma warning restore CS0436 // Type conflicts with imported type