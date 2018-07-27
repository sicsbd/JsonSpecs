using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPagingOptions(this IServiceCollection services, IConfiguration configuration) 
			=> services.Configure<PagingOptions>(configuration);

		public static IServiceCollection AddPagingOptions(this IServiceCollection services, Action<PagingOptions> pagingOptionsAction)
			=> services.Configure<PagingOptions>(pagingOptionsAction);
	}
}
