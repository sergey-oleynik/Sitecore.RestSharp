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

namespace Sitecore.RestSharp.Service
{
  using System.Collections.Generic;
  
  using global::RestSharp;
  using global::RestSharp.Deserializers;
  using global::RestSharp.Serializers;

  using Sitecore.RestSharp.Parameters;
  using Sitecore.RestSharp.Tokens;
  using Sitecore.RestSharp.Request;

  public interface IServiceConfiguration
  {
    IRequestProvider RequestProvider { get; }

    string BaseUrl { get; }

    List<ITokenReplacer> TokenReplacers { get; }

    List<IParameterReplacer> ParameterReplacers { get; }

    ISerializer XmlSerializer { get; }

    ISerializer JsonSerializer { get; }

    IAuthenticator Authenticator { get; }

    Dictionary<string, IDeserializer> Handlers { get; }

    Dictionary<string, string> Headers { get; }
  }
}