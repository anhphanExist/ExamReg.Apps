using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamProgram;
using ExamReg.Apps.Services.MSemester;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Controllers.exam_program
{
    public class ExamProgramRoute : Root
    {
        private const string Default = Base + "/exam-program";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string ListSemester = Default + "/list-semester";
        public const string SetCurrentExamProgram = Default + "/set-current";
    }

    [Authorize(Policy = "CanManage")]
    public class ExamProgramController : ApiController
    {
        private IExamProgramService ExamProgramService;
        private ISemesterService SemesterService;
        public ExamProgramController(ICurrentContext CurrentContext,
            IExamProgramService ExamProgramService,
            ISemesterService SemesterService
            ) : base(CurrentContext)
        {
            this.ExamProgramService = ExamProgramService;
            this.SemesterService = SemesterService;
        }

        [Route(ExamProgramRoute.List), HttpPost]
        public async Task<List<ExamProgramDTO>> List()
        {
            List<ExamProgram> examPrograms = await ExamProgramService.List(new ExamProgramFilter
            {
                OrderBy = ExamProgramOrder.SemesterCode,
                OrderType = OrderType.ASC
            });
            return examPrograms.Select(e => new ExamProgramDTO
            {
                Id = e.Id,
                SemesterId = e.SemesterId,
                Name = e.Name,
                SemesterCode = e.SemesterCode,
                IsCurrent = e.IsCurrent,
                Errors = e.Errors
            }).ToList();
        }

        [Route(ExamProgramRoute.Create), HttpPost]
        public async Task<ExamProgramDTO> Create([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram newExamProgram = new ExamProgram
            {
                Name = examProgramRequestDTO.Name,
                SemesterCode = examProgramRequestDTO.SemesterCode
            };
            ExamProgram res = await ExamProgramService.Create(newExamProgram);
            return new ExamProgramDTO
            {
                Id = res.Id,
                SemesterId = res.SemesterId,
                Name = res.Name,
                SemesterCode = res.SemesterCode,
                IsCurrent = res.IsCurrent,
                Errors = res.Errors
            };
        }

        [Route(ExamProgramRoute.Update), HttpPost]
        public async Task<ExamProgramDTO> Update([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram examProgram = new ExamProgram
            {
                Id = examProgramRequestDTO.Id,
                Name = examProgramRequestDTO.Name,
                SemesterCode = examProgramRequestDTO.SemesterCode
            };
            ExamProgram res = await ExamProgramService.Update(examProgram);
            return new ExamProgramDTO
            {
                Id = res.Id,
                SemesterId = res.SemesterId,
                SemesterCode = res.SemesterCode,
                Name = res.Name,
                IsCurrent = res.IsCurrent,
                Errors = res.Errors
            };
        }

        [Route(ExamProgramRoute.Delete), HttpPost]
        public async Task<ExamProgramDTO> Delete([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram examProgram = new ExamProgram
            {
                Id = examProgramRequestDTO.Id,
                Name = examProgramRequestDTO.Name,
                SemesterCode = examProgramRequestDTO.SemesterCode,
                IsCurrent = examProgramRequestDTO.IsCurrent
            };
            ExamProgram res = await ExamProgramService.Delete(examProgram);
            return new ExamProgramDTO
            {
                Id = res.Id,
                SemesterId = res.SemesterId,
                Name = res.Name,
                SemesterCode = res.SemesterCode,
                IsCurrent = res.IsCurrent,
                Errors = res.Errors
            };
        }

        [Route(ExamProgramRoute.ListSemester), HttpPost]
        public async Task<List<SemesterDTO>> ListSemester()
        {
            List<Semester> semesters = await SemesterService.List(new SemesterFilter
            {
                OrderBy = SemesterOrder.Code,
                OrderType = OrderType.DESC
            });
            return semesters.Select(s => new SemesterDTO
            {
                Id = s.Id,
                Code = s.Code,
                StartYear = s.StartYear,
                EndYear = s.EndYear,
                IsFirstHalf = s.IsFirstHalf,
                Errors = s.Errors
            }).ToList();
        }

        [Route(ExamProgramRoute.SetCurrentExamProgram), HttpPost]
        public async Task<ExamProgramDTO> SetCurrentExamProgram([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram examProgram = new ExamProgram
            {
                Id = examProgramRequestDTO.Id
            };
            ExamProgram res = await ExamProgramService.SetCurrentExamProgram(examProgram);
            return new ExamProgramDTO
            {
                Id = res.Id,
                SemesterId = res.SemesterId,
                Name = res.Name,
                SemesterCode = res.SemesterCode,
                IsCurrent = res.IsCurrent,
                Errors = res.Errors
            };
        }
    }
}