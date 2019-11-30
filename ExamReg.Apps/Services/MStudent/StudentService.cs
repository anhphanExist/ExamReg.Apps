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
        Task<List<Student>> ImportExcelStudent(byte[] file);
        Task<byte[]> GenerateStudentTemplate();
        Task<byte[]> ExportStudent();
        Task<List<StudentTerm>> ImportExcelStudentTerm(byte[] file);
        Task<byte[]> GenerateStudentTermTemplate();
        Task<byte[]> ExportStudentTerm();
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

        public async Task<List<Student>> ImportExcelStudent(byte[] file)
        {
            // Chuyển hoá dữ liệu excel từ byte về Student BO
            List<Student> excelTemplates = await LoadStudentFromExcel(file);

            // Lấy dữ liệu đã tồn tại trong database
            List<Student> students = await UOW.StudentRepository.List(new StudentFilter());
            List<User> users = await UOW.UserRepository.List(new UserFilter());
            List<User> newUsers = new List<User>();

            // Duyệt qua các student BO trong excel để gán dữ liệu excel vào dữ liệu database
            foreach (Student template in excelTemplates)
            {
                // Kiểm tra dữ liệu đã tồn tại hay chưa
                Student student = students.Where(s => s.StudentNumber == template.StudentNumber).FirstOrDefault();
                User user = users.Where(u => u.Username.Equals(template.StudentNumber)).FirstOrDefault();

                // Nếu student chưa tồn tại thì thêm mới và tạo mới tài khoản 
                // Không có chuyện student chưa tồn tại mà đã có tài khoản
                // Nếu student đã tồn tại mà chưa có tài khoản thì tạo mới tài khoản
                // Nếu đã tồn tại cả student lẫn tài khoản thì update student và không sửa đổi tài khoản
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
                            Username = template.StudentNumber.ToString(),
                            Password = template.StudentNumber.ToString(),
                            Birthday = template.Birthday,
                            Email = template.Email
                        };
                        students.Add(student);
                        user = new User
                        {
                            Id = new Guid(),
                            Username = student.Username,
                            Password = student.Password,
                            IsAdmin = false,
                            StudentId = student.Id,
                            StudentGivenName = student.GivenName,
                            StudentLastName = student.LastName
                        };
                        newUsers.Add(user);
                    }
                    else if (user == null)
                    {
                        user = new User
                        {
                            Id = new Guid(),
                            Username = student.Username,
                            Password = student.Password,
                            IsAdmin = false,
                            StudentId = student.Id,
                            StudentGivenName = student.GivenName,
                            StudentLastName = student.LastName
                        };
                        newUsers.Add(user);
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
                    throw e;
                }
            }

            // Tiến hành giao dịch với database thông qua repo
            using (UOW.Begin())
            {
                try
                {
                    // Merge dữ liệu vào repository
                    await UOW.StudentRepository.BulkMerge(students);
                    await UOW.UserRepository.BulkInsert(newUsers);
                    await UOW.Commit();
                    return excelTemplates;
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    throw e;
                }
            }
        }

        private async Task<List<Student>> LoadStudentFromExcel(byte[] file)
        {
            // tạo List sinh viên để chứa dữ liệu
            List<Student> excelTemplates = new List<Student>();
            using (MemoryStream ms = new MemoryStream(file))
            {
                using (var package = new ExcelPackage(ms))
                {
                    // lấy worksheet đầu tiên
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        // bỏ qua các dòng không chứa đủ thông tin
                        string studentNumber = worksheet.Cells[i, 2].Value?.ToString().Trim();
                        string lastName = worksheet.Cells[i, 3].Value?.ToString().Trim();
                        string givenName = worksheet.Cells[i, 4].Value?.ToString().Trim();
                        string birthday = worksheet.Cells[i, 5].Value?.ToString().Trim();
                        if (string.IsNullOrEmpty(studentNumber) ||
                            string.IsNullOrEmpty(lastName) ||
                            string.IsNullOrEmpty(givenName) ||
                            string.IsNullOrEmpty(birthday))
                            continue;
                        // thêm dòng đủ thông tin vào biến
                        Student excelTemplate = new Student
                        {
                            StudentNumber = int.Parse(studentNumber),
                            LastName = lastName,
                            GivenName = givenName,
                            Birthday = DateTime.Parse(birthday),
                            Email = worksheet.Cells[i, 6].Value?.ToString().Trim(),
                        };
                        excelTemplates.Add(excelTemplate);
                    }
                }
            }
            // trả về list dữ liệu sinh viên để xử lý sau
            return excelTemplates;
        }

        public async Task<byte[]> GenerateStudentTemplate()
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Tạo header
                var studentHeaders = new List<string[]>
                {
                    new string[] 
                    {
                        "STT",
                        "Mã số sinh viên",
                        "Họ",
                        "Tên",
                        "Ngày sinh",
                        "Email",
                        "Tài khoản",
                        "Mật khẩu"
                    }
                };

                // Tạo data rỗng
                List<object[]> data = new List<object[]>();

                // Tạo worksheet từ data và header
                excel.GenerateWorksheet("Sinh viên", studentHeaders, data);

                // trả về dữ liệu dạng byte của excelPackage để xuất ra file thật
                return excel.GetAsByteArray();
            }
        }

        public async Task<byte[]> ExportStudent()
        {
            // Lấy dữ liệu trong database
            StudentFilter studentFilter = new StudentFilter()
            {
                OrderBy = StudentOrder.StudentNumber
            };
            List<Student> students = await UOW.StudentRepository.List(studentFilter);

            // Mở excelPackage
            using (ExcelPackage excel = new ExcelPackage())
            {
                // đặt header
                var studentHeaders = new List<string[]>()
                {
                    new string[] 
                    {
                        "STT",
                        "Mã số sinh viên",
                        "Họ",
                        "Tên",
                        "Ngày sinh",
                        "Email",
                        "Tài khoản",
                        "Mật khẩu"
                    }
                };
                // tạo data
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < students.Count; i++)
                {
                    data.Add(new object[] {
                        i + 1,
                        students[i].StudentNumber,
                        students[i].LastName,
                        students[i].GivenName,
                        students[i].Birthday,
                        students[i].Email,
                        students[i].Username,
                        students[i].Password
                    });
                }
                // tạo worksheet
                excel.GenerateWorksheet("Sinh viên", studentHeaders, data);
                // trả về dữ liệu dạng byte
                return excel.GetAsByteArray();
            }
        }

        public async Task<List<StudentTerm>> ImportExcelStudentTerm(byte[] file)
        {
            // Chuyển hoá dữ liệu excel từ byte về Student Term BO
            List<StudentTerm> excelTemplates = await LoadStudentTermFromExcel(file);

            // Lấy dữ liệu đã tồn tại trong database
            List<StudentTerm> studentTerms = await UOW.StudentTermRepository.List(new StudentTermFilter());
            List<Student> students = await UOW.StudentRepository.List(new StudentFilter());
            List<Term> terms = await UOW.TermRepository.List(new TermFilter());

            // Duyệt qua các student BO trong excel để gán dữ liệu excel vào dữ liệu database
            foreach (StudentTerm template in excelTemplates)
            {
                // kiểm tra dữ liệu đã tồn tại hay chưa
                StudentTerm studentTerm = studentTerms
                    .Where(st =>
                        st.SubjectName.Equals(template.SubjectName) &&
                        st.StudentNumber.Equals(template.StudentNumber)
                    )
                    .FirstOrDefault();
                Student student = students
                    .Where(s => s.StudentNumber.Equals(template.StudentNumber))
                    .FirstOrDefault();
                Term term = terms
                    .Where(t => t.SubjectName.Equals(template.SubjectName))
                    .FirstOrDefault();
                // studentTerm chưa tồn tại thì tạo mới, đã tồn tại thì cập nhật
                if (studentTerm == null)
                {
                    // studentTerm nhập láo dữ liệu không tồn tại thì next
                    if (student == null || term == null)
                    {
                        continue;
                    }
                    studentTerm = new StudentTerm
                    {
                        StudentId = student.Id,
                        StudentNumber = student.StudentNumber,
                        LastName = student.LastName,
                        GivenName = student.GivenName,
                        TermId = term.Id,
                        SubjectName = term.SubjectName,
                        IsQualified = template.IsQualified
                    };
                    studentTerms.Add(studentTerm);
                }
                else
                {
                    studentTerm.IsQualified = template.IsQualified;
                }
                
            }

            using (UOW.Begin())
            {
                try
                {
                    await UOW.StudentTermRepository.BulkMerge(studentTerms);
                    await UOW.Commit();
                }
                catch(Exception e)
                {
                    await UOW.Rollback();
                    throw e;
                }
            }
            throw new NotImplementedException();
        }

        public async Task<List<StudentTerm>> LoadStudentTermFromExcel(byte[] file)
        {
            // tạo List để chứa dữ liệu
            List<StudentTerm> excelTemplates = new List<StudentTerm>();
            using (MemoryStream ms = new MemoryStream(file))
            {
                using (var package = new ExcelPackage(ms))
                {
                    // lấy worksheet đầu tiên
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                    {
                        // bỏ qua các dòng không chứa đủ thông tin
                        string studentNumber = worksheet.Cells[i, 2].Value?.ToString().Trim();
                        string lastName = worksheet.Cells[i, 3].Value?.ToString().Trim();
                        string givenName = worksheet.Cells[i, 4].Value?.ToString().Trim();
                        string subjectName = worksheet.Cells[i, 5].Value?.ToString().Trim();
                        string isQualified = worksheet.Cells[i, 6].Value?.ToString().Trim();
                        if (string.IsNullOrEmpty(studentNumber) ||
                            string.IsNullOrEmpty(lastName) ||
                            string.IsNullOrEmpty(givenName) ||
                            string.IsNullOrEmpty(subjectName))
                            continue;
                        // thêm dòng đủ thông tin vào biến
                        StudentTerm excelTemplate = new StudentTerm
                        {
                            StudentNumber = int.Parse(studentNumber),
                            LastName = lastName,
                            GivenName = givenName,
                            SubjectName = subjectName,
                            IsQualified = !string.IsNullOrEmpty(isQualified)
                        };
                    }
                }
            }
            // trả về list dữ liệu để xử lý sau
            return excelTemplates;
        }

        public async Task<byte[]> GenerateStudentTermTemplate()
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Tạo header
                var studentHeaders = new List<string[]>
                {
                    new string[]
                    {
                        "STT",
                        "Mã số sinh viên",
                        "Họ",
                        "Tên",
                        "Tên môn học",
                        "Đủ điều kiện dự thi"
                    }
                };

                // Tạo data rỗng
                List<object[]> data = new List<object[]>();

                // Tạo worksheet từ data và header
                excel.GenerateWorksheet("Sinh viên học môn học", studentHeaders, data);

                // trả về dữ liệu dạng byte của excelPackage để xuất ra file thật
                return excel.GetAsByteArray();
            }
        }
        public async Task<byte[]> ExportStudentTerm()
        {
            throw new NotImplementedException();
        }
    }
}
