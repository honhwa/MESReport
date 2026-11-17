using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Innolux.Core.Extensions;
using Innolux.Core.Extensions.ApiModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Innolux.Api.Filter
{
    /// <summary>
    /// 繁體轉化為簡體
    /// </summary>
    public class TransSimplified:ActionFilterAttribute
    {
      

        /// <summary>
        ///result之後執行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Exception == null)
                {
                 

                  var ignore=  context.ActionDescriptor.FilterDescriptors
                        .Select(f => f.Filter)
                        .OfType<TypeFilterAttribute>()
                        .Any(f => f.ImplementationType.Equals(typeof(IgonreGlobalActionFilter)));

                    if (ignore)
                    {
                 
                        return;
                    }

                    base.OnActionExecuted(context);

                    var result = (Microsoft.AspNetCore.Mvc.ObjectResult)context.Result;

                    var setting = new JsonSerializerSettings
                    {
                        // 设置为驼峰命名 (首字母小寫)
                        ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
                    };
                    string value = JsonConvert.SerializeObject(result.Value, Formatting.None, setting);
                    //繁體轉監聽并且轉化為json
                    JObject reslut = JObject.Parse(ChineseStringUtility.ToSimplified(value));
                    context.Result = new JsonResult(reslut);
                }
            }
            catch (Exception e)
            {
              
            }

          


        }
    }
}
