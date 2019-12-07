using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class SemesterInit
    {
        private ExamRegContext examRegContext;

        public SemesterInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
