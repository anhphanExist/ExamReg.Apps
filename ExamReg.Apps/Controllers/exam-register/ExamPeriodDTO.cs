using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class ExamPeriodDTO : DataDTO
    {
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
    }
}