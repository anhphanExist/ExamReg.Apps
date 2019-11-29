using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;

namespace ExamReg.Apps.Controllers.term
{
    public class Route : Root
    {
        private const string Default = Base + "/term";
    }

    [Authorize(Policy = "CanManage")]
    public class TermController : ApiController
    {
        public TermController(ICurrentContext CurrentContext) : base(CurrentContext)
        {
        }
    }
}