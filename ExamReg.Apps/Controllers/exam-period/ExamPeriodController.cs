using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;

namespace ExamReg.Apps.Controllers.exam_period
{
    public class Route : Root
    {
        private const string Default = Base + "/exam-period";
    }

    [Authorize(Policy = "CanManage")]
    public class ExamPeriodController : ApiController
    {
        public ExamPeriodController(ICurrentContext CurrentContext) : base(CurrentContext)
        {
        }
    }
}