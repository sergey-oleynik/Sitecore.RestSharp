
namespace Sitecore.RestSharp
{
  using System;

  using System.Collections.Generic;
  
  using global::RestSharp;

  using Sitecore.Configuration;
  using Sitecore.RestSharp.Caching;
  using Sitecore.RestSharp.Data;
  using Sitecore.RestSharp.Service;

  public class RestContext
  {
    protected static readonly ICache Cache;

    static RestContext()
    {
      var setting = Settings.GetSetting("Sitecore.RestSharp.Cache");
      
      if (setting.Length > 0)
      {
        Cache = Activator.CreateInstance(Type.GetType(setting,true,false)) as ICache;
      }

      if (Cache == null)
      {
        Cache = new Cache();
      }
    }

    public RestContext(string serviceName, IAuthenticator authenticator = null) : this(Cache.GetServiceConfiguration(serviceName), authenticator)
    {
    }

    public RestContext(IServiceConfiguration service, IAuthenticator authenticator = null)
    {
      this.Service = service;

      this.Client = this.CreateClient();
      this.Client.Authenticator = authenticator ?? this.Service.Authenticator;
    }

    protected IServiceConfiguration Service { get; set; }

    public IRestClient Client { get; protected set; }

    public IRestResponse<TResult> Create<TSource, TResult>(TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Create<TSource, TResult>(null, body, parameters);
    }

    public IRestResponse<TResult> Create<TSource, TResult>(string requestName, TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Create, requestName, body, parameters);
    }

    public IRestResponse<TResult> Read<TSource, TResult>(TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Read<TSource, TResult>(null, body, parameters);
    }

    public IRestResponse<TResult> Read<TSource, TResult>(string requestName, TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Read, requestName, body, parameters);
    }

    public IRestResponse<TResult> Read<TResult>(IList<Parameter> parameters = null)
      where TResult : class, new()
    {
      return this.Read<RestEmptyType, TResult>(null, null, parameters);
    }

    public IRestResponse<TResult> Read<TResult>(string requestName, IList<Parameter> parameters = null)
      where TResult : class, new()
    {
      return this.Read<RestEmptyType, TResult>(requestName, null, parameters);
    }

    public IRestResponse<TResult> Update<TSource, TResult>(TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Update<TSource, TResult>(null, body, parameters);
    }

    public IRestResponse<TResult> Update<TSource, TResult>(string requestName, TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Update, requestName, body, parameters);
    }

    public IRestResponse<TResult> Delete<TSource, TResult>(TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Delete<TSource, TResult>(null, body, parameters);
    }

    public IRestResponse<TResult> Delete<TSource, TResult>(string requestName, TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, EntityActionType.Delete, requestName, body, parameters);
    }

    public IRestResponse<TResult> Delete<TResult>(IList<Parameter> parameters = null)
      where TResult : class, new()
    {
      return this.Delete<RestEmptyType, TResult>(null, null, parameters);
    }

    public IRestResponse<TResult> Delete<TResult>(string requestName, IList<Parameter> parameters = null)
      where TResult : class, new()
    {
      return this.Delete<RestEmptyType, TResult>(requestName, null,parameters);
    }

    public IRestResponse<TResult> GetResult<TSource, TResult>(EntityActionType actionType, TSource body, IList<Parameter> parameters)
      where TSource : class
      where TResult : class, new()
    {
      return this.GetResult<TSource, TResult>(actionType, null, body, parameters);
    }

    public IRestResponse<TResult> GetResult<TSource, TResult>(EntityActionType actionType, string requestName, TSource body, IList<Parameter> parameters)
      where TSource : class
      where TResult : class, new()
    {
      return this.Service.RequestProvider.GetResult<TSource, TResult>(this.Service, this.Client, actionType, requestName, body, parameters);
    }

    public IRestRequest CreateRequest<TSource, TResult>(EntityActionType actionType, string requestName, TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.Service.RequestProvider.CreateRequest<TSource, TResult>(this.Service, actionType, requestName, body, parameters);
    }

    public IRestRequest CreateRequest<TSource, TResult>(EntityActionType actionType, TSource body = default(TSource), IList<Parameter> parameters = null)
      where TSource : class
      where TResult : class, new()
    {
      return this.CreateRequest<TSource, TResult>(actionType, null, body, parameters);
    }
    public IRestResponse<TResult> GetResponse<TResult>(IRestRequest restRequest)
      where TResult : class, new()
    {
      return this.Service.RequestProvider.GetResponse<TResult>(this.Service, this.Client, restRequest);
    }

    protected virtual IRestClient CreateClient()
    {
      RestClient client = new RestClient(this.Service.BaseUrl);

      foreach (var pair in this.Service.Handlers)
      {
        client.AddHandler(pair.Key, pair.Value);
      }

      foreach (var pair in this.Service.Headers)
      {
        client.AddDefaultHeader(pair.Key, pair.Value);
      }

      return client;
    }
  }
}