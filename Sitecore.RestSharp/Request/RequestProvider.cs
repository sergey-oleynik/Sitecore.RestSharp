/*
   Copyright 2013 Sergey Oleynik
 
   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

namespace Sitecore.RestSharp.Request
{
  using System;
  using System.Collections.Generic;
  using System.Xml;

  using Sitecore.Configuration;

  using global::RestSharp;
  using Sitecore.Diagnostics;
  using Sitecore.RestSharp.Data;
  using Sitecore.RestSharp.Service;

  public class RequestProvider : IRequestProvider
  {
    protected Dictionary<Tuple<EntityActionType, Type, Type>, RequestEntry> RequestsByTypes { get; private set; }

    protected Dictionary<Tuple<EntityActionType, string>, RequestEntry> RequestsByName { get; private set; }

    public RequestProvider()
    {
      this.RequestsByTypes = new Dictionary<Tuple<EntityActionType, Type, Type>, RequestEntry>();
      this.RequestsByName = new Dictionary<Tuple<EntityActionType, string>, RequestEntry>();
    }

    public void AddRequest(XmlNode node)
    {
      Assert.ArgumentNotNull(node, "node");

      var request = Factory.CreateObject<RequestEntry>(node);

      if (request != null)
      {
        this.RegisterRequest(request);
      }
    }

    public DataFormat Format { get; set; }

    public virtual IRestResponse<TResult> GetResult<TSource, TResult>(IServiceConfiguration serviceConfiguration, IRestClient client, EntityActionType actionType, string requestName, TSource body, IList<Parameter> parameters)
      where TSource : class
      where TResult : class, new()
    {
      IRestRequest restRequest = this.CreateRequest<TSource, TResult>(serviceConfiguration, actionType, requestName, body, parameters);

      return restRequest == null ? null : this.GetResponse<TResult>(serviceConfiguration, client, restRequest);
    }

    public virtual IRestRequest CreateRequest<TSource, TResult>(IServiceConfiguration serviceConfiguration, EntityActionType actionType, string requestName, TSource body, IList<Parameter> parameters)
      where TSource : class
      where TResult : class, new()
    {
      RequestEntry request = !string.IsNullOrEmpty(requestName) ? this.ResolveRequest(actionType, requestName) : this.ResolveRequest<TSource, TResult>(actionType);

      if (request == null)
      {
        Log.Warn("Sitecore.RestSharp: Could not resolve request.", this);
        return null;
      }

      return this.BindRequestPorperties(serviceConfiguration, request, body, parameters);
    }

    public virtual IRestResponse<TResult> GetResponse<TResult>(IServiceConfiguration serviceConfiguration, IRestClient client, IRestRequest restRequest)
      where TResult : class, new()
    {
      try
      {
        return client.Execute<TResult>(restRequest);
      }
      catch
      {
        Log.Warn("Sitecore.RestSharp: Error during request executing.", this);
        return null;
      }
    }

    protected virtual RequestEntry ResolveRequest<TSource, TResult>(EntityActionType actionType)
      where TSource : class
      where TResult : class, new()
    {
      RequestEntry request;
      this.RequestsByTypes.TryGetValue(Tuple.Create(actionType, typeof(TSource), typeof(TResult)), out request);
      return request;
    }

    protected virtual RequestEntry ResolveRequest(EntityActionType actionType, string requestName)
    {
      RequestEntry request;
      this.RequestsByName.TryGetValue(Tuple.Create(actionType, requestName), out request);
      return request;
    }

    protected virtual IRestRequest BindRequestPorperties<TSource>(IServiceConfiguration serviceConfiguration, RequestEntry request, TSource body, IList<Parameter> parameters)
      where TSource : class
    {
      IRestRequest restRequest = new RestRequest(request.Url, request.Method)
        {
          RequestFormat = this.Format
        };
      if (serviceConfiguration.JsonSerializer != null)
      {
        restRequest.JsonSerializer = serviceConfiguration.JsonSerializer;
      }

      if (serviceConfiguration.XmlSerializer != null)
      {
        restRequest.XmlSerializer = serviceConfiguration.XmlSerializer;
      }

      if (body != default(TSource))
      {
        restRequest.AddBody(body);
      }

      foreach (var pair in serviceConfiguration.TokenReplacers)
      {
        pair.Value.ReplaceToken(restRequest, pair.Key);
      }

      if (parameters != null && parameters.Count > 0)
      {
        restRequest.Parameters.AddRange(parameters);
      }

      foreach (var pair in serviceConfiguration.ParameterReplacers)
      {
        pair.Value.ReplaceParameter(restRequest, pair.Key);
      }

      return restRequest;
    }

    protected virtual void RegisterRequest(RequestEntry request)
    {
      if (!string.IsNullOrEmpty(request.Name))
      {
        var key = Tuple.Create(request.ActionType, request.Name);

        if (!this.RequestsByName.ContainsKey(key))
        {
          this.RequestsByName.Add(key, request);
        }
        else
        {
          Log.Warn("Sitecore.RestSharp: RequestProvider.RequestsByName already contains key:"+ key, this);
        }
      }

      if (request.RequestType != typeof(RestEmptyType) || request.ResponseType != typeof(RestEmptyType))
      {
        var key = Tuple.Create(request.ActionType, request.RequestType, request.ResponseType);

        if (!this.RequestsByTypes.ContainsKey(key))
        {
          this.RequestsByTypes.Add(Tuple.Create(request.ActionType, request.RequestType, request.ResponseType), request);
        }
        else
        {
          Log.Warn("Sitecore.RestSharp: RequestProvider.RequestsByTypes already contains key:" + key, this);
        }
      }
    }
  }
}