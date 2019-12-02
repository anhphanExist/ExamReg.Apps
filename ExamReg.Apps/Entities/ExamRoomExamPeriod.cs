using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamRoomExamPeriod : DataEntity
    {
        public Guid ExamRoomId { get; set; }
        public Guid ExamPeriodId { get; set; }
        public string ExamRoomNumber { get; set; }
        public string ExamRoomAmphitheaterName { get; set; }
        public int CurrentNumberOfStudentRegistered { get; set; }
        public int ExamRoomComputerNumber { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
    }
    public class ExamRoomExamPeriodFilter : FilterEntity
    {
        public StringFilter ExamRoomNumber { get; set; }
        public StringFilter ExamRoomAmphitheaterName { get; set; }
        public IntFilter CurrentNumberOfStudentRegistered { get; set; }
        public DateTimeFilter ExamDate { get; set; }
        public ShortFilter StartHour { get; set; }
        public ShortFilter FinishHour { get; set; }
        public StringFilter SubjectName { get; set; }
        public StringFilter ExamProgramName { get; set; }
        public ExamRoomExamPeriodFilter() : base() { }
    }
}
