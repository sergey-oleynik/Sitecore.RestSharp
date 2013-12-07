﻿/*
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

namespace Sitecore.RestSharp.Parameters
{
  using System.Collections.Generic;
  using System.Linq;

  using global::RestSharp;

  public abstract class ParameterReplacerBase : IParameterReplacer
  {
    public abstract void ReplaceParameters(IRestRequest request);

    [CanBeNull]
    protected virtual object GetValue(Parameter parameter)
    {
      return parameter != null ? parameter.Value : null;
    }

    [NotNull]
    protected virtual IEnumerable<Parameter> GetParameters(IRestRequest request)
    {
      return request.Parameters;
    }

    [CanBeNull]
    protected virtual Parameter GetParameter(IRestRequest request, string name)
    {
      return request.Parameters.FirstOrDefault(p => p.Name == name);
    }

    protected virtual void SetParameter(IRestRequest request, Parameter parameter)
    {
      object value = this.GetValue(parameter);
      if (value != null)
      {
        request.Resource = request.Resource.Replace('{' + parameter.Name + '}', value.ToString());
      }
    }
  }
}