using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamRoomExamPeriodDAO
    {
        public ExamRoomExamPeriodDAO()
        {
            ExamRegisters = new HashSet<ExamRegisterDAO>();
        }

        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public long CX { get; set; }

        public virtual ExamPeriodDAO ExamPeriod { get; set; }
        public virtual ExamRoomDAO ExamRoom { get; set; }
        public virtual ICollection<ExamRegisterDAO> ExamRegisters { get; set; }
    }
}
