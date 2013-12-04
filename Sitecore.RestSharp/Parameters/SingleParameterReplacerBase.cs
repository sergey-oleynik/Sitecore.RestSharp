using System.Xml;
using RestSharp;
using Sitecore.Reflection;
using Sitecore.Xml;

namespace Sitecore.RestSharp.Parameters
{
  public abstract class SingleParameterReplacerBase : ParameterReplacerBase, IInitializable
  {
    public string Parameter { get; private set; }

    public virtual void Initialize(XmlNode configNode)
    {
      this.AssignProperties = false;

      this.Parameter = XmlUtil.GetAttribute("parameter", configNode);
    }

    public bool AssignProperties { get; private set; }

    protected virtual Parameter GetParameter(IRestRequest request)
    {
      return this.GetParameter(request, this.Parameter);
    }
  }
}