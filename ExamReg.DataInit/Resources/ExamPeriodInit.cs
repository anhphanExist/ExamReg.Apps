using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamPeriodInit : CommonInit
    {
        public List<string> ExamPeriodCodes { get; private set; }
        public ExamPeriodInit(ExamRegContext examRegContext) : base(examRegContext)
        {
        }

        public List<string> Init(List<string> termIds, List<string> examProgramIds)
        {
            List<string> returnList = new List<string>();
            var rand = new Random();

            foreach (string examProgramId in examProgramIds)
            {
                foreach (string termId in termIds)
                {
                    int startHour = rand.Next(7, 18);
                    examRegContext.ExamPeriod.Add(new ExamPeriodDAO
                    {
                        Id = CreateGuid(string.Format(termId + "|" + examProgramId)),
                        ExamDate = new DateTime(2019, 12, 24),
                        ExamProgramId = CreateGuid(examProgramId),
                        TermId = CreateGuid(termId),
                        StartHour = Convert.ToInt16(startHour),
                        FinishHour = Convert.ToInt16(startHour + 2),
                    });
                    returnList.Add(string.Format(termId + "|" + examProgramId));
                }
            }
            ExamPeriodCodes.AddRange(returnList);
            return returnList;
        }
    }
}
