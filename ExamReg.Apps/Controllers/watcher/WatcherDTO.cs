using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.watcher
{
    public class WatcherDTO : DataDTO
    {
        public short ExamRoomNumber { get; set; }
        public string ExamRoomAmphitheaterName { get; set; }
        public int CurrentNumberOfStudentRegistered { get; set; }
        public int ExamRoomComputerNumber { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
    }
}