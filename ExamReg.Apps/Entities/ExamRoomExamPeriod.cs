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
        public Guid ExamProgramId { get; set; }
        public Guid TermId { get; set; }
        public short ExamRoomNumber { get; set; }
        public string ExamRoomAmphitheaterName { get; set; }
        public int ExamRoomComputerNumber { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public string SubjectName { get; set; }
        public string ExamProgramName { get; set; }
        public List<Student> Students { get; set; }
        public ExamRoomExamPeriod()
        {
            Students = new List<Student>();
        }
    }
    public class ExamRoomExamPeriodFilter : FilterEntity
    {
        public GuidFilter StudentId { get; set; }
        public IntFilter StudentNumber { get; set; }
        public GuidFilter TermId { get; set; }
        public GuidFilter ExamProgramId { get; set; }
        public GuidFilter ExamPeriodId { get; set; }
        public GuidFilter ExamRoomId { get; set; }
        public ExamOrder OrderBy { get; set; }
        public ExamRoomExamPeriodFilter() : base() { }
    }

    public enum ExamOrder
    {
        SubjectName,
        ExamDate,
        StartHour,
        ExamProgramName
    }
}
