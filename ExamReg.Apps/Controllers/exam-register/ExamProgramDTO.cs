using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class ExamProgramDTO
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public Guid SemesterId { get; internal set; }
        public string SemesterCode { get; internal set; }
        public bool IsCurrent { get; internal set; }
        public List<string> Errors { get; internal set; }
    }
}