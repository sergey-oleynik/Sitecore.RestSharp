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

namespace Sitecore.RestSharp.Service
{
  using System.Collections.Generic;
  using System.Xml;
  using global::RestSharp;
  using global::RestSharp.Deserializers;
  using global::RestSharp.Serializers;
  using Sitecore.Reflection;
  using Sitecore.RestSharp.Extentions;
  using Sitecore.RestSharp.Parameters;
  using Sitecore.RestSharp.Request;
  using Sitecore.RestSharp.Tokens;
  using Sitecore.Xml;

  public class ServiceConfiguration : IServiceConfiguration, IInitializable
  {
    public ServiceConfiguration()
    {
      this.TokenReplacers = new List<ITokenReplacer>();
      this.ParameterReplacers = new List<IParameterReplacer>();

      this.Handlers = new Dictionary<string, IDeserializer>();
      this.Headers = new Dictionary<string, string>();
    }

    public IRequestProvider RequestProvider { get; protected set; }

    public string BaseUrl { get; protected set; }

    public List<ITokenReplacer> TokenReplacers { get; set; }

    public List<IParameterReplacer> ParameterReplacers { get; set; }

    public ISerializer XmlSerializer { get; set; }

    public ISerializer JsonSerializer { get; set; }

    public IAuthenticator Authenticator { get; set; }

    public Dictionary<string, IDeserializer> Handlers { get; protected set; }

    public Dictionary<string, string> Headers { get; protected set; }

    #region Initialize

    public bool AssignProperties { get; private set; }

    public virtual void Initialize(XmlNode configNode)
    {
      this.AssignProperties = false;

      this.RequestProvider = XmlUtil.GetChildNode("requestProvider", configNode).CreateObject(true) as IRequestProvider ?? new RequestProvider();

      this.BaseUrl = XmlUtil.GetAttribute("baseUrl", configNode);

      this.JsonSerializer = XmlUtil.GetChildNode("jsonSerializer", configNode).CreateObject(true) as ISerializer;
      this.XmlSerializer = XmlUtil.GetChildNode("xmlSerializer", configNode).CreateObject(true) as ISerializer;

      this.Authenticator = XmlUtil.GetChildNode("authenticator", configNode).CreateObject(true) as IAuthenticator;

      this.AddHandlers(configNode);

      this.AddHeaders(configNode);

      this.AddParameterReplacers(configNode);

      this.AddTokenReplacers(configNode);
    }

    protected virtual void AddHandlers(XmlNode configNode)
    {
      XmlNode handlersNode = XmlUtil.GetChildNode("handlers", configNode);
      if (handlersNode == null)
      {
        return;
      }

      foreach (XmlNode childNode in XmlUtil.GetChildNodes(handlersNode, true))
      {
        string contentType = XmlUtil.GetAttribute("contentType", childNode);
        IDeserializer deserializer = childNode.CreateObject(true) as IDeserializer;

        if (!string.IsNullOrEmpty(contentType) && deserializer != null)
        {
          this.Handlers.Add(contentType, deserializer);
        }
      }
    }

    protected virtual void AddHeaders(XmlNode configNode)
    {
      XmlNode headersNode = XmlUtil.GetChildNode("headers", configNode);
      if (headersNode == null)
      {
        return;
      }
      foreach (XmlNode childNode in XmlUtil.GetChildNodes(headersNode, true))
      {
        string name = XmlUtil.GetAttribute("name", childNode);
        string value = XmlUtil.GetAttribute("value", childNode);

        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
        {
          this.Headers.Add(name, value);
        }
      }
    }

    protected virtual void AddParameterReplacers(XmlNode configNode)
    {
      this.ParameterReplacers.AddRange(XmlUtil.GetChildNode("parameters", configNode).ReadChilds<IParameterReplacer>());
    }

    protected virtual void AddTokenReplacers(XmlNode configNode)
    {
      this.TokenReplacers.AddRange(XmlUtil.GetChildNode("tokens", configNode).ReadChilds<ITokenReplacer>());
    }
    #endregion
  }
}