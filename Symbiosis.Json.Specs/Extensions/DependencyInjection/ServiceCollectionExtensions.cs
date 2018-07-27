using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddPagingOptions(this IServiceCollection services, IConfiguration configuration) 
			=> services.Configure<PagingOptions>(configuration);
	}
}
