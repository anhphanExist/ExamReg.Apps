using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class ExamProgram : DataEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
        public bool IsCurrent { get; set; }
    }
    public class ExamProgramFilter : FilterEntity
    {
        public StringFilter Name { get; set; }
        public StringFilter SemesterCode { get; set; }
        public bool? IsCurrent { get; set; }
        public ExamProgramOrder OrderBy { get; set; }
        public ExamProgramFilter() : base()
        {

        }
    }
    public enum ExamProgramOrder
    {
        Name,
        SemesterCode
    }
}
