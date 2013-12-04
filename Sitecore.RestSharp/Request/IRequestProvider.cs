
namespace Sitecore.RestSharp.Request
{
  using System.Collections.Generic;
  using global::RestSharp;
  using Sitecore.RestSharp.Data;
  using Sitecore.RestSharp.Service;

  public interface IRequestProvider
  {
    DataFormat Format { get; set; }

    IRestResponse<TResult> GetResult<TSource, TResult>(IServiceConfiguration serviceConfiguration, IRestClient client, EntityActionType actionType, string requestName, TSource body, IList<Parameter> parameters)
      where TSource : class
      where TResult : class, new();

    IRestRequest CreateRequest<TSource, TResult>(IServiceConfiguration serviceConfiguration, EntityActionType actionType, string requestName, TSource body, IList<Parameter> parameters)
      where TSource : class
      where TResult : class, new();

    IRestResponse<TResult> GetResponse<TResult>(IServiceConfiguration serviceConfiguration, IRestClient client, IRestRequest restRequest)
      where TResult : class, new();
  }
}
