using ExamReg.Apps.Common;
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
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string ResetStudentPassword = Default + "/reset-password"; // Vấn đề bảo mật: Nếu kẻ tấn công lấy được token của admin và gửi request reset password và chiếm được tài khoản của sinh viên
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

        [Route(StudentRoute.List), HttpPost]
        public async Task<List<StudentDTO>> List()
        {
            List<Student> students = await StudentService.List(new StudentFilter());
            List<StudentDTO> res = new List<StudentDTO>();
            students.ForEach(s => res.Add(new StudentDTO
            {
                Id = s.Id,
                StudentNumber = s.StudentNumber,
                Username = s.Username,
                LastName = s.LastName,
                GivenName = s.GivenName,
                Birthday = s.Birthday,
                Email = s.Email,
                Errors = s.Errors
            }));
            return res;
        }

        [Route(StudentRoute.Create), HttpPost]
        public async Task<StudentDTO> Create([FromBody] StudentDTO studentRequestDTO)
        {
            Student newStudent = new Student
            {
                StudentNumber = studentRequestDTO.StudentNumber,
                Username = studentRequestDTO.Username,
                LastName = studentRequestDTO.LastName,
                GivenName = studentRequestDTO.GivenName,
                Birthday = studentRequestDTO.Birthday,
                Email = studentRequestDTO.Email
            };
            Student res = await StudentService.Create(newStudent);
            return new StudentDTO
            {
                Id = res.Id,
                StudentNumber = res.StudentNumber,
                Username = res.Username,
                LastName = res.LastName,
                GivenName = res.GivenName,
                Birthday = res.Birthday,
                Email = res.Email,
                Errors = res.Errors
            };
        }

        [Route(StudentRoute.Update), HttpPost]
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

        [Route(StudentRoute.Delete), HttpPost]
        public async Task<StudentDTO> Delete([FromBody] StudentDTO studentRequestDTO)
        {
            Student student = new Student
            {
                Id = studentRequestDTO.Id,
                StudentNumber = studentRequestDTO.StudentNumber
            };
            Student res = await StudentService.Delete(student);
            return new StudentDTO
            {
                Id = res.Id,
                StudentNumber = res.StudentNumber,
                Username = res.Username,
                LastName = res.LastName,
                GivenName = res.GivenName,
                Birthday = res.Birthday,
                Email = res.Email,
                Errors = res.Errors
            };
        }

        [Route(StudentRoute.ResetStudentPassword), HttpPost]
        public async Task<StudentDTO> ResetStudentPassword([FromBody] StudentDTO studentRequestDTO)
        {
            Student student = new Student
            {
                StudentNumber = studentRequestDTO.StudentNumber
            };
            Student res = await StudentService.ResetPassword(student);
            return new StudentDTO
            {
                Id = res.Id,
                StudentNumber = res.StudentNumber,
                Username = res.Username,
                LastName = res.LastName,
                GivenName = res.GivenName,
                Birthday = res.Birthday,
                Email = res.Email,
                Errors = res.Errors
            };
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