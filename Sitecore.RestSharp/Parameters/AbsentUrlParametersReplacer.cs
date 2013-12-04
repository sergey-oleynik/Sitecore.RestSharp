namespace Sitecore.RestSharp.Parameters
{
  using global::RestSharp;

  public class AbsentUrlParametersReplacer : MultiUrlParameterReplacer
  {
    protected override void SetParameter(IRestRequest request, Parameter parameter)
    {
      string key = '{' + parameter.Name + '}';

      if (!request.Resource.Contains(key))
      {
        request.Resource += (request.Resource.Contains("?") ? '&' : '?') + parameter.Name + '=' + this.GetValue(parameter);
      }
    }
  }
}