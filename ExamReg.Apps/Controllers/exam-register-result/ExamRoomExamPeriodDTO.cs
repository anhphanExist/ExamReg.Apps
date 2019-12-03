using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_register_result
{
    public class ExamRoomExamPeriodDTO : DataDTO
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

    public class ExamRoomExamPeriodFilterDTO : FilterDTO
    {
        public int StudentNumber { get; set; }
        public short ExamRoomNumber { get; set; }
        public string ExamRoomAmphitheaterName { get; set; }
        public int CurrentNumberOfStudentRegistered { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
    }
}