using System;
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
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
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

        [Route(WatcherRoute.List), HttpPost]
        public async Task<List<WatcherDTO>> List()
        {
            List<ExamRoomExamPeriod> examRoomExamPeriods = await ExamRoomExamPeriodService.List(new ExamRoomExamPeriodFilter());
            List<WatcherDTO> res = new List<WatcherDTO>();
            examRoomExamPeriods.ForEach(r => res.Add(new WatcherDTO
            {
                ExamProgramName = r.ExamProgramName,
                ExamRoomNumber = r.ExamRoomNumber,
                ExamRoomAmphitheaterName = r.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = r.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = r.CurrentNumberOfStudentRegistered,
                ExamDate = r.ExamDate,
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                SubjectName = r.SubjectName,
                Errors = r.Errors
            }));
            return res;
        }

        [Route(WatcherRoute.Create), HttpPost]
        public async Task<WatcherDTO> Create([FromBody] WatcherDTO watcherRequestDTO)
        {
            ExamRoomExamPeriod newWatcher = new ExamRoomExamPeriod
            {
                ExamProgramName = watcherRequestDTO.ExamProgramName,
                ExamRoomNumber = watcherRequestDTO.ExamRoomNumber,
                ExamRoomAmphitheaterName = watcherRequestDTO.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = watcherRequestDTO.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = watcherRequestDTO.CurrentNumberOfStudentRegistered,
                ExamDate = watcherRequestDTO.ExamDate,
                StartHour = watcherRequestDTO.StartHour,
                FinishHour = watcherRequestDTO.FinishHour,
                SubjectName = watcherRequestDTO.SubjectName,
            };
            ExamRoomExamPeriod res = await ExamRoomExamPeriodService.Create(newWatcher);
            return new WatcherDTO
            {
                ExamProgramName = res.ExamProgramName,
                ExamRoomNumber = res.ExamRoomNumber,
                ExamRoomAmphitheaterName = res.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = res.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = res.CurrentNumberOfStudentRegistered,
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                Errors = res.Errors
            };
        }

        [Route(WatcherRoute.Update), HttpPost]
        public async Task<WatcherDTO> Update([FromBody] WatcherDTO watcherRequestDTO)
        {
            ExamRoomExamPeriod newWatcher = new ExamRoomExamPeriod
            {
                ExamProgramName = watcherRequestDTO.ExamProgramName,
                ExamRoomNumber = watcherRequestDTO.ExamRoomNumber,
                ExamRoomAmphitheaterName = watcherRequestDTO.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = watcherRequestDTO.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = watcherRequestDTO.CurrentNumberOfStudentRegistered,
                ExamDate = watcherRequestDTO.ExamDate,
                StartHour = watcherRequestDTO.StartHour,
                FinishHour = watcherRequestDTO.FinishHour,
                SubjectName = watcherRequestDTO.SubjectName,
            };
            ExamRoomExamPeriod res = await ExamRoomExamPeriodService.Update(newWatcher);
            return new WatcherDTO
            {
                ExamProgramName = res.ExamProgramName,
                ExamRoomNumber = res.ExamRoomNumber,
                ExamRoomAmphitheaterName = res.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = res.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = res.CurrentNumberOfStudentRegistered,
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                Errors = res.Errors
            };
        }

        [Route(WatcherRoute.Delete), HttpPost]
        public async Task<WatcherDTO> Delete([FromBody] WatcherDTO watcherRequestDTO)
        {
            ExamRoomExamPeriod newWatcher = new ExamRoomExamPeriod
            {
                ExamProgramName = watcherRequestDTO.ExamProgramName,
                ExamRoomNumber = watcherRequestDTO.ExamRoomNumber,
                ExamRoomAmphitheaterName = watcherRequestDTO.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = watcherRequestDTO.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = watcherRequestDTO.CurrentNumberOfStudentRegistered,
                ExamDate = watcherRequestDTO.ExamDate,
                StartHour = watcherRequestDTO.StartHour,
                FinishHour = watcherRequestDTO.FinishHour,
                SubjectName = watcherRequestDTO.SubjectName,
            };
            ExamRoomExamPeriod res = await ExamRoomExamPeriodService.Delete(newWatcher);
            return new WatcherDTO
            {
                ExamProgramName = res.ExamProgramName,
                ExamRoomNumber = res.ExamRoomNumber,
                ExamRoomAmphitheaterName = res.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = res.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = res.CurrentNumberOfStudentRegistered,
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                Errors = res.Errors
            };
        }

        [Route(WatcherRoute.ExportStudent), HttpPost]
        public async Task<WatcherDTO> ExportStudent([FromBody] WatcherDTO watcherRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}