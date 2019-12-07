using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class TermDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string SubjectName { get; set; }
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
        public List<ExamPeriodDTO> ExamPeriods { get; set; }
        public bool IsQualified { get; set; }
    }

    public class TermFilterDTO : FilterDTO
    {
        public Guid SemesterId { get; set; }
        public string SemesterCode { get; set; }
    }
}