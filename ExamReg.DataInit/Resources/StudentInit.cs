using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class StudentInit
    {
        private ExamRegContext examRegContext;

        public StudentInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
