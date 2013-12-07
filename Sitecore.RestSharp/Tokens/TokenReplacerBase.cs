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

namespace Sitecore.RestSharp.Tokens
{
  using Sitecore.Diagnostics;

  using global::RestSharp;

  public abstract class TokenReplacerBase : ITokenReplacer
  {
    private readonly string token;

    public string Token
    {
      get { return token; }
    }

    protected TokenReplacerBase(string token)
    {
      Assert.ArgumentNotNullOrEmpty(token, "token");

      this.token = token;
    }
    
    public virtual void ReplaceToken(IRestRequest request)
    {
      request.AddUrlSegment(this.Token, this.GetValue(request));
    }

    protected abstract string GetValue(IRestRequest request);
  }
}