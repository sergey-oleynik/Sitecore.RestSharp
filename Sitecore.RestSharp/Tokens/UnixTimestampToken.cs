
namespace Sitecore.RestSharp.Tokens
{
  using System;
  using System.Globalization;
  using Sitecore.RestSharp.Extentions;

  public class UnixTimestampToken : TokenReplacer
  {
    protected override string GetValue()
    {
      return this.GetTimestamp().ToString(CultureInfo.InvariantCulture);
    }

    protected virtual int GetTimestamp()
    {
      return DateTime.UtcNow.ToUnixTimestamp();
    }
  }
}