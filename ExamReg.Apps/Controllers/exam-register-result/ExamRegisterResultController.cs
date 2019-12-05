﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MExamRoomExamPeriod;
using ExamReg.Apps.Services.MStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.exam_register_result
{
    public class ExamRegisterResultRoute : Root
    {
        private const string Default = Base + "/exam-register-result";
        public const string GetStudentInfo = Default + "/get-student-info";
        public const string ListExamRoomExamPeriod = Default + "/list";
    }

    [Authorize(Policy= "CanRegisterExam")]
    public class ExamRegisterResultController : ApiController
    {
        private IExamRoomExamPeriodService ExamRoomExamPeriodService;
        private IStudentService StudentService;
        public ExamRegisterResultController(ICurrentContext CurrentContext,
            IExamRoomExamPeriodService ExamRoomExamPeriodService,
            IStudentService StudentService) : base(CurrentContext)
        {
            this.ExamRoomExamPeriodService = ExamRoomExamPeriodService;
            this.StudentService = StudentService;
        }

        // Lấy thông tin của sinh viên
        [Route(ExamRegisterResultRoute.GetStudentInfo), HttpPost]
        public async Task<StudentDTO> GetStudentInfo()
        {
            Student res = await StudentService.Get(CurrentContext.StudentId);
            return new StudentDTO
            {
                Id = res.Id,
                StudentNumber = res.StudentNumber,
                LastName = res.LastName,
                GivenName = res.GivenName,
                Birthday = res.Birthday,
                Email = res.Email,
                Errors = res.Errors
            };
        }

        // Lấy danh sách môn thi và phòng thi của sinh viên
        [Route(ExamRegisterResultRoute.ListExamRoomExamPeriod), HttpPost]
        public async Task<List<ExamRoomExamPeriodDTO>> ListExamRoomExamPeriod()
        {
            ExamRoomExamPeriodFilter filter = new ExamRoomExamPeriodFilter
            {
                StudentNumber = new IntFilter { Equal = CurrentContext.StudentNumber },
                OrderBy = ExamOrder.SubjectName,
                OrderType = OrderType.ASC
            };
            List<ExamRoomExamPeriod> res = await ExamRoomExamPeriodService.List(filter);
            return res.Select(r => new ExamRoomExamPeriodDTO
            {
                ExamPeriodId = r.ExamPeriodId,
                ExamRoomId = r.ExamRoomId,
                TermId = r.TermId,
                ExamProgramId = r.ExamProgramId,
                ExamProgramName = r.ExamProgramName,
                ExamDate = r.ExamDate,
                StartHour = r.StartHour,
                FinishHour = r.FinishHour,
                ExamRoomNumber = r.ExamRoomNumber,
                ExamRoomAmphitheaterName = r.ExamRoomAmphitheaterName,
                ExamRoomComputerNumber = r.ExamRoomComputerNumber,
                CurrentNumberOfStudentRegistered = r.Students.Count,
                SubjectName = r.SubjectName,
                Errors = r.Errors,
            }).ToList();
        }
    }
}