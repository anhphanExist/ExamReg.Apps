using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;

namespace ExamReg.Apps.Controllers.student
{
    public class Route : Root
    {
        private const string Default = Base + "/student";
    }

    [Authorize(Policy = "CanManage")]
    public class StudentController : ApiController
    {
        public StudentController(ICurrentContext CurrentContext) : base(CurrentContext)
        {
        }
    }
}