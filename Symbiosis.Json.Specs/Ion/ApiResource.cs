using Newtonsoft.Json;

namespace Symbiosis.Json.Specs.Ion
{
	public abstract class ApiResource
		: Link
    {
		[JsonIgnore]
		public Link Self { get; set; }
    }
}
