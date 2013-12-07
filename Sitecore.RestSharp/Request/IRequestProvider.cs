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
