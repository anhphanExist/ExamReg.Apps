using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class StudentExamPeriodDAO
    {
        public Guid StudentId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public long CX { get; set; }

        public virtual ExamPeriodDAO ExamPeriod { get; set; }
        public virtual StudentDAO Student { get; set; }
    }
}
