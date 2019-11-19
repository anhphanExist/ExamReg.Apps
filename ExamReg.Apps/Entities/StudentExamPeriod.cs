using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class StudentExamPeriod : DataEntity
    {
        public Guid StudentId { get; set; }
        public Guid ExamPeriodId { get; set; }
    }
}
