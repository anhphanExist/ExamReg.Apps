using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class SemesterInit : CommonInit
    {
        public List<string> SemesterCodes { get; private set; }
        public SemesterInit(ExamRegContext examRegContext) : base(examRegContext)
        {
            SemesterCodes = new List<string>();
        }

        public List<string> Init()
        {
            List<string> returnList = new List<string>
            {
                "2017_2018_1",
                "2017_2018_2",
                "2018_2019_1",
                "2018_2019_2",
                "2019_2020_1",
                "2019_2020_2"
            };
            var rand = new Random();

            for (int i = 0; i < returnList.Count; i++)
            {
                // tách semesterCode trong returnList thành thông tin để gán vào db
                string[] parts = returnList[i].Split("_");
                examRegContext.Semester.Add(new SemesterDAO
                {
                    Id = CreateGuid(returnList[i]),
                    StartYear = short.Parse(parts[0]),
                    EndYear = short.Parse(parts[1]),
                    IsFirstHalf = parts[2].Equals("1")
                });
            }
            SemesterCodes.AddRange(returnList);
            return returnList;
        }
    }
}
