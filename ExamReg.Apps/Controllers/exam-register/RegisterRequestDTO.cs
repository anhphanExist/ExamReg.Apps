using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class RegisterRequestDTO : DataDTO
    {
        public Guid TermId { get; set; }
        public Guid ExamPeriodId { get; set; }
    }
}