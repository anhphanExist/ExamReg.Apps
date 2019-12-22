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
            StudentTermCodes = new List<string>();
        }

        public List<string> Init(List<string> studentIds, List<string> termIds)
        {
            List<string> returnList = new List<string>();
            var rand = new Random();

            // Lấy 1/5 số student cho thi 5 môn đầu tiên
            // Lấy 1/5 student cho thi 5 môn tiếp theo
            // Lấy 1/5 student cho thi 5 môn tiếp theo
            // Lấy 1/5 student cho thi 5 môn tiếp theo
            // Lấy 1/5 student cho thi 6 môn cuối cùng
            int studentBlock = studentIds.Count / 5;
            int termBlock = termIds.Count / 5;
            for (int i = 0; i < studentIds.Count; i++)
            {
                if (i <= studentBlock)
                {
                    for (int j = 0; j <= termBlock; j++)
                    {
                        returnList.Add(AddStudentTerm(termIds[j], studentIds[i], rand));
                    }
                }
                else if (i <= studentBlock * 2)
                {
                    for (int j = termBlock + 1; j <= termBlock * 2; j++)
                    {
                        returnList.Add(AddStudentTerm(termIds[j], studentIds[i], rand));
                    }
                }
                else if (i <= studentBlock * 3)
                {
                    for (int j = termBlock * 2 + 1; j <= termBlock * 3; j++)
                    {
                        returnList.Add(AddStudentTerm(termIds[j], studentIds[i], rand));
                    }
                }
                else if (i <= studentBlock * 4)
                {
                    for (int j = termBlock * 3 + 1; j <= termBlock * 4; j++)
                    {
                        returnList.Add(AddStudentTerm(termIds[j], studentIds[i], rand));
                    }
                }
                else
                {
                    for (int j = termBlock * 4 + 1; j < termIds.Count; j++)
                    {
                        returnList.Add(AddStudentTerm(termIds[j], studentIds[i], rand));
                    }
                }
                
            }

            StudentTermCodes.AddRange(returnList);
            return returnList;
        }

        private string AddStudentTerm(string termId, string studentId, Random rand)
        {
            examRegContext.StudentTerm.Add(new StudentTermDAO
            {
                TermId = CreateGuid(termId),
                StudentId = CreateGuid(studentId),
                IsQualified = rand.NextDouble() >= 0.5
            });
            return string.Format(studentId + "|" + termId);
        }
    }
}
