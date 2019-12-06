using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MSemester;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.semester
{
    public class SemesterRoute : Root
    {
        private const string Default = Base + "/semester";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Delete = Default + "/delete";
    }

    [Authorize(Policy = "CanManage")]
    public class SemesterController : ApiController
    {
        private ISemesterService SemesterService;
        public SemesterController(ICurrentContext CurrentContext,
            ISemesterService SemesterService
            ) : base(CurrentContext)
        {
            this.SemesterService = SemesterService;
        }

        [Route(SemesterRoute.List), HttpPost]
        public async Task<List<SemesterDTO>> List()
        {
            List<Semester> res = await SemesterService.List(new SemesterFilter
            {
                OrderBy = SemesterOrder.Code,
                OrderType = OrderType.ASC
            });
            return res.Select(r => new SemesterDTO
            {
                Id = r.Id,
                Code = r.Code,
                StartYear = r.StartYear,
                EndYear = r.EndYear,
                IsFirstHalf = r.IsFirstHalf,
                Errors = r.Errors
            }).ToList();
        }

        [Route(SemesterRoute.Create), HttpPost]
        public async Task<SemesterDTO> Create([FromBody] SemesterDTO semesterRequestDTO)
        {
            Semester newSemester = new Semester
            {
                StartYear = semesterRequestDTO.StartYear,
                EndYear = semesterRequestDTO.EndYear,
                IsFirstHalf = semesterRequestDTO.IsFirstHalf
            };
            Semester res = await SemesterService.Create(newSemester);
            return new SemesterDTO
            {
                Id = res.Id,
                StartYear = res.StartYear,
                EndYear = res.EndYear,
                IsFirstHalf = res.IsFirstHalf,
                Code = res.Code,
                Errors = res.Errors
            };
        }

        [Route(SemesterRoute.Delete), HttpPost]
        public async Task<SemesterDTO> Delete([FromBody] SemesterDTO semesterRequestDTO)
        {
            Semester semester = new Semester
            {
                Id = semesterRequestDTO.Id
            };
            Semester res = await SemesterService.Delete(semester);
            return new SemesterDTO
            {
                Id = res.Id,
                StartYear = res.StartYear,
                EndYear = res.EndYear,
                IsFirstHalf = res.IsFirstHalf,
                Code = res.Code,
                Errors = res.Errors
            };
        }
    }
}