namespace Sitecore.RestSharp.Serialization
{
  using Newtonsoft.Json;

  using global::RestSharp.Serializers;

  public class JsonNetSerializer : ISerializer
  {
    public string Serialize(object obj)
    {
      return JsonConvert.SerializeObject(obj);
    }

    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }

    public string ContentType { get; set; }
  }
}