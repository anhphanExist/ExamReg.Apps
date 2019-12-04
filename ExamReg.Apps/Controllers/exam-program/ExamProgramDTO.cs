using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_program
{
    public class ExamProgramDTO : DataDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}