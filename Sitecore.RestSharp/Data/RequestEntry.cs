namespace Sitecore.RestSharp.Data
{
  using System;
  using System.Collections.Specialized;
  using System.Web;
  using System.Xml;

  using Sitecore.Configuration;
  using Sitecore.Reflection;
  using Sitecore.Xml;

  using global::RestSharp;

  public class RequestEntry : IInitializable, IConstructable
  {
    private bool isMethodAssigned;

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
      this.isMethodAssigned = Enum.TryParse(attributes["method"], true, out method);

      this.Method = method;

      this.RequestType = ReflectionUtil.GetTypeInfo(attributes["requestType"] ?? string.Empty) ?? typeof(RestEmptyType);
      this.ResponseType = ReflectionUtil.GetTypeInfo(attributes["responseType"] ?? string.Empty) ?? typeof(RestEmptyType);
    }

    public void Constructed(XmlNode configuration)
    {
      if (!this.isMethodAssigned)
      {
        this.Method = this.GetDefaultMethod(configuration.LocalName);
      }
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