using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class TermInit
    {
        private ExamRegContext examRegContext;

        public TermInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
