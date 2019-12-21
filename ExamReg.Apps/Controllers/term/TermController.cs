using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MSemester;
using ExamReg.Apps.Services.MTerm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Controllers.term
{
    public class TermRoute : Root
    {
        private const string Default = Base + "/term";
        public const string List = Default + "/list";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string DownloadTemplate = Default + "/download-template";
        public const string ListSemester = Default + "/list-semester";
    }

    [Authorize(Policy = "CanManage")]
    public class TermController : ApiController
    {
        private ITermService TermService;
        private ISemesterService SemesterService;
        public TermController(ICurrentContext CurrentContext,
            ITermService TermService,
            ISemesterService SemesterService
            ) : base(CurrentContext)
        {
            this.TermService = TermService;
            this.SemesterService = SemesterService;
        }

        [Route(TermRoute.List), HttpPost]
        public async Task<List<TermDTO>> List()
        {
            List<Term> res = await TermService.List(new TermFilter
            {
                OrderBy = TermOrder.SubjectName,
                OrderType = OrderType.ASC
            });
            return res.Select(r => new TermDTO
            {
                Id = r.Id,
                SubjectName = r.SubjectName,
                SemesterCode = r.SemesterCode,
                SemesterId = r.SemesterId,
                Errors = r.Errors
            }).ToList();
        }

        [Route(TermRoute.Create), HttpPost]
        public async Task<TermDTO> Create([FromBody] TermDTO termRequestDTO)
        {
            Term newTerm = new Term
            {
                SubjectName = termRequestDTO.SubjectName,
                SemesterCode = termRequestDTO.SemesterCode
            };
            Term res = await TermService.Create(newTerm);
            return new TermDTO
            {
                Id = res.Id,
                SubjectName = res.SubjectName,
                SemesterId = res.SemesterId,
                SemesterCode = res.SemesterCode,
                Errors = res.Errors
            };
        }

        [Route(TermRoute.Update), HttpPost]
        public async Task<TermDTO> Update([FromBody] TermDTO termRequestDTO)
        {
            Term term = new Term
            {
                Id = termRequestDTO.Id,
                SubjectName = termRequestDTO.SubjectName,
                SemesterCode = termRequestDTO.SemesterCode
            };
            Term res = await TermService.Update(term);
            return new TermDTO
            {
                Id = res.Id,
                SubjectName = res.SubjectName,
                SemesterCode = res.SemesterCode,
                SemesterId = res.SemesterId,
                Errors = res.Errors
            };
        }

        [Route(TermRoute.Delete), HttpPost]
        public async Task<TermDTO> Delete([FromBody] TermDTO termRequestDTO)
        {
            Term term = new Term
            {
                Id = termRequestDTO.Id
            };
            Term res = await TermService.Delete(term);
            return new TermDTO
            {
                Id = res.Id,
                SemesterCode = res.SemesterCode,
                SubjectName = res.SubjectName,
                SemesterId = res.SemesterId,
                Errors = res.Errors
            };
        }

        [Route(TermRoute.Import), HttpPost]
        public async Task<List<Term>> ImportExcel()
        {
            MemoryStream memoryStream = new MemoryStream();
            Request.Body.CopyTo(memoryStream);
            return await TermService.ImportExcel(memoryStream.ToArray());
        }

        [Route(TermRoute.Export), HttpGet]
        public async Task<FileResult> Export()
        {
            byte[] data = await TermService.Export();
            return File(data, "application/octet-stream", "Term.xlsx");
        }

        [Route(TermRoute.DownloadTemplate), HttpGet]
        public async Task<FileResult> GetTemplate()
        {
            byte[] data = await TermService.GenerateTemplate();
            return File(data, "application/octet-stream", "Term Template.xlsx");
        }

        [Route(TermRoute.ListSemester), HttpPost]
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
    }
}