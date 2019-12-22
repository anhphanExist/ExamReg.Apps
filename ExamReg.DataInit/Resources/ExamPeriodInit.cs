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
            ExamPeriodCodes = new List<string>();
        }

        public List<string> Init(List<string> termIds, List<string> examProgramIds)
        {
            List<string> returnList = new List<string>();
            var rand = new Random();

            foreach (string examProgramId in examProgramIds)
            {
                // Lấy năm học của examProgram để gán vào examDate
                // Chia examDate ra tháng 12 và tháng 6
                // Init ra 2 ca thi cho mỗi môn thi
                string[] chunks = examProgramId.Split(":")[1].Split("_");
                int examYear = 1900;
                int startHour = rand.Next(7, 18);

                if (chunks[2].Equals("1"))
                {
                    examYear = Int32.TryParse(chunks[0], out int i) ? i : 1900;
                    foreach (string termId in termIds)
                    {
                        examRegContext.ExamPeriod.Add(new ExamPeriodDAO
                        {
                            Id = CreateGuid(string.Format(termId + "|" + examProgramId)),
                            ExamDate = new DateTime(examYear, 12, rand.Next(15, 31)),
                            ExamProgramId = CreateGuid(examProgramId),
                            TermId = CreateGuid(termId),
                            StartHour = Convert.ToInt16(startHour),
                            FinishHour = Convert.ToInt16(startHour + 2),
                        });
                        returnList.Add(string.Format(termId + "|" + examProgramId));

                        examRegContext.ExamPeriod.Add(new ExamPeriodDAO
                        {
                            Id = CreateGuid(string.Format(termId + "|" + examProgramId + "|" + "2")),
                            ExamDate = new DateTime(examYear, 12, rand.Next(15, 31)),
                            ExamProgramId = CreateGuid(examProgramId),
                            TermId = CreateGuid(termId),
                            StartHour = Convert.ToInt16(startHour + 4),
                            FinishHour = Convert.ToInt16(startHour + 6)
                        });
                        returnList.Add(string.Format(termId + "|" + examProgramId + "|" + "2"));
                    }
                }
                else
                {
                    examYear = Int32.TryParse(chunks[1], out int i) ? i : 1900;
                    foreach (string termId in termIds)
                    {
                        examRegContext.ExamPeriod.Add(new ExamPeriodDAO
                        {
                            Id = CreateGuid(string.Format(termId + "|" + examProgramId)),
                            ExamDate = new DateTime(examYear, 6, rand.Next(15, 31)),
                            ExamProgramId = CreateGuid(examProgramId),
                            TermId = CreateGuid(termId),
                            StartHour = Convert.ToInt16(startHour),
                            FinishHour = Convert.ToInt16(startHour + 2),
                        });
                        returnList.Add(string.Format(termId + "|" + examProgramId));

                        examRegContext.ExamPeriod.Add(new ExamPeriodDAO
                        {
                            Id = CreateGuid(string.Format(termId + "|" + examProgramId + "|" + "2")),
                            ExamDate = new DateTime(examYear, 6, rand.Next(15, 31)),
                            ExamProgramId = CreateGuid(examProgramId),
                            TermId = CreateGuid(termId),
                            StartHour = Convert.ToInt16(startHour + 4),
                            FinishHour = Convert.ToInt16(startHour + 6)
                        });
                        returnList.Add(string.Format(termId + "|" + examProgramId + "|" + "2"));
                    }
                }
                
            }
            ExamPeriodCodes.AddRange(returnList);
            return returnList;
        }
    }
}
