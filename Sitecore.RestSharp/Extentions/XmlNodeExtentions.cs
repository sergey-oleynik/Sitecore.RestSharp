

namespace Sitecore.RestSharp.Extentions
{
  using System.Collections.Generic;
  using System.Xml;
  using Sitecore.Configuration;
  using Sitecore.Xml;

  public static class XmlNodeExtentions
  {
    public static T CreateObject<T>(this XmlNode node) where T : class
    {
      return node != null ? Factory.CreateObject<T>(node) : default(T);
    }

    public static object CreateObject(this XmlNode node, bool assert)
    {
      return node != null ? Factory.CreateObject(node, assert) : null;
    }

    public static List<T> ReadChilds<T>(this XmlNode configNode, List<T> list = null) where T : class
    {
      list = list ?? new List<T>();

      if (configNode == null)
      {
        return list;
      }

      foreach (XmlNode childNode in XmlUtil.GetChildNodes(configNode, true))
      {
        switch (childNode.LocalName)
        {
          case "add":
            if (XmlUtil.GetAttribute("mode", childNode) != "off")
            {
              T obj = CreateObject<T>(childNode);

              if (obj != default(T))
              {
                list.Add(obj);
              }
            }
            break;
          case "clear":
            list.Clear();
            break;
        }
      }

      return list;
    }
  }
}