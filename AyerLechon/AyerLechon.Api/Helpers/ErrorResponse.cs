using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.ModelBinding;

namespace AyerLechon.Api.Helpers
{
    public static class ModelStateHelper
    {
        public static IEnumerable<string> ToErrorResponse(this ModelStateDictionary model)
        {
            return model.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage);
        }
    }
}