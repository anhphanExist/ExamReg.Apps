using ExamReg.Apps.Common;
using System;

namespace ExamReg.Apps.Controllers.watcher
{
    public class ExamProgramDTO : DataDTO
    {
        public Guid SemesterId { get; set; }
        public string Name { get; set; }
        public Guid Id { get; set; }
        public string SemesterCode { get; set; }
        public bool IsCurrent { get; set; }
    }
}