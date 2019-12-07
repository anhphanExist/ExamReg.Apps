using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamRegister : DataEntity
    {
        public Guid StudentId { get; set; }
        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public Guid ExamProgramId { get; set; }
        public int StudentNumber { get; set; }
        public short ExamRoomNumber { get; set; }
        public string ExamRoomAmphitheaterName { get; set; }
        public int ExamRoomComputerNumber { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
    }

    public class ExamRegisterFilter : FilterEntity
    {
        public GuidFilter StudentId { get; set; }
        public GuidFilter ExamPeriodId { get; set; }
        public GuidFilter TermId { get; set; }
    }
}
