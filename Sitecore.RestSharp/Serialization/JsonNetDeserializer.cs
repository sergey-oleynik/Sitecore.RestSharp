﻿namespace Sitecore.RestSharp.Serialization
{
  using Newtonsoft.Json;

  using global::RestSharp;
  using global::RestSharp.Deserializers;

  public class JsonNetDeserializer : IDeserializer
  {
    public T Deserialize<T>(IRestResponse response)
    {
      return JsonConvert.DeserializeObject<T>(response.Content);
    }

    public string RootElement { get; set; }

    public string Namespace { get; set; }

    public string DateFormat { get; set; }
  }
}