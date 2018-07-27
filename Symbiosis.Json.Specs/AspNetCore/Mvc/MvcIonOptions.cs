namespace Microsoft.AspNetCore.Mvc
{
	public class MvcIonOptions
	{
		public bool AddOutputFormatter { get; set; } = true;
		public bool AddLinkRewritingFilter { get; set; } = true;
		public bool RemoveJsonOutputformatter { get; set; } = true;
	}
}
