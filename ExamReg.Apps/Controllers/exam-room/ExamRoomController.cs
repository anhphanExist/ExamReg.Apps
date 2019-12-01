using ExamReg.Apps.Common;
using Microsoft.AspNetCore.Authorization;

namespace ExamReg.Apps.Controllers.exam_room
{
    public class ExamRoomRoute : Root
    {
        private const string Default = Base + "/exam-room";
    }

    [Authorize(Policy = "CanManage")]
    public class ExamRoomController : ApiController
    {
        public ExamRoomController(ICurrentContext CurrentContext) : base(CurrentContext)
        {
        }
    }
}