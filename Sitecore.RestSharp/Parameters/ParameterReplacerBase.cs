using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace Sitecore.RestSharp.Parameters
{
  public abstract class ParameterReplacerBase : IParameterReplacer
  {
    public abstract void ReplaceParameters(IRestRequest request);

    [CanBeNull]
    protected virtual object GetValue(Parameter parameter)
    {
      return parameter != null ? parameter.Value : null;
    }

    [NotNull]
    protected virtual IEnumerable<Parameter> GetParameters(IRestRequest request)
    {
      return request.Parameters;
    }

    [CanBeNull]
    protected virtual Parameter GetParameter(IRestRequest request, string name)
    {
      return request.Parameters.FirstOrDefault(p => p.Name == name);
    }

    protected virtual void SetParameter(IRestRequest request, Parameter parameter)
    {
      object value = this.GetValue(parameter);
      if (value != null)
      {
        request.Resource = request.Resource.Replace('{' + parameter.Name + '}', value.ToString());
      }
    }
  }
}