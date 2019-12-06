using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.exam_program
{
    public class ExamProgramDTO : DataDTO
    {
        public Guid Id { get; set; }
        public Guid SemesterId { get; set; }
        public string Name { get; set; }
        public string SemesterCode { get; set; }
        public bool IsCurrent { get; set; }
    }
}