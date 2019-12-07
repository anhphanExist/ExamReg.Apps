using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamProgramInit
    {
        private ExamRegContext examRegContext;

        public ExamProgramInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
