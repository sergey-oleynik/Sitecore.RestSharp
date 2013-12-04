namespace Sitecore.RestSharp.Tokens
{
  using global::RestSharp;

  public interface ITokenReplacer
  {
    string Token { get; }

    void ReplaceToken(IRestRequest request);
  }
}