/*
   Copyright 2014 Sergey Oleynik
 
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

namespace Sitecore.RestSharp.Service
{
  using System.Collections.Generic;
  using System.Xml;
  using global::RestSharp;
  using global::RestSharp.Authenticators;
  using global::RestSharp.Deserializers;
  using global::RestSharp.Serializers;
  using Sitecore.Configuration;
  using Sitecore.Diagnostics;
  using Sitecore.RestSharp.Parameters;
  using Sitecore.RestSharp.Request;
  using Sitecore.RestSharp.Tokens;
  using Sitecore.Xml;

  public class ServiceConfiguration : IServiceConfiguration
  {
    public ServiceConfiguration()
    {
      this.TokenReplacers = new Dictionary<string, ITokenReplacer>();
      this.ParameterReplacers = new Dictionary<string,IParameterReplacer>();

      this.Handlers = new Dictionary<string, IDeserializer>();
      this.Headers = new Dictionary<string, string>();
    }

    public IRequestProvider RequestProvider { get; set; }

    public string BaseUrl { get; set; }

    public ISerializer XmlSerializer { get; set; }

    public ISerializer JsonSerializer { get; set; }

    public IAuthenticator Authenticator { get; set; }

    public Dictionary<string, ITokenReplacer> TokenReplacers { get; private set; }

    public Dictionary<string, IParameterReplacer> ParameterReplacers { get; private set; }

    public Dictionary<string, IDeserializer> Handlers { get; private set; }

    public Dictionary<string, string> Headers { get; private set; }

    #region Initialize

    public void AddHandler(XmlNode configNode)
    {
      Assert.ArgumentNotNull(configNode, "configNode");

      string contentType = XmlUtil.GetAttribute("contentType", configNode);

      if (!string.IsNullOrEmpty(contentType))
      {
        IDeserializer obj = Factory.CreateObject(configNode, true) as IDeserializer;

        if (obj != null)
        {
          this.Handlers[contentType] = obj;
        }
      }
    }

    public void AddHeader(XmlNode configNode)
    {
      Assert.ArgumentNotNull(configNode, "configNode");

      string name = XmlUtil.GetAttribute("name", configNode);
      string value = XmlUtil.GetAttribute("value", configNode);

      if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
      {
        this.Headers[name] = value;
      }
    }

    public void AddTokenReplacer(XmlNode configNode)
    {
      Assert.ArgumentNotNull(configNode, "configNode");

      string name = XmlUtil.GetAttribute("name", configNode);

      if (!string.IsNullOrEmpty(name))
      {
        ITokenReplacer obj = Factory.CreateObject(configNode, true) as ITokenReplacer;

        if (obj != null)
        {
          this.TokenReplacers[name] = obj;
        }
      }
    }

    public void AddParameterReplacer(XmlNode configNode)
    {
      Assert.ArgumentNotNull(configNode, "configNode");

      string name = XmlUtil.GetAttribute("name", configNode);

      if (!string.IsNullOrEmpty(name))
      {
        IParameterReplacer obj = Factory.CreateObject(configNode, true) as IParameterReplacer;

        if (obj != null)
        {
          this.ParameterReplacers[name] = obj;
        }
      }
    }
    #endregion
  }
}