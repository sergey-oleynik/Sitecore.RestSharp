using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace Sitecore.RestSharp.Parameters
{
  public abstract class UrlParameterReplacerBase : ParameterReplacerBase
  {
    protected override IEnumerable<Parameter> GetParameters(IRestRequest request)
    {
      return request.Parameters.Where(i => i.Type == ParameterType.UrlSegment);
    }
  }
}