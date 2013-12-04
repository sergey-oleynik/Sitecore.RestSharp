namespace Sitecore.RestSharp.Parameters
{
  using global::RestSharp;

  public class DymamicParameterReplacer : SingleParameterReplacer
  {
    protected override void SetParameter(IRestRequest request, Parameter parameter)
    {
      object value = this.GetValue(parameter);

      if (value != null)
      {
        string key = '{' + parameter.Name + '}';
        if (request.Resource.Contains(key))
        {
          request.Resource = request.Resource.Replace(key, value.ToString());
        }
        else
        {
          request.Resource += (request.Resource.Contains("?") ? '&' : '?') + parameter.Name + '=' + value;
        }
      }
    }
  }
}