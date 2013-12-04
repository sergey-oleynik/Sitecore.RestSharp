namespace Sitecore.RestSharp.Parameters
{
  using global::RestSharp;

  public interface IParameterReplacer
  {
    void ReplaceParameters(IRestRequest request);
  }
}
