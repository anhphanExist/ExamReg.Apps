using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Linq;
using System.Reflection;

namespace ExamReg.Apps.Common
{
    public class FilterEntity
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType OrderType { get; set; }

        public FilterEntity()
        {
            Skip = 0;
            Take = Int32.MaxValue;
            OrderType = OrderType.ASC;
        }
    }

    public enum OrderType
    {
        ASC,
        DESC
    }
}
