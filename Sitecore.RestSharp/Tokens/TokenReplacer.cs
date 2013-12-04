namespace Sitecore.RestSharp.Tokens
{
  using global::RestSharp;

  public abstract class TokenReplacer : ITokenReplacer
  {
    public string Token { get; set; }

    public virtual void ReplaceToken(IRestRequest request)
    {
      if (!string.IsNullOrEmpty(this.Token))
      {
        request.AddUrlSegment(this.Token, this.GetValue());
      }
    }

    protected abstract string GetValue();
  }
}