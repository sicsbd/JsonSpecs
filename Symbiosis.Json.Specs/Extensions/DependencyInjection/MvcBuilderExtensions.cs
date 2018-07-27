using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MvcBuilderExtensions
	{
		public static IMvcBuilder AddIon(
			this IMvcBuilder mvcBuilder,
			Action<MvcIonOptions> options = null) => mvcBuilder
				.AddMvcOptions(mvcOptions =>
				{
					var opt = new MvcIonOptions();
					options?.Invoke(opt);
					if (opt.AddLinkRewritingFilter)
						mvcOptions
							.AddLinkRewritingFilter();
					if (opt.AddOutputFormatter)
						mvcOptions
							.AddIonOutputFormatter(opt.RemoveJsonOutputformatter);
				});
	}
}
