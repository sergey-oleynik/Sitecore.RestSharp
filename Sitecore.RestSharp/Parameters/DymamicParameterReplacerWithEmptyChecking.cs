using RestSharp;

namespace Sitecore.RestSharp.Parameters
{
  public class DymamicParameterReplacerWithEmptyChecking : DymamicParameterReplacer
  {
    protected override object GetValue(Parameter parameter)
    {
      if (parameter != null && parameter.Value != null)
      {
        string str = parameter.Value.ToString();

        return !string.IsNullOrEmpty(str) ? str : null;
      }

      return null;
    }
  }
}