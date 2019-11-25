using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamRoomExamPeriod : DataEntity
    {
        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
    }
    public class ExamRoomExamPeriodFilter : FilterEntity
    {
        public GuidFilter ExamRoomId { get; set; }
        public GuidFilter ExamPeriodId { get; set; }
        public ExamRoomExamPeriodFilter() : base() { }
    }
}
