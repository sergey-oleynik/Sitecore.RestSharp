/*
   Copyright 2013 Sergey Oleynik
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

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