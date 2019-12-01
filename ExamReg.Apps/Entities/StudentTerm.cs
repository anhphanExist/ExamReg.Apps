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
        public int StudentNumber { get; set; }
        public string StudentLastName { get; set; }
        public string StudentGivenName { get; set; }
        public string SubjectName { get; set; }
        public bool IsQualified { get; set; }
    }
    public class StudentTermFilter : FilterEntity
    {
        public GuidFilter StudentId { get; set; }
        public GuidFilter TermId { get; set; }
        public StringFilter StudentNumber { get; set; }
        public StringFilter SubjectName { get; set; }
        public StringFilter StudentLastName { get; set; }
        public StringFilter StudentGivenName { get; set; }
        public StudentTermOrder OrderBy { get; set; }
        public bool? IsQualified { get; set; }
        public StudentTermFilter() : base()
        {

        }
    }
    public enum StudentTermOrder
    {
        StudentNumber,
        SubjectName,
        StudentLastName,
        StudentGivenName,
        IsQualified
    }
}
