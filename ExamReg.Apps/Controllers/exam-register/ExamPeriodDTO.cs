using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class ExamPeriodDTO : DataDTO
    {
        public Guid Id { get; set; }
        public Guid ExamProgramId { get; set; }
        public Guid TermId { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
    }
}