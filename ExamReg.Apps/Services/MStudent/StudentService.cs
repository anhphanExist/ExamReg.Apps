using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MStudent
{
    public interface IStudentService : IServiceScoped
    {
        Task<int> Count(StudentFilter filter);
        Task<List<Student>> List(StudentFilter filter);
        Task<Student> Create(Student student);
        Task<Student> Update(Student student);
        Task<Student> Delete(Student student);
        Task<List<Student>> ImportExcel(byte[] file);
    }
    public class StudentService : IStudentService
    {
        private IUOW UOW;
        public StudentService(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<int> Count(StudentFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> Create(Student student)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> Delete(Student student)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> List(StudentFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<Student> Update(Student student)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Student>> ImportExcel(byte[] file)
        {
            // Chuyển hoá dữ liệu excel từ byte về Student BO
            List<Student> excelTemplates = await LoadFromExcel(file);

            // Lấy list students trong database
            List<Student> students = await UOW.StudentRepository.List(new StudentFilter());

            // Duyệt qua các student BO trong excel để gán dữ liệu excel vào dữ liệu database
            foreach (Student template in excelTemplates)
            {
                // Kiểm tra dữ liệu đã tồn tại hay chưa
                Student student = students.Where(s => s.StudentNumber == template.StudentNumber).FirstOrDefault();

                // Nếu chưa tồn tại thì thêm mới, nếu đã tồn tại thì update
                try
                {
                    if (student == null)
                    {
                        student = new Student
                        {
                            Id = new Guid(),
                            StudentNumber = template.StudentNumber,
                            GivenName = template.GivenName,
                            LastName = template.LastName,
                            Username = Convert.ToString(template.StudentNumber),
                            Password = Convert.ToString(template.StudentNumber),
                            Birthday = template.Birthday,
                            Email = template.Email
                        };

                        students.Add(student);
                    }
                    else
                    {
                        student.GivenName = template.GivenName;
                        student.LastName = template.LastName;
                        student.Birthday = template.Birthday;
                        student.Email = template.Email;
                    }
                }
                catch (Exception e)
                {
                    throw;
                }
            }

            // Tiến hành giao dịch với database thông qua repo
            using (UOW.Begin())
            {
                try
                {
                    var result = await UOW.StudentRepository.BulkInsert(students);
                    await UOW.Commit();
                    return excelTemplates;
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }

        private async Task<List<Student>> LoadFromExcel(byte[] file)
        {
            List<Student> excelTemplates = new List<Student>();
            using (MemoryStream ms = new MemoryStream(file))
            {
                using (var package = new ExcelPackage(ms))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        // ...
                    }
                }
            }
            return excelTemplates;
        }
    }
}
