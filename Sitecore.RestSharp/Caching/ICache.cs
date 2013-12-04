
namespace Sitecore.RestSharp.Caching
{
  using Sitecore.RestSharp.Service;

  public interface ICache
  {
    IServiceConfiguration GetServiceConfiguration(string serviceName);
  }
}