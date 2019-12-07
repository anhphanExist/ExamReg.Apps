using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class ExamProgramDTO : DataDTO
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public Guid SemesterId { get; internal set; }
        public string SemesterCode { get; internal set; }
        public bool IsCurrent { get; internal set; }
    }
}