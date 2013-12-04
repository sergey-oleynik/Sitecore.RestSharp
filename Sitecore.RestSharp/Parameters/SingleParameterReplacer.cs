using RestSharp;

namespace Sitecore.RestSharp.Parameters
{
  public class SingleParameterReplacer : SingleParameterReplacerBase
  {
    public override void ReplaceParameters(IRestRequest request)
    {
      Parameter parameter = this.GetParameter(request);
      
      this.SetParameter(request, parameter);
    }
  }
}