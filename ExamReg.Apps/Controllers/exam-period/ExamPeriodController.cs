﻿using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamPeriod;
using ExamReg.Apps.Services.MExamProgram;
using ExamReg.Apps.Services.MExamRoom;
using ExamReg.Apps.Services.MTerm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Controllers.exam_period
{
    public class ExamPeriodRoute : Root
    {
        private const string Default = Base + "/exam-period";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string GetCurrentExamProgram = Default + "/get-current-exam-program";
        public const string ListTerm = Default + "/list-term";
        public const string ListAvailableExamRoom = Default + "/list-available-exam-room";
    }

    [Authorize(Policy = "CanManage")]
    public class ExamPeriodController : ApiController
    {
        private IExamPeriodService ExamPeriodService;
        private IExamProgramService ExamProgramService;
        private ITermService TermService;
        private IExamRoomService ExamRoomService;
        public ExamPeriodController(ICurrentContext CurrentContext,
            IExamPeriodService ExamPeriodService,
            IExamProgramService ExamProgramService,
            ITermService TermService,
            IExamRoomService ExamRoomService
            ) : base(CurrentContext)
        {
            this.ExamPeriodService = ExamPeriodService;
            this.ExamProgramService = ExamProgramService;
            this.ExamRoomService = ExamRoomService;
            this.TermService = TermService;
        }

        [Route(ExamPeriodRoute.List), HttpPost]
        public async Task<List<ExamPeriodDTO>> List()
        {
            List<ExamPeriod> examPeriods = await ExamPeriodService.List(new ExamPeriodFilter
            {
                OrderBy = ExamPeriodOrder.SubjectName,
                OrderType = OrderType.ASC
            });
            return examPeriods.Select(e => new ExamPeriodDTO
            {
                Id = e.Id,
                ExamDate = e.ExamDate,
                StartHour = e.StartHour,
                FinishHour = e.FinishHour,
                SubjectName = e.SubjectName,
                ExamProgramId = e.ExamProgramId,
                ExamProgramName = e.ExamProgramName,
                Errors = e.Errors
            }).ToList();
        }

        [Route(ExamPeriodRoute.Create), HttpPost]
        public async Task<ExamPeriodDTO> Create([FromBody] ExamPeriodDTO examPeriodRequestDTO)
        {
            ExamPeriod newExamPeriod = new ExamPeriod
            {
                ExamDate = examPeriodRequestDTO.ExamDate,
                StartHour = examPeriodRequestDTO.StartHour,
                FinishHour = examPeriodRequestDTO.FinishHour,
                SubjectName = examPeriodRequestDTO.SubjectName,
                ExamProgramName = examPeriodRequestDTO.ExamProgramName,
            };
            ExamPeriod res = await ExamPeriodService.Create(newExamPeriod);
            return new ExamPeriodDTO
            {
                Id = res.Id,
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                ExamProgramId = res.ExamProgramId,
                ExamProgramName = res.ExamProgramName,
                Errors = res.Errors
            };
        }

        [Route(ExamPeriodRoute.Update), HttpPost]
        public async Task<ExamPeriodDTO> Update([FromBody] ExamPeriodDTO examPeriodRequestDTO)
        {
            ExamPeriod examPeriod = new ExamPeriod
            {
                Id = examPeriodRequestDTO.Id,
                
            };
            ExamPeriod res = await ExamPeriodService.Update(examPeriod);
            return new ExamPeriodDTO
            {
                Id = res.Id,
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                ExamProgramId = res.ExamProgramId,
                ExamProgramName = res.ExamProgramName,
                Errors = res.Errors
            };
        }

        [Route(ExamPeriodRoute.Delete), HttpPost]
        public async Task<ExamPeriodDTO> Delete([FromBody] ExamPeriodDTO examPeriodRequestDTO)
        {
            ExamPeriod examPeriod = new ExamPeriod
            {
                Id = examPeriodRequestDTO.Id,
                ExamDate = examPeriodRequestDTO.ExamDate,
                StartHour = examPeriodRequestDTO.StartHour,
                FinishHour = examPeriodRequestDTO.FinishHour,
                SubjectName = examPeriodRequestDTO.SubjectName,
                ExamProgramName = examPeriodRequestDTO.ExamProgramName,
            };
            ExamPeriod res = await ExamPeriodService.Delete(examPeriod);
            return new ExamPeriodDTO
            {
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                ExamProgramName = res.ExamProgramName,
                Errors = res.Errors
            };
        }

        [Route(ExamPeriodRoute.GetCurrentExamProgram), HttpPost]
        public async Task<ExamProgramDTO> GetCurrentExamProgram()
        {
            ExamProgram res = await ExamProgramService.GetCurrentExamProgram();
            return new ExamProgramDTO
            {
                Id = res.Id,
                Name = res.Name,
                SemesterId = res.SemesterId,
                SemesterCode = res.SemesterCode,
                IsCurrent = res.IsCurrent,
                Errors = res.Errors
            };
        }

        [Route(ExamPeriodRoute.ListTerm), HttpPost]
        public async Task<List<TermDTO>> ListTerm([FromBody] TermFilterDTO termRequestFilterDTO)
        {
            List<Term> terms = await TermService.List(new TermFilter
            {
                SemesterId = new GuidFilter { Equal = termRequestFilterDTO.SemesterId },
                SemesterCode = new StringFilter { Equal = termRequestFilterDTO.SemesterCode },
                OrderBy = TermOrder.SubjectName,
                OrderType = OrderType.ASC
            });
            return terms.Select(t => new TermDTO
            {
                Id = t.Id,
                SubjectName = t.SubjectName,
                SemesterId = t.SemesterId,
                SemesterCode = t.SemesterCode,
                Errors = t.Errors
            }).ToList();
        }

        [Route(ExamPeriodRoute.ListAvailableExamRoom), HttpPost]
        public async Task<List<ExamRoomDTO>> ListAvailableExamRoom([FromBody] ExamRoomFilterDTO examRoomRequestFilterDTO)
        {
            List<ExamRoom> examRooms = await ExamRoomService.ListAvailableExamRoom(new ExamRoomFilter
            {
                ExamDate = new DateTimeFilter { Equal = examRoomRequestFilterDTO.ExamDate },
                StartHour = new ShortFilter { Equal = examRoomRequestFilterDTO.StartHour },
                FinishHour = new ShortFilter { Equal = examRoomRequestFilterDTO.FinishHour },
                OrderBy = ExamRoomOrder.AmphitheaterName,
                OrderType = OrderType.ASC
            });
            return examRooms.Select(s => new ExamRoomDTO
            {
                Id = s.Id,
                Code = s.Code,
                RoomNumber = s.RoomNumber,
                AmphitheaterName = s.AmphitheaterName,
                ComputerNumber = s.ComputerNumber,
                Errors = s.Errors
            }).ToList();
        }
    }
}