using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class StudentExamPeriod : DataEntity
    {
        public Guid StudentId { get; set; }
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public Guid ExamPeriodId { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
    }
    public class StudentExamPeriodFilter : FilterEntity
    {
        public IntFilter StudentNumber { get; set; }
        public StringFilter LastName { get; set; }
        public StringFilter GivenName { get; set; }
        public DateTimeFilter ExamDate { get; set; }
        public ShortFilter StartHour { get; set; }
        public ShortFilter FinishHour { get; set; }
        public StudentExamPeriodOrder OrderBy { get; set; }
        public StudentExamPeriodFilter() : base() { }
    }
    public enum StudentExamPeriodOrder
    {
        StudentNumber,
        LastName,
        GivenName,
        ExamDate,
        StartHour,
        FinishHour
    }
}
