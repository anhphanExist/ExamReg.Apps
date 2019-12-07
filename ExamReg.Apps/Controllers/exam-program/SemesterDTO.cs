using ExamReg.Apps.Common;
using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Controllers.exam_program
{
    public class SemesterDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public short StartYear { get; set; }
        public short EndYear { get; set; }
        public bool IsFirstHalf { get; set; }
    }
}