using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamProgram;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
    }

    [Authorize(Policy = "CanManage")]
    public class ExamProgramController : ApiController
    {
        private IExamProgramService ExamProgramService;
        public ExamProgramController(ICurrentContext CurrentContext,
            IExamProgramService ExamProgramService) : base(CurrentContext)
        {
            this.ExamProgramService = ExamProgramService;
        }

        [Route(ExamProgramRoute.List), HttpPost]
        public async Task<List<ExamProgramDTO>> List()
        {
            List<ExamProgram> examPrograms = await ExamProgramService.List(new ExamProgramFilter());
            List<ExamProgramDTO> res = new List<ExamProgramDTO>();
            examPrograms.ForEach(e => res.Add(new ExamProgramDTO
            {
                Id = e.Id,
                Name = e.Name,
                Errors = e.Errors
            }));
            return res;
        }

        [Route(ExamProgramRoute.Create), HttpPost]
        public async Task<ExamProgramDTO> Create([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram newExamProgram = new ExamProgram
            {
                Name = examProgramRequestDTO.Name
            };
            ExamProgram res = await ExamProgramService.Create(newExamProgram);
            return new ExamProgramDTO
            {
                Id = res.Id,
                Name = res.Name,
                Errors = res.Errors
            };
        }

        [Route(ExamProgramRoute.Update), HttpPost]
        public async Task<ExamProgramDTO> Update([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram examProgram = new ExamProgram
            {
                Id = examProgramRequestDTO.Id,
                Name = examProgramRequestDTO.Name
            };
            ExamProgram res = await ExamProgramService.Update(examProgram);
            return new ExamProgramDTO
            {
                Id = examProgramRequestDTO.Id,
                Name = res.Name,
                Errors = res.Errors
            };
        }

        [Route(ExamProgramRoute.Delete), HttpPost]
        public async Task<ExamProgramDTO> Delete([FromBody] ExamProgramDTO examProgramRequestDTO)
        {
            ExamProgram examProgram = new ExamProgram
            {
                Id = examProgramRequestDTO.Id,
                Name = examProgramRequestDTO.Name
            };
            ExamProgram res = await ExamProgramService.Delete(examProgram);
            return new ExamProgramDTO
            {
                Id = res.Id,
                Name = res.Name,
                Errors = res.Errors
            };
        }
    }
}