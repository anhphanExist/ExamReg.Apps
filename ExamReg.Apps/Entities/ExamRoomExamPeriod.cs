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
}
