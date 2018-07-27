namespace Symbiosis.Json.Specs.Ion.Generics
{
	public class Collection<T> 
		: ApiResource
	{
		public T[] Value { get; set; }
	}

}
