using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Services.MStudent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExamReg.Apps.Controllers.student_profile
{
    public class StudentProfileRoute : Root
    {
        private const string Default = Base + "/student-profile";
        public const string GetInfoStudent = Default + "/get";
        public const string Update = Default + "/update";
    }

    [Authorize(Policy = "CanRegisterExam")]
    public class StudentProfileController : ApiController
    {
        private IStudentService StudentService;
        public StudentProfileController(ICurrentContext CurrentContext,
            IStudentService StudentService
            ) : base(CurrentContext)
        {
            this.StudentService = StudentService;
        }

        // Lấy thông tin của sinh viên
        [Route(StudentProfileRoute.GetInfoStudent), HttpPost]
        public async Task<StudentDTO> GetInfoStudent()
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

        [Route(StudentProfileRoute.Update), HttpPost]
        public async Task<StudentDTO> Update([FromBody] StudentDTO studentRequestDTO)
        {
            Student student = new Student
            {
                LastName = studentRequestDTO.LastName,
                GivenName = studentRequestDTO.GivenName,
                Birthday = studentRequestDTO.Birthday,
                Email = studentRequestDTO.Email
            };
            //Student res = await StudentService.Update(student);
            //return new StudentDTO
            //{
            //    StudentNumber = res.StudentNumber,
            //    Username = res.Username,
            //    LastName = res.LastName,
            //    GivenName = res.GivenName,
            //    Birthday = res.Birthday,
            //    Email = res.Email,
            //    Errors = res.Errors
            //};
            throw new NotImplementedException();
        }
    }
}