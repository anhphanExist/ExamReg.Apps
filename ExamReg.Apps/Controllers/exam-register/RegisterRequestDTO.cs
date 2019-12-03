using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class RegisterRequestDTO : DataDTO
    {
        public List<Guid> ExamPeriodIds { get; set; }
    }
}