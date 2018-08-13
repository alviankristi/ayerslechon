using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AyerLechon.Api.Controllers
{
    public class ResponseViewModel<T>
    {
        public Status Status { get; set; }
        public T Data { get; set; }
    }

    public class Status
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public IEnumerable<string> FieldMessages { get; set; }
    }
}