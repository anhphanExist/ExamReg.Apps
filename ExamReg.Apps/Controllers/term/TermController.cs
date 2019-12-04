using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MTerm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
    }

    [Authorize(Policy = "CanManage")]
    public class TermController : ApiController
    {
        private ITermService TermService;
        public TermController(ICurrentContext CurrentContext,
            ITermService TermService
            ) : base(CurrentContext)
        {
            this.TermService = TermService;
        }

        [Route(TermRoute.List), HttpPost]
        public async Task<List<TermDTO>> List()
        {
            List<Term> terms = await TermService.List(new TermFilter());
            List<TermDTO> res = new List<TermDTO>();
            terms.ForEach(t => res.Add(new TermDTO
            {
                Id = t.Id,
                SemesterCode = t.SemesterCode,
                SubjectName = t.SubjectName,
                Errors = t.Errors
            }));
            return res;
        }

        [Route(TermRoute.Create), HttpPost]
        public async Task<TermDTO> Create([FromBody] TermDTO termRequestDTO)
        {
            Term newTerm = new Term
            {
                SemesterCode = termRequestDTO.SemesterCode,
                SubjectName = termRequestDTO.SubjectName
            };
            Term res = await TermService.Create(newTerm);
            return new TermDTO
            {
                Id = res.Id,
                SemesterCode = res.SemesterCode,
                SubjectName = res.SubjectName,
                Errors = res.Errors
            };
        }

        [Route(TermRoute.Update), HttpPost]
        public async Task<TermDTO> Update([FromBody] TermDTO termRequestDTO)
        {
            Term term = new Term
            {
                Id = termRequestDTO.Id,
                SemesterCode = termRequestDTO.SemesterCode,
                SubjectName = termRequestDTO.SubjectName
            };
            Term res = await TermService.Update(term);
            return new TermDTO
            {
                Id = res.Id,
                SemesterCode = res.SemesterCode,
                SubjectName = res.SubjectName,
                Errors = res.Errors
            };
        }

        [Route(TermRoute.Delete), HttpPost]
        public async Task<TermDTO> Delete([FromBody] TermDTO termRequestDTO)
        {
            Term term = new Term
            {
                Id = termRequestDTO.Id,
                SemesterCode = termRequestDTO.SemesterCode,
                SubjectName = termRequestDTO.SubjectName
            };
            Term res = await TermService.Delete(term);
            return new TermDTO
            {
                Id = res.Id,
                SemesterCode = res.SemesterCode,
                SubjectName = res.SubjectName,
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
    }
}