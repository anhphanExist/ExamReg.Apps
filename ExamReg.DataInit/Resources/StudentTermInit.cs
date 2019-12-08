using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class StudentTermInit : CommonInit
    {
        public List<string> StudentTermCodes { get; private set; }
        public StudentTermInit(ExamRegContext examRegContext) : base(examRegContext)
        {
        }

        public List<string> Init(List<string> studentIds, List<string> termIds)
        {
            List<string> returnList = new List<string>();
            var rand = new Random();

            foreach (string studentId in studentIds)
            {
                foreach (string termId in termIds)
                {
                    examRegContext.StudentTerm.Add(new StudentTermDAO
                    {
                        TermId = CreateGuid(termId),
                        StudentId = CreateGuid(studentId),
                        IsQualified = rand.NextDouble() >= 0.5
                    });
                    returnList.Add(string.Format(studentId + "|" + termId));
                }
            }
            StudentTermCodes.AddRange(returnList);
            return returnList;
        }
    }
}
