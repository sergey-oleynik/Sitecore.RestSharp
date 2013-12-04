using RestSharp;

namespace Sitecore.RestSharp.Parameters
{
  public class MultiParameterReplacer : ParameterReplacerBase
  {
    public override void ReplaceParameters(IRestRequest request)
    {
      var parameters = this.GetParameters(request);

      foreach (var parameter in parameters)
      {
        this.SetParameter(request, parameter);
      }
    }
  }
}