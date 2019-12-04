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
        Task<Student> Get(Guid studentId);
        Task<Student> Create(Student student);
        Task<Student> Update(Student student, string newPassword);
        Task<Student> Delete(Student student);
        Task<Student> ResetPassword(Student student);
        Task<List<Student>> ImportExcelStudent(byte[] file);
        Task<byte[]> GenerateStudentTemplate();
        Task<byte[]> ExportStudent();
        Task<List<StudentTerm>> ImportExcelStudentTerm(byte[] file);
        Task<byte[]> GenerateStudentTermTemplate();
        Task<byte[]> ExportStudentTerm();
        Task RegisterExam(Guid studentId, Guid examPeriodId);
    }
    public class StudentService : IStudentService
    {
        private IUOW UOW;
        private IStudentValidator StudentValidator;
        public StudentService(IUOW UOW, IStudentValidator StudentValidator)
        {
            this.UOW = UOW;
            this.StudentValidator = StudentValidator;
        }
        public async Task<int> Count(StudentFilter filter)
        {
            return await UOW.StudentRepository.Count(filter);
        }
       
        public async Task<Student> Get(Guid studentId)
        {
            if (studentId == Guid.Empty) return null;
            return await UOW.StudentRepository.Get(studentId);
        }

        public async Task<Student> Create(Student student)
        {
            if (!await StudentValidator.Create(student))
                return student;

            using (UOW.Begin())
            {
                try
                {
                    student.Id = Guid.NewGuid();
                    await UOW.StudentRepository.Create(student);

                    var user = await UOW.UserRepository.Create(new User()
                    {
                        Id = Guid.NewGuid(),
                        Username = student.StudentNumber.ToString(),
                        Password = student.StudentNumber.ToString(),
                        StudentId = student.Id,
                        IsAdmin = false
                    });
                    
                    await UOW.Commit();
                    return await Get(student.Id);
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    student.AddError(nameof(StudentService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return student;
                }
            }
        }

        public async Task<Student> Delete(Student student)
        {
            if (!await StudentValidator.Delete(student))
                return student;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.StudentRepository.Delete(student);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    student.AddError(nameof(StudentService), nameof(Delete), CommonEnum.ErrorCode.SystemError);
                }
            }
            return student;
        }

        public async Task<List<Student>> List(StudentFilter filter)
        {
            return await UOW.StudentRepository.List(filter);
        }

        public async Task<Student> Update(Student student, string newPassword)
        {
            if (!await StudentValidator.Update(student, newPassword))
                return student;

            using (UOW.Begin())
            {
                try
                {
                    StudentFilter studentFilter = new StudentFilter
                    {
                        StudentNumber = new IntFilter { Equal = student.StudentNumber }
                    };
                    Student existingStudent = await UOW.StudentRepository.Get(studentFilter);
                    existingStudent.Password = newPassword;
                    await UOW.StudentRepository.Update(existingStudent);
                    await UOW.Commit();
                    return existingStudent;
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    student.AddError(nameof(StudentService), nameof(Update), CommonEnum.ErrorCode.SystemError);
                    return student;
                }
            }
        }

        public async Task<Student> ResetPassword(Student student)
        {
            using (UOW.Begin())
            {
                try
                {
                    StudentFilter filter = new StudentFilter
                    {
                        StudentNumber = new IntFilter { Equal = student.StudentNumber }
                    };
                    Student existingStudent = await UOW.StudentRepository.Get(filter);
                    existingStudent.Password = student.StudentNumber.ToString();
                    await UOW.StudentRepository.Update(existingStudent);
                    await UOW.Commit();
                    return existingStudent;
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    student.AddError(nameof(StudentService), nameof(ResetPassword), CommonEnum.ErrorCode.SystemError);
                    return student;
                }
            }
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
                    throw new MessageException(e);
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
                catch (Exception)
                {
                    await UOW.Rollback();
                    throw new MessageException("StudentService.ImportExcel.SystemError");
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
                        StudentLastName = student.LastName,
                        StudentGivenName = student.GivenName,
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
                    throw new MessageException(e);
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
                            StudentLastName = lastName,
                            StudentGivenName = givenName,
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
            // Lấy dữ liệu trong database
            StudentTermFilter studentTermFilter = new StudentTermFilter()
            {
                OrderBy = StudentTermOrder.StudentNumber
            };
            List<StudentTerm> studentTerms = await UOW.StudentTermRepository.List(studentTermFilter);

            // Mở excelPackage
            using (ExcelPackage excel = new ExcelPackage())
            {
                // đặt header
                var studentTermHeaders = new List<string[]>()
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
                // tạo data
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < studentTerms.Count; i++)
                {
                    data.Add(new object[] {
                        i + 1,
                        studentTerms[i].StudentNumber,
                        studentTerms[i].StudentLastName,
                        studentTerms[i].StudentGivenName,
                        studentTerms[i].SubjectName,
                        studentTerms[i].IsQualified
                    });
                }
                // tạo worksheet
                excel.GenerateWorksheet("Sinh viên - Môn học", studentTermHeaders, data);
                // trả về dữ liệu dạng byte
                return excel.GetAsByteArray();
            }
        }

        public async Task RegisterExam(Guid studentId, Guid examPeriodId, Guid termId)
        {
            // nếu sinh viên đã đăng ký thi 1 ca thi của môn đó trước đó thì phải xoá đi đăng ký lại nếu đăng ký ca thi mới của môn đó
            // đếm số lượng exam register để biết sinh viên đã đăng ký ca thi nào đó của môn học đó chưa
            int countExistedExamRegisterOfTerm = await UOW.ExamRegisterRepository.Count(new ExamRegisterFilter
            {
                StudentId = new GuidFilter { Equal = studentId },
                TermId = new GuidFilter { Equal = termId }
            });
            // nếu đã từng đăng ký thì xoá đi tạo lại
            if (countExistedExamRegisterOfTerm > 0)
            {
                await UOW.ExamRegisterRepository.Delete(studentId, examPeriodId);
            }
            
            // nếu chưa từng đăng ký thì tạo mới
            // lấy số lượng sinh viên ở trong các phòng thi hiện tại và insert sinh viên vào phòng thi có ít sinh viên nhất
            // nếu số lượng sinh viên trong tất cả các phòng đã đạt max thì báo lỗi

            // Lấy examRoomExamPeriod có số student trong đó ít nhất
            List<ExamRoomExamPeriod> exams = await UOW.ExamRoomExamPeriodRepository.List(new ExamRoomExamPeriodFilter
            {
                ExamPeriodId = new GuidFilter { Equal = examPeriodId }
            });
            // Loại exam có số lượng student đạt max, nếu list exam trống thì báo lỗi
            exams.RemoveAll(e => e.Students.Count == e.ExamRoomComputerNumber);
            if (exams.Count == 0)
                throw new MessageException("Đăng ký thất bại! Có ca thi đã đủ sinh viên đăng ký thi, vui lòng đăng kí ca thi khác");

            // Tiếp tục quy trình nếu list exam còn chỗ trống
            ExamRoomExamPeriod examToRegisterIn = exams
                .Where(exam => exam.Students.Count == exams.Select(e => e.Students.Count).Min())
                .FirstOrDefault();
            // Tạo examRegister mới
            ExamRegister examRegister = new ExamRegister
            {
                StudentId = studentId,
                ExamPeriodId = examPeriodId,
                ExamRoomId = examToRegisterIn.ExamRoomId
            };
            await UOW.ExamRegisterRepository.Create(examRegister);
            
        }
    }
}
