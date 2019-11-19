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
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public Guid TermId { get; set; }
        public string SubjectName { get; set; }
        public bool IsQualified { get; set; }
    }
    public class StudentTermFilter : FilterEntity
    {
        public int StudentNumber { get; set; }
        public string LastName { get; set; }
        public string GivenName { get; set; }
        public string SubjectName { get; set; }
        public StudentTermFilter() : base()
        {

        }
    }
}
