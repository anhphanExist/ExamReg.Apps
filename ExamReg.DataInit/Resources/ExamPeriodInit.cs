using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamPeriodInit
    {
        private ExamRegContext examRegContext;

        public ExamPeriodInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
