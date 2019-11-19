using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRoomExamPeriod
    {
        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public long CX { get; set; }
    }
}
