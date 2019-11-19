using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamProgramDAO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SemesterId { get; set; }
        public long CX { get; set; }
    }
}
