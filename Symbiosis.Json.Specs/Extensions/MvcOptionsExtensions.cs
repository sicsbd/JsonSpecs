using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MvcOptionsExtensions
	{
		public static bool RemoveJsonOutputFormatter(this MvcOptions options)
		{
			var formatter = options.OutputFormatters.OfType<JsonOutputFormatter>().Single();
			return options.OutputFormatters.Remove(formatter);
		}

		public static bool RemoveJsonOutputFormatter(this MvcOptions options, out JsonOutputFormatter formatter)
		{
			formatter = options.OutputFormatters.OfType<JsonOutputFormatter>().Single();
			return options.OutputFormatters.Remove(formatter);
		}

		public static MvcOptions AddIonOutputFormatter(this MvcOptions options)
		=> options.AddIonOutputFormatter(true);

		public static MvcOptions AddIonOutputFormatter(this MvcOptions options, bool removeJsonOutputFormatter)
		{
			var formatter = options.OutputFormatters.OfType<JsonOutputFormatter>().Single();
			if (removeJsonOutputFormatter)
			{
				options.RemoveJsonOutputFormatter();
				options.OutputFormatters.Add(new IonOutputFormatter(formatter));
			}
			else
				options.OutputFormatters.Insert(0, new IonOutputFormatter(formatter));
			return options;
		}
		public static MvcOptions AddLinkRewritingFilter(this MvcOptions options)
		{
			options.Filters.Add<LinkRewritingFilter>();
			return options;
		}
	}
}
