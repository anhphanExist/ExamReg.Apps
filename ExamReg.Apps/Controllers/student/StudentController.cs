﻿using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExamReg.Apps.Controllers.student
{
    public class StudentRoute : Root
    {
        private const string Default = Base + "/student";
        public const string List = Default + "/list";
        public const string Count = Default + "/count";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string ImportStudent = Default + "/import-student";
        public const string DownloadStudentTemplate = Default + "/download-student-template";
        public const string ExportStudent = Default + "/export-student";
        public const string ImportStudentTerm = Default + "/import-student-term";
        public const string DownloadStudentTermTemplate = Default + "/download-student-term-template";
        public const string ExportStudentTerm = Default + "/export-student-term";
    }

    [Authorize(Policy = "CanManage")]
    public class StudentController : ApiController
    {
        private IStudentService StudentService;
        public StudentController(ICurrentContext CurrentContext,
            IStudentService StudentService
            ) : base(CurrentContext)
        {
            this.StudentService = StudentService;
        }


        [Route(StudentRoute.ImportStudent), HttpPost]
        public async Task<List<Student>> ImportExcelStudent()
        {
            MemoryStream memoryStream = new MemoryStream();
            Request.Body.CopyTo(memoryStream);
            return await StudentService.ImportExcelStudent(memoryStream.ToArray());
        }

        [Route(StudentRoute.DownloadStudentTemplate), HttpGet]
        public async Task<FileResult> GetStudentTemplate()
        {
            byte[] data = await StudentService.GenerateStudentTemplate();
            return File(data, "application/octet-stream", "Student Template.xlsx");
        }

        [Route(StudentRoute.ExportStudent), HttpGet]
        public async Task<FileResult> ExportStudent()
        {
            byte[] data = await StudentService.ExportStudent();
            return File(data, "application/octet-stream", "Student.xlsx");
        }

        [Route(StudentRoute.ImportStudentTerm), HttpPost]
        public async Task<List<StudentTerm>> ImportExcelStudentTerm()
        {
            MemoryStream memoryStream = new MemoryStream();
            Request.Body.CopyTo(memoryStream);
            return await StudentService.ImportExcelStudentTerm(memoryStream.ToArray());
        }

        [Route(StudentRoute.DownloadStudentTermTemplate), HttpGet]
        public async Task<FileResult> GetStudentTermTemplate()
        {
            byte[] data = await StudentService.GenerateStudentTermTemplate();
            return File(data, "application/octet-stream", "StudentTerm Template.xlsx");
        }

        [Route(StudentRoute.ExportStudentTerm), HttpGet]
        public async Task<FileResult> ExportStudentTerm()
        {
            byte[] data = await StudentService.ExportStudentTerm();
            return File(data, "application/octet-stream", "StudentTerm.xlsx");
        }
    }
}