using System;
using System.Collections.Generic;

namespace ExamReg.Apps.Repositories.Models
{
    public partial class ExamProgram
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid SemesterId { get; set; }
        public long CX { get; set; }
    }
}
