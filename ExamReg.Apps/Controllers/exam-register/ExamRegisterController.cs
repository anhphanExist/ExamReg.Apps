using System;
using System.Collections.Generic;
using System.Globalization;
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
        private const string Default = Base + "/exam-register";
        public const string ListTerm = Default + "/list-term";
        public const string ListCurrentExamPeriod = Default + "/list-current-exam-period";
        public const string RegisterExam = Default + "/register-exam";
        public const string GetCurrentExamProgram = Default + "/register-get-current-exam-program";
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
        public async Task<List<TermDTO>> ListTerm()
        {
            ExamProgram currentExamProgram = await ExamProgramService.GetCurrentExamProgram();
            TermFilter filter = new TermFilter
            {
                StudentNumber = new IntFilter { Equal = CurrentContext.StudentNumber },
                SemesterId = new GuidFilter { Equal = currentExamProgram.SemesterId }
            };
            List<Term> res = await TermService.List(filter);
            return res.Select(r => new TermDTO
            {
                Id = r.Id,
                SemesterId = r.SemesterId,
                SubjectName = r.SubjectName,
                SemesterCode = r.SemesterCode,
                ExamPeriods = r.ExamPeriods
                    .Where(e => e.ExamProgramId.Equals(currentExamProgram.Id))
                    .Select(e => new ExamPeriodDTO
                    {
                        Id = e.Id,
                        ExamProgramId = e.ExamProgramId,
                        TermId = e.TermId,
                        SubjectName = e.SubjectName,
                        StartHour = e.StartHour,
                        FinishHour = e.FinishHour,
                        ExamDate = e.ExamDate.ToString("dd-MM-yyyy"),
                        ExamProgramName = e.ExamProgramName,
                        Errors = e.Errors
                    })
                    .ToList(),
                IsQualified = r.QualifiedStudents.Where(q => q.StudentNumber == CurrentContext.StudentNumber).Any(),
                Errors = r.Errors
            }).ToList();
        }

        // Hiển thị các ca thi hiện tại của môn thi mà sinh viên đã đăng ký
        [Route(ExamRegisterRoute.ListCurrentExamPeriod), HttpPost]
        public async Task<List<ExamPeriodDTO>> ListCurrentExamPeriod()
        {
            ExamProgram currentExamProgram = await ExamProgramService.GetCurrentExamProgram();
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                StudentNumber = CurrentContext.StudentNumber,
                ExamProgramId = new GuidFilter { Equal = currentExamProgram.Id }
            };
            List<ExamPeriod> res = await ExamPeriodService.List(filter);
            return res.Select(r => new ExamPeriodDTO
            {
                Id = r.Id,
                TermId = r.TermId,
                ExamProgramId = r.ExamProgramId,
                SubjectName = r.SubjectName,
                ExamDate = r.ExamDate.ToString("dd-MM-yyyy"),
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                ExamProgramName = r.ExamProgramName,
                Errors = r.Errors
            }).ToList();
        }

        // Tạo hoặc sửa đổi đăng ký dự thi các ca thi của môn thi
        [Route(ExamRegisterRoute.RegisterExam), HttpPost]
        public async Task<RegisterResponseDTO> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            
            List<ExamPeriod> examPeriods = registerRequestDTO.ExamPeriods.Where(e => e != null).Select(e => new ExamPeriod
            {
                Id = e.Id,
                ExamProgramId = e.ExamProgramId,
                TermId = e.TermId,
                ExamDate = DateTime.ParseExact(e.ExamDate, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                StartHour = e.StartHour,
                FinishHour = e.FinishHour
            }).ToList();

            bool res = await StudentService.Register(CurrentContext.StudentId, examPeriods);
            if (!res)
            {
                return new RegisterResponseDTO
                {
                    Message = string.Format("Đăng ký thất bại! Có ca thi đã đủ sinh viên đăng ký thi hoặc trùng giờ thi, vui lòng đăng kí ca thi khác"),
                    Errors = new List<string> {"Fail"}
                };
            }
            return new RegisterResponseDTO
            {
                Message = string.Format("Đăng ký thành công, vui lòng kiểm tra thông tin phiếu báo dự thi"),
                Errors = new List<string>()
            };
            
        }
    }
}