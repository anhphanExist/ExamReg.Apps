using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamPeriod;
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
        public const string ListCurrentExamRoomExamPeriod = Default + "/list-current-exam-room-exam-period";
        public const string ListCurrentExamPeriod = Default + "/list-current-exam-period";
        public const string CreateExamRoomExamPeriod = Default + "/create-exam-room-exam-period";
    }

    [Authorize(Policy = "CanRegisterExam")]
    public class ExamRegisterController : ApiController
    {
        private IStudentService StudentService;
        private IExamPeriodService ExamPeriodService;
        private ITermService TermService;
        public ExamRegisterController(ICurrentContext CurrentContext,
            IExamPeriodService ExamPeriodService,
            IStudentService StudentService,
            ITermService TermService
            ) : base(CurrentContext)
        {
            this.StudentService = StudentService;
            this.TermService = TermService;
            this.ExamPeriodService = ExamPeriodService;
        }

        // Hiển thị các môn thi cho sinh viên chọn ca thi
        [Route(ExamRegisterRoute.ListTerm), HttpPost]
        public async Task<List<TermDTO>> ListTerm()
        {
            TermFilter filter = new TermFilter
            {
                StudentNumber = new IntFilter { Equal = CurrentContext.StudentNumber }
            };
            List<Term> res = await TermService.List(filter);
            return res.Select(r => new TermDTO
            {
                SubjectName = r.SubjectName,
                SemesterCode = r.SemesterCode,
                ExamPeriods = r.ExamPeriods.Select(e => new ExamPeriodDTO
                {
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

        // Hiển thị các ca thi hiện tại của môn thi
        [Route(ExamRegisterRoute.ListCurrentExamPeriod), HttpPost]
        public async Task<List<ExamPeriodDTO>> ListCurrentExamPeriod()
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                StudentNumber = new IntFilter { Equal = CurrentContext.StudentNumber }
            };
            List<ExamPeriod> res = await ExamPeriodService.List(filter);
            return res.Select(r => new ExamPeriodDTO
            {
                SubjectName = r.SubjectName,
                ExamDate = r.ExamDate,
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                ExamProgramName = r.ExamProgramName,
                Errors = r.Errors
            }).ToList();
        }
    }
}