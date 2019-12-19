using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamProgram;
using ExamReg.Apps.Services.MExamRoomExamPeriod;
using ExamReg.Apps.Services.MStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.exam_register_result
{
    public class ExamRegisterResultRoute : Root
    {
        private const string Default = Base + "/exam-register-result";
        public const string GetStudentInfo = Default + "/get-student-info";
        public const string GetCurrentExamProgram = Default + "/result-get-current-exam-program";
        public const string ListExamRoomExamPeriod = Default + "/list";
        public const string PrintExamRegisterResult = Default + "/print";
    }

    [Authorize(Policy= "CanRegisterExam")]
    public class ExamRegisterResultController : ApiController
    {
        private IExamRoomExamPeriodService ExamRoomExamPeriodService;
        private IStudentService StudentService;
        private IExamProgramService ExamProgramService;
        public ExamRegisterResultController(ICurrentContext CurrentContext,
            IExamRoomExamPeriodService ExamRoomExamPeriodService,
            IStudentService StudentService,
            IExamProgramService ExamProgramService
            ) : base(CurrentContext)
        {
            this.ExamRoomExamPeriodService = ExamRoomExamPeriodService;
            this.StudentService = StudentService;
            this.ExamProgramService = ExamProgramService;
        }

        // Lấy thông tin của sinh viên
        [Route(ExamRegisterResultRoute.GetStudentInfo), HttpPost]
        public async Task<StudentDTO> GetStudentInfo()
        {
            Student res = await StudentService.Get(CurrentContext.StudentId);
            return new StudentDTO
            {
                Id = res.Id,
                StudentNumber = res.StudentNumber,
                LastName = res.LastName,
                GivenName = res.GivenName,
                Birthday = res.Birthday.ToString("dd-MM-yyyy"),
                Email = res.Email,
                Errors = res.Errors
            };
        }

        [Route(ExamRegisterResultRoute.GetCurrentExamProgram), HttpPost]
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

        // Lấy danh sách môn thi và phòng thi của sinh viên
        [Route(ExamRegisterResultRoute.ListExamRoomExamPeriod), HttpPost]
        public async Task<List<ExamRoomExamPeriodDTO>> ListExamRoomExamPeriod()
        {
            ExamProgram currentExamProgram = await ExamProgramService.GetCurrentExamProgram();
            ExamRoomExamPeriodFilter filter = new ExamRoomExamPeriodFilter
            {
                StudentNumber = CurrentContext.StudentNumber,
                ExamProgramId = new GuidFilter { Equal = currentExamProgram.Id },
                OrderBy = ExamOrder.ExamDate,
                OrderType = OrderType.ASC
            };
            List<ExamRoomExamPeriod> res = await ExamRoomExamPeriodService.List(filter);
            return res.Select(r => new ExamRoomExamPeriodDTO
            {
                ExamPeriodId = r.ExamPeriodId,
                ExamRoomId = r.ExamRoomId,
                TermId = r.TermId,
                ExamProgramId = r.ExamProgramId,
                ExamProgramName = r.ExamProgramName,
                ExamDate = r.ExamDate.ToString("dd-MM-yyyy"),
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                ExamRoomNumber = r.ExamRoomNumber,
                ExamRoomAmphitheaterName = r.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = r.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = r.Students.Count,
                SubjectName = r.SubjectName,
                Errors = r.Errors,
            }).ToList();
        }

        [Route(ExamRegisterResultRoute.PrintExamRegisterResult), HttpGet]
        public async Task<FileResult> PrintExamRegisterResult()
        {
            ExamProgram currentExamProgram = await ExamProgramService.GetCurrentExamProgram();
            ExamRoomExamPeriodFilter filter = new ExamRoomExamPeriodFilter
            {
                StudentNumber = CurrentContext.StudentNumber,
                ExamProgramId = new GuidFilter { Equal = currentExamProgram.Id },
                OrderBy = ExamOrder.ExamDate,
                OrderType = OrderType.ASC
            };
            MemoryStream stream = await ExamRoomExamPeriodService.PrintExamRegisterResult(filter);
            // Download Word document in the browser
            return File(stream, "application/msword", "ExamRegisterResult.docx");
        }
    }
}