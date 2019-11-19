using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamPeriodDAO
    {
        public Guid Id { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public Guid TermId { get; set; }
        public Guid ExamProgramId { get; set; }
        public long CX { get; set; }
    }
}
