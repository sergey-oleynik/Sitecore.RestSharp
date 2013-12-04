
namespace Sitecore.RestSharp.Caching
{
  using System.Collections.Concurrent;
  using Sitecore.Configuration;
  using Sitecore.RestSharp.Extentions;
  using Sitecore.RestSharp.Service;

  public class Cache : ICache
  {
    private const string ConfigurationNode = "Sitecore.RestSharp";

    protected readonly ConcurrentDictionary<string, IServiceConfiguration> ServiceConfigurations = new ConcurrentDictionary<string, IServiceConfiguration>();

    public virtual IServiceConfiguration GetServiceConfiguration(string serviceName)
    {
      IServiceConfiguration service;
      ServiceConfigurations.TryGetValue(serviceName.ToLowerInvariant(), out service);

      if (service == null)
      {
        service = this.CreateServiceConfiguration(serviceName);
        if (service != null)
        {
          this.ServiceConfigurations.TryAdd(serviceName.ToLowerInvariant(), service);
        }
      }

      return service;
    }

    protected virtual IServiceConfiguration CreateServiceConfiguration(string serviceName)
    {
      var configNode = Factory.GetConfigNode(string.Format(ConfigurationNode + "/service[@name='{0}']", serviceName));

      return configNode.CreateObject(true) as IServiceConfiguration;
    }
  }
}