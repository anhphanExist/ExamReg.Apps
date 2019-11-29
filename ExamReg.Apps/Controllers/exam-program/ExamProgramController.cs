using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;

namespace ExamReg.Apps.Controllers.exam_program
{
    public class Route : Root
    {
        private const string Default = Base + "/exam-program";
    }

    [Authorize(Policy = "CanManage")]
    public class ExamProgramController : ApiController
    {
        public ExamProgramController(ICurrentContext CurrentContext) : base(CurrentContext)
        {
        }
    }
}