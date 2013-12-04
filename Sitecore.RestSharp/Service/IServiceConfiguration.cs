
using Sitecore.RestSharp.Request;

namespace Sitecore.RestSharp.Service
{
  using System.Collections.Generic;
  
  using global::RestSharp;
  using global::RestSharp.Deserializers;
  using global::RestSharp.Serializers;

  using Sitecore.RestSharp.Parameters;
  using Sitecore.RestSharp.Tokens;

  public interface IServiceConfiguration
  {
    IRequestProvider RequestProvider { get; }

    string BaseUrl { get; }

    List<ITokenReplacer> TokenReplacers { get; }

    List<IParameterReplacer> ParameterReplacers { get; }

    ISerializer XmlSerializer { get; }

    ISerializer JsonSerializer { get; }

    IAuthenticator Authenticator { get; }

    Dictionary<string, IDeserializer> Handlers { get; }

    Dictionary<string, string> Headers { get; }
  }
}