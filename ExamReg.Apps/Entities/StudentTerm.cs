using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Entities
{
    public class StudentTerm : DataEntity
    {
        public Guid StudentId { get; set; }
        public Guid TermId { get; set; }
        public bool IsQualified { get; set; }
    }
    public class StudentTermFilter : FilterEntity
    {
        public GuidFilter StudentId { get; set; }
        public GuidFilter TermId { get; set; }
        public bool IsQualified { get; set; }
        public StudentTermFilter() : base()
        {

        }
    }
}
