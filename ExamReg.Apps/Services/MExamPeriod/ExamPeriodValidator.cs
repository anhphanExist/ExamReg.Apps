using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamPeriod
{
    public interface IExamPeriodValidator : IServiceScoped
    {
        Task<bool> Update(ExamPeriod examPeriod);
    }
    public class ExamPeriodValidator : IExamPeriodValidator
    {
        public async Task<bool> Update(ExamPeriod examPeriod)
        {
            bool isValid = true;
            return isValid;
        }
    }
}
