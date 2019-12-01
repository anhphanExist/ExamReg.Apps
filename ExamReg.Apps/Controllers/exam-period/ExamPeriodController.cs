using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamPeriod;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    }

    [Authorize(Policy = "CanManage")]
    public class ExamPeriodController : ApiController
    {
        private IExamPeriodService ExamPeriodService;
        public ExamPeriodController(ICurrentContext CurrentContext,
            IExamPeriodService ExamPeriodService) : base(CurrentContext)
        {
            this.ExamPeriodService = ExamPeriodService;
        }

        [Route(ExamPeriodRoute.List), HttpPost]
        public async Task<List<ExamPeriodDTO>> List()
        {
            List<ExamPeriod> examPeriods = await ExamPeriodService.List(new ExamPeriodFilter());
            List<ExamPeriodDTO> res = new List<ExamPeriodDTO>();
            examPeriods.ForEach(e => res.Add(new ExamPeriodDTO
            {
                ExamDate = e.ExamDate,
                StartHour = e.StartHour,
                FinishHour = e.FinishHour,
                SubjectName = e.SubjectName,
                ExamProgramName = e.ExamProgramName,
                Errors = e.Errors
            }));
            return res;
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
                ExamDate = res.ExamDate,
                StartHour = res.StartHour,
                FinishHour = res.FinishHour,
                SubjectName = res.SubjectName,
                ExamProgramName = res.ExamProgramName,
                Errors = res.Errors
            };
        }

        [Route(ExamPeriodRoute.Update), HttpPost]
        public async Task<ExamPeriodDTO> Update([FromBody] ExamPeriodDTO examPeriodRequestDTO)
        {
            ExamPeriod examPeriod = new ExamPeriod
            {
                ExamDate = examPeriodRequestDTO.ExamDate,
                StartHour = examPeriodRequestDTO.StartHour,
                FinishHour = examPeriodRequestDTO.FinishHour,
                SubjectName = examPeriodRequestDTO.SubjectName,
                ExamProgramName = examPeriodRequestDTO.ExamProgramName,
            };
            ExamPeriod res = await ExamPeriodService.Update(examPeriod);
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

        [Route(ExamPeriodRoute.Delete), HttpPost]
        public async Task<ExamPeriodDTO> Delete([FromBody] ExamPeriodDTO examPeriodRequestDTO)
        {
            ExamPeriod examPeriod = new ExamPeriod
            {
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
    }
}