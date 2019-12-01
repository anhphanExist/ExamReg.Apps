using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamRoom;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExamReg.Apps.Controllers.exam_room
{
    public class ExamRoomRoute : Root
    {
        private const string Default = Base + "/exam-room";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
    }

    [Authorize(Policy = "CanManage")]
    public class ExamRoomController : ApiController
    {
        private IExamRoomService ExamRoomService;
        public ExamRoomController(ICurrentContext CurrentContext,
            IExamRoomService ExamRoomService) : base(CurrentContext)
        {
            this.ExamRoomService = ExamRoomService;
        }

        [Route(ExamRoomRoute.List), HttpPost]
        public async Task<List<ExamRoomDTO>> List()
        {
            List<ExamRoom> examRooms = await ExamRoomService.List(new ExamRoomFilter());
            List<ExamRoomDTO> res = new List<ExamRoomDTO>();
            examRooms.ForEach(r => res.Add(new ExamRoomDTO
            {
                RoomNumber = r.RoomNumber,
                AmphitheaterName = r.AmphitheaterName,
                ComputerNumber = r.ComputerNumber,
                Errors = r.Errors
            }));
            return res;
        }

        [Route(ExamRoomRoute.Create), HttpPost]
        public async Task<ExamRoomDTO> Create([FromBody] ExamRoomDTO examRoomRequestDTO)
        {
            ExamRoom newExamRoom = new ExamRoom
            {
                RoomNumber = examRoomRequestDTO.RoomNumber,
                AmphitheaterName = examRoomRequestDTO.AmphitheaterName,
                ComputerNumber = examRoomRequestDTO.ComputerNumber
            };
            ExamRoom res = await ExamRoomService.Create(newExamRoom);
            return new ExamRoomDTO
            {
                RoomNumber = res.RoomNumber,
                AmphitheaterName = res.AmphitheaterName,
                ComputerNumber = res.ComputerNumber,
                Errors = res.Errors
            };
        }

        [Route(ExamRoomRoute.Update), HttpPost]
        public async Task<ExamRoomDTO> Update([FromBody] ExamRoomDTO examRoomRequestDTO)
        {
            ExamRoom examRoom = new ExamRoom
            {
                RoomNumber = examRoomRequestDTO.RoomNumber,
                AmphitheaterName = examRoomRequestDTO.AmphitheaterName,
                ComputerNumber = examRoomRequestDTO.ComputerNumber
            };
            ExamRoom res = await ExamRoomService.Update(examRoom);
            return new ExamRoomDTO
            {
                RoomNumber = res.RoomNumber,
                AmphitheaterName = res.AmphitheaterName,
                ComputerNumber = res.ComputerNumber,
                Errors = res.Errors
            };
        }

        [Route(ExamRoomRoute.Delete), HttpPost]
        public async Task<ExamRoomDTO> Delete([FromBody] ExamRoomDTO examRoomRequestDTO)
        {
            ExamRoom examRoom = new ExamRoom
            {
                RoomNumber = examRoomRequestDTO.RoomNumber,
                AmphitheaterName = examRoomRequestDTO.AmphitheaterName,
                ComputerNumber = examRoomRequestDTO.ComputerNumber
            };
            ExamRoom res = await ExamRoomService.Delete(examRoom);
            return new ExamRoomDTO
            {
                RoomNumber = res.RoomNumber,
                AmphitheaterName = res.AmphitheaterName,
                ComputerNumber = res.ComputerNumber,
                Errors = res.Errors
            };
        }
    }
}