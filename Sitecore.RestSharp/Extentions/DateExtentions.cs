using System;

namespace Sitecore.RestSharp.Extentions
{
  public static class DateExtentions
  {
    public static readonly DateTime EpochDatetime = new DateTime(1970, 1, 1);

    public static int ToUnixTimestamp(this DateTime dateTime)
    {
      return (int)(DateTime.UtcNow - EpochDatetime).TotalSeconds;
    }
  }
}