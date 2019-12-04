using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamPeriodDAO
    {
        public ExamPeriodDAO()
        {
            ExamRoomExamPeriods = new HashSet<ExamRoomExamPeriodDAO>();
        }

        public Guid Id { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public Guid TermId { get; set; }
        public Guid ExamProgramId { get; set; }
        public long CX { get; set; }

        public virtual ExamProgramDAO ExamProgram { get; set; }
        public virtual TermDAO Term { get; set; }
        public virtual ICollection<ExamRoomExamPeriodDAO> ExamRoomExamPeriods { get; set; }
    }
}
