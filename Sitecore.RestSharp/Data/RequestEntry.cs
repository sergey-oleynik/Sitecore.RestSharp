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

namespace Sitecore.RestSharp.Data
{
  using System;
  using System.Collections.Specialized;
  using System.Web;
  using System.Xml;

  using Sitecore.Reflection;
  using Sitecore.Xml;

  using global::RestSharp;

  public class RequestEntry : IInitializable
  {
    public string Name { get; set; }

    public EntityActionType ActionType { get; set; }

    public Type RequestType { get; set; }
    
    public Type ResponseType { get; set; }

    public string Url { get; set; }

    public Method Method { get; set; }

    #region Initialize

    public void Initialize(XmlNode configNode)
    {
      this.AssignProperties = true;

      NameValueCollection attributes = XmlUtil.GetAttributes(configNode);

      this.Name = attributes["name"];
      this.ActionType = this.GetActionType(configNode.LocalName);
      this.Url = HttpUtility.UrlDecode(attributes["url"]);

      Method method;
      if (!Enum.TryParse(attributes["method"], true, out method))
      {
        method = this.GetDefaultMethod(configNode.LocalName);
      }

      this.Method = method;

      this.RequestType = ReflectionUtil.GetTypeInfo(attributes["requestType"] ?? string.Empty) ?? typeof(RestEmptyType);
      this.ResponseType = ReflectionUtil.GetTypeInfo(attributes["responseType"] ?? string.Empty) ?? typeof(RestEmptyType);
    }

    public bool AssignProperties { get; private set; }

    public virtual EntityActionType GetActionType(string nodeName)
    {
      switch (nodeName)
      {
        case "create":
          return EntityActionType.Create;
        case "read":
          return EntityActionType.Read;
        case "update":
          return EntityActionType.Update;
        case "delete":
          return EntityActionType.Delete;
        default:
          return EntityActionType.Read;
      }
    }

    protected virtual Method GetDefaultMethod(string nodeName)
    {
      switch (nodeName)
      {
        case "create":
          return Method.POST;
        case "read":
          return Method.GET;
        case "update":
          return Method.PUT;
        case "delete":
          return Method.DELETE;
        default:
          return Method.GET;
      }
    }
    #endregion
  }
}