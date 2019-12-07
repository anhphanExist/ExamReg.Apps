using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRegisterDAO
    {
        public Guid StudentId { get; set; }
        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }

        public virtual ExamRoomExamPeriodDAO Exam { get; set; }
        public virtual StudentDAO Student { get; set; }
    }
}
