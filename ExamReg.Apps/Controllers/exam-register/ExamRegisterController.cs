using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamPeriod;
using ExamReg.Apps.Services.MExamProgram;
using ExamReg.Apps.Services.MExamRoomExamPeriod;
using ExamReg.Apps.Services.MStudent;
using ExamReg.Apps.Services.MTerm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.exam_register
{
    public class ExamRegisterRoute : Root
    {
        private const string Default = Base + "/exam-register-result";
        public const string ListTerm = Default + "/list-term";
        public const string ListCurrentExamPeriod = Default + "/list-current-exam-period";
        public const string RegisterExam = Default + "/register-exam";
        public const string GetCurrentExamProgram = Default + "/get-current-exam-program";
    }

    [Authorize(Policy = "CanRegisterExam")]
    public class ExamRegisterController : ApiController
    {
        private IStudentService StudentService;
        private IExamPeriodService ExamPeriodService;
        private ITermService TermService;
        private IExamRoomExamPeriodService ExamRoomExamPeriodService;
        private IExamProgramService ExamProgramService;
        public ExamRegisterController(ICurrentContext CurrentContext,
            IExamPeriodService ExamPeriodService,
            IStudentService StudentService,
            ITermService TermService,
            IExamRoomExamPeriodService ExamRoomExamPeriodService,
            IExamProgramService ExamProgramService
            ) : base(CurrentContext)
        {
            this.StudentService = StudentService;
            this.TermService = TermService;
            this.ExamPeriodService = ExamPeriodService;
            this.ExamRoomExamPeriodService = ExamRoomExamPeriodService;
            this.ExamProgramService = ExamProgramService;
        }

        [Route(ExamRegisterRoute.GetCurrentExamProgram), HttpPost]
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

        // Hiển thị các môn thi cho sinh viên chọn ca thi
        [Route(ExamRegisterRoute.ListTerm), HttpPost]
        public async Task<List<TermDTO>> ListTerm([FromBody] TermFilterDTO termRequestFilterDTO)
        {
            TermFilter filter = new TermFilter
            {
                StudentNumber = new IntFilter { Equal = CurrentContext.StudentNumber },
                SemesterId = new GuidFilter { Equal = termRequestFilterDTO.SemesterId },
                SemesterCode = new StringFilter { Equal = termRequestFilterDTO.SemesterCode }
            };
            List<Term> res = await TermService.List(filter);
            return res.Select(r => new TermDTO
            {
                Id = r.Id,
                SemesterId = r.SemesterId,
                SubjectName = r.SubjectName,
                SemesterCode = r.SemesterCode,
                ExamPeriods = r.ExamPeriods.Select(e => new ExamPeriodDTO
                {
                    Id = e.Id,
                    ExamProgramId = e.ExamProgramId,
                    TermId = e.TermId,
                    SubjectName = e.SubjectName,
                    StartHour = e.StartHour,
                    FinishHour = e.FinishHour,
                    ExamDate = e.ExamDate,
                    ExamProgramName = e.ExamProgramName,
                    Errors = e.Errors
                }).ToList(),
                IsQualified = r.QualifiedStudents.Where(q => q.StudentNumber == CurrentContext.StudentNumber).Any(),
                Errors = r.Errors
            }).ToList();
        }

        // Hiển thị các ca thi hiện tại của môn thi mà sinh viên đã đăng ký
        [Route(ExamRegisterRoute.ListCurrentExamPeriod), HttpPost]
        public async Task<List<ExamPeriodDTO>> ListCurrentExamPeriod([FromBody] ExamPeriodFilterDTO examPeriodRequestFilterDTO)
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                StudentNumber = new IntFilter { Equal = CurrentContext.StudentNumber },
                ExamProgramId = new GuidFilter { Equal = examPeriodRequestFilterDTO.ExamProgramId },
                ExamProgramName = new StringFilter { Equal = examPeriodRequestFilterDTO.ExamProgramName }
            };
            List<ExamPeriod> res = await ExamPeriodService.List(filter);
            return res.Select(r => new ExamPeriodDTO
            {
                Id = r.Id,
                TermId = r.TermId,
                ExamProgramId = r.ExamProgramId,
                SubjectName = r.SubjectName,
                ExamDate = r.ExamDate,
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                ExamProgramName = r.ExamProgramName,
                Errors = r.Errors
            }).ToList();
        }

        // Tạo hoặc sửa đổi đăng ký dự thi các ca thi của môn thi
        [Route(ExamRegisterRoute.RegisterExam), HttpPost]
        public async Task RegisterExam([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            Parallel.ForEach(registerRequestDTO.ExamPeriods, async examPeriod =>
            {
                await StudentService.RegisterExam(CurrentContext.StudentId, examPeriod.Id, examPeriod.TermId);
            });
        }
    }
}