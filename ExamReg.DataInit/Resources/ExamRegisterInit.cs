using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamRegisterInit
    {
        private ExamRegContext examRegContext;

        public ExamRegisterInit(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
