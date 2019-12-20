using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamProgram;
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
        public const string GetCurrentExamProgram = Default + "/watcher-get-current-exam-program";
        public const string ExportStudent = Default + "/export-student/{examPeriodId}/{examRoomId}";
    }
    [Authorize(Policy = "CanManage")]
    public class WatcherController : ApiController
    {
        private IExamRoomExamPeriodService ExamRoomExamPeriodService;
        private IExamProgramService ExamProgramService;
        public WatcherController(ICurrentContext CurrentContext,
            IExamRoomExamPeriodService ExamRoomExamPeriodService,
            IExamProgramService ExamProgramService
            ) : base(CurrentContext)
        {
            this.ExamRoomExamPeriodService = ExamRoomExamPeriodService;
            this.ExamProgramService = ExamProgramService;
        }

        // Lấy danh sách tất cả thông tin của các ca thi ứng với phòng thi và môn thi
        [Route(WatcherRoute.List), HttpPost]
        public async Task<List<WatcherDTO>> List()
        {
            ExamProgram currentExamProgram = await ExamProgramService.GetCurrentExamProgram();
            List<ExamRoomExamPeriod> examRoomExamPeriods = await ExamRoomExamPeriodService.List(new ExamRoomExamPeriodFilter
            {
                ExamProgramId = new GuidFilter { Equal = currentExamProgram.Id }
            });
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
                ExamDate = r.ExamDate.ToString("dd-MM-yyyy"),
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                SubjectName = r.SubjectName,
                Errors = r.Errors
            }));
            return res;
        }

        [Route(WatcherRoute.GetCurrentExamProgram), HttpPost]
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

        // Xuất danh sách sinh viên thi trong 1 phòng thi của 1 ca thi của 1 môn thi
        [Route(WatcherRoute.ExportStudent), HttpGet]
        public async Task<FileResult> ExportStudent(Guid examPeriodId, Guid examRoomId)
        {
            ExamRoomExamPeriodFilter filter = new ExamRoomExamPeriodFilter
            {
                ExamPeriodId = new GuidFilter { Equal = examPeriodId },
                ExamRoomId = new GuidFilter { Equal = examRoomId }
            };
            ExamRoomExamPeriod examRoomExamPeriod = await ExamRoomExamPeriodService.Get(filter);
            
            byte[] data = await ExamRoomExamPeriodService.ExportStudent(filter);
            return File(data, "application/octet-stream", "Exam" 
                + examRoomExamPeriod.ExamProgramName + "_" 
                + examRoomExamPeriod.ExamDate + "_"
                + examRoomExamPeriod.StartHour + "h_"
                + examRoomExamPeriod.FinishHour + "h_"
                + examRoomExamPeriod.SubjectName + "_"
                + examRoomExamPeriod.ExamRoomAmphitheaterName + "_"
                + examRoomExamPeriod.ExamRoomNumber
                +".xlsx");
        }
    }
}