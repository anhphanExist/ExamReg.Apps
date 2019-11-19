using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamPeriod : DataEntity
    {
        public Guid Id { get; set; }
        public DateTime ExamDate { get; set; }
        public short StartHour { get; set; }
        public short FinishHour { get; set; }
        public Guid TermId { get; set; }
        public string SubjectName { get; set; }
        public Guid ExamProgramId { get; set; }
        public string Name { get; set; }
    }
    public class ExamPeriodFilter : FilterEntity
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public string Name { get; set; }
        public ExamPeriodOrder OrderBy { get; set; }
        public ExamPeriodFilter() : base()
        {

        }
    }
    public enum ExamPeriodOrder
    {
        Id
    }
}
