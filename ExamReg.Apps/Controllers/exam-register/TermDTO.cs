using ExamReg.Apps.Common;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class TermDTO : DataDTO
    {
        public string SubjectName { get; set; }
        public string SemesterCode { get; set; }
        public List<ExamPeriodDTO> ExamPeriods { get; set; }
        public bool IsQualified { get; set; }
    }
}