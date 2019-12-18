using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.watcher
{
    public class WatcherDTO : DataDTO
    {
        public Guid TermId { get; set; }
        public Guid ExamProgramId { get; set; }
        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public string ExamProgramName { get; set; }
        public short ExamRoomNumber { get; set; }
        public string ExamRoomAmphitheaterName { get; set; }
        public int ExamRoomComputerNumber { get; set; }
        public int CurrentNumberOfStudentRegistered { get; set; }
        public string ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
    }
}