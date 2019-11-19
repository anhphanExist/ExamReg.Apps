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
    }
    public class ExamProgramFilter : FilterEntity
    {
        public GuidFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public ExamProgramOrder OrderBy { get; set; }
        public ExamProgramFilter() : base()
        {

        }
    }
    public enum ExamProgramOrder
    {
        Id,
        Name
    }
}
