using System;
using System.Collections.Generic;
using System.Text;
using ExamReg.Apps.Repositories.Models;

namespace ExamReg.DataInit.Resources
{
    public class ExamRoomInit : CommonInit
    {
        public List<string> ExamRoomCodes { get; private set; }
        public ExamRoomInit(ExamRegContext examRegContext) : base(examRegContext)
        {
        }

        public List<string> Init()
        {
            List<string> returnList = new List<string>
            {
                "G2_101",
                "G2_103",
                "G2_107",
                "G2_201",
                "G2_202",
                "G2_203",
                "G2_204",
                "G2_205",
                "G2_206",
                "G2_207",
                "G2_208",
                "G2_209",
                "G2_210",
                "G2_211",
                "G2_212",
                "G2_213",
                "G2_301",
                "G2_302",
                "G2_303",
                "G2_304",
                "G2_305",
                "G2_306",
                "G2_307",
                "G2_308",
                "G2_309",
                "GD2_301",
                "GD2_302",
                "GD2_303",
                "GD2_304",
                "GD2_305",
                "GD2_306",
                "GD2_307",
                "GD2_308",
                "GD2_309",
                "GD2_310",
                "GD2_311",
                "GD2_312",
                "GD2_313",
            };
            var rand = new Random();

            foreach (string item in returnList)
            {
                string[] examParts = item.Split("_");
                examRegContext.ExamRoom.Add(new ExamRoomDAO
                {
                    Id = CreateGuid(item),
                    RoomNumber = Convert.ToInt16(examParts[1]),
                    AmphitheaterName = examParts[0],
                    ComputerNumber = 5 + rand.Next(1, 5)
                });
            }

            ExamRoomCodes.AddRange(returnList);
            return returnList;
        }
    }
}
