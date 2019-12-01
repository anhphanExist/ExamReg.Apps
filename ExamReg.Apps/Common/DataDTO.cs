using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Common
{
    public class DataDTO
    {
        public List<string> Errors { get; set; }
    }

    public class FilterDTO
    {
        public int Skip { get; set; }
        public int Take { get; set; }
        public OrderType OrderType { get; set; }

        public FilterDTO()
        {
            Skip = 0;
            Take = Int32.MaxValue;
            OrderType = OrderType.ASC;
        }
    }
}
