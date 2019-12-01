using ExamReg.Apps.Common;

namespace ExamReg.Apps.Controllers.term
{
    public class TermDTO : DataDTO
    {
        public string SubjectName { get; set; }
        public string SemesterCode { get; set; }
    }
}