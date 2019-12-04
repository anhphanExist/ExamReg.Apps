﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamRoomExamPeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.watcher
{
    public class WatcherRoute : Root
    {
        private const string Default = Base + "/watcher";
        public const string List = Default + "/list";
        public const string ExportStudent = Default + "/export-student";
    }
    [Authorize(Policy = "CanManage")]
    public class WatcherController : ApiController
    {
        private IExamRoomExamPeriodService ExamRoomExamPeriodService;
        public WatcherController(ICurrentContext CurrentContext,
            IExamRoomExamPeriodService ExamRoomExamPeriodService) : base(CurrentContext)
        {
            this.ExamRoomExamPeriodService = ExamRoomExamPeriodService;
        }

        // Lấy danh sách tất cả thông tin của các ca thi ứng với phòng thi và môn thi
        [Route(WatcherRoute.List), HttpPost]
        public async Task<List<WatcherDTO>> List()
        {
            List<ExamRoomExamPeriod> examRoomExamPeriods = await ExamRoomExamPeriodService.List(new ExamRoomExamPeriodFilter());
            List<WatcherDTO> res = new List<WatcherDTO>();
            examRoomExamPeriods.ForEach(r => res.Add(new WatcherDTO
            {
                ExamPeriodId = r.ExamPeriodId,
                ExamRoomId = r.ExamRoomId,
                ExamProgramId = r.ExamProgramId,
                TermId = r.TermId,
                ExamProgramName = r.ExamProgramName,
                ExamRoomNumber = r.ExamRoomNumber,
                ExamRoomAmphitheaterName = r.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = r.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = r.Students.Count,
                ExamDate = r.ExamDate,
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                SubjectName = r.SubjectName,
                Errors = r.Errors
            }));
            return res;
        }

        // Xuất danh sách sinh viên thi trong 1 phòng thi của 1 ca thi của 1 môn thi
        [Route(WatcherRoute.ExportStudent), HttpPost]
        public async Task<FileResult> ExportStudent([FromBody] WatcherDTO watcherRequestDTO)
        {
            ExamRoomExamPeriodFilter filter = new ExamRoomExamPeriodFilter
            {
                ExamPeriodId = new GuidFilter { Equal = watcherRequestDTO.ExamPeriodId },
                ExamRoomId = new GuidFilter { Equal = watcherRequestDTO.ExamRoomId }
            };
            byte[] data = await ExamRoomExamPeriodService.ExportStudent(filter);
            return File(data, "application/octet-stream", "StudentRoomPeriod.xlsx");
        }
    }
}