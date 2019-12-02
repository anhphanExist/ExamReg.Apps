using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoomExamPeriod
{
    public interface IExamRoomExamPeriodService : IServiceScoped
    {
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
        Task<ExamRoomExamPeriod> Create(ExamRoomExamPeriod examRoomExamPeriod);
        Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod);
        Task<byte[]> ExportStudent(ExamRoomExamPeriodFilter filter);
    }
    public class ExamRoomExamPeriodService : IExamRoomExamPeriodService
    {
        private IUOW UOW;
        public ExamRoomExamPeriodService(IUOW UOW)
        {
            this.UOW = UOW;
        }

        public Task<ExamRoomExamPeriod> Create(ExamRoomExamPeriod examRoomExamPeriod)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            List<ExamRoomExamPeriod> examRoomExamPeriods = await UOW.ExamRoomExamPeriodRepository.List(filter);
            Parallel.ForEach(examRoomExamPeriods, async examRoomExamPeriod =>
            {
                StudentFilter studentFilter = new StudentFilter
                {
                    ExamProgramName = new StringFilter { Equal = examRoomExamPeriod.ExamProgramName },
                    SubjectName = new StringFilter { Equal = examRoomExamPeriod.SubjectName },
                    ExamDate = new DateTimeFilter { Equal = examRoomExamPeriod.ExamDate },
                    StartHour = new ShortFilter { Equal = examRoomExamPeriod.StartHour },
                    FinishHour = new ShortFilter { Equal = examRoomExamPeriod.FinishHour },
                    ExamRoomNumber = new ShortFilter { Equal = examRoomExamPeriod.ExamRoomNumber },
                    ExamRoomAmphitheaterName = new StringFilter { Equal = examRoomExamPeriod.ExamRoomAmphitheaterName },
                    OrderBy = StudentOrder.GivenName
                };
                examRoomExamPeriod.CurrentNumberOfStudentRegistered = await UOW.StudentRepository.Count(studentFilter);
            });
            return examRoomExamPeriods;
        }

        public async Task<byte[]> ExportStudent(ExamRoomExamPeriodFilter filter)
        {
            // Lấy dữ liệu trong database
            ExamRoomExamPeriod examRoomExamPeriod = await UOW.ExamRoomExamPeriodRepository.Get(filter);
            StudentFilter studentFilter = new StudentFilter
            {
                ExamProgramName = filter.ExamProgramName,
                SubjectName = filter.SubjectName,
                ExamDate = filter.ExamDate,
                StartHour = filter.StartHour,
                FinishHour = filter.FinishHour,
                ExamRoomNumber = filter.ExamRoomNumber,
                ExamRoomAmphitheaterName = filter.ExamRoomAmphitheaterName,
                OrderBy = StudentOrder.GivenName
            };
            List<Student> studentsInExamRoomExamPeriod = await UOW.StudentRepository.List(studentFilter);
            examRoomExamPeriod.CurrentNumberOfStudentRegistered = studentsInExamRoomExamPeriod.Count();
            // Mở excelPackage
            using (ExcelPackage excel = new ExcelPackage())
            {
                // đặt header
                var examRoomExamPeriodHeaders = new List<string[]>()
                {
                    new string[]
                    {
                        "Tên kỳ thi",
                        "Tên môn thi",
                        "Mã số phòng thi",
                        "Tên giảng đường",
                        "Số lượng máy tính",
                        "Ngày thi",
                        "Giờ bắt đầu",
                        "Giờ kết thúc",
                        "Số lượng thí sinh đã đăng kí"
                    },
                    new string[]
                    {
                        examRoomExamPeriod.ExamProgramName,
                        examRoomExamPeriod.SubjectName,
                        examRoomExamPeriod.ExamRoomNumber.ToString(),
                        examRoomExamPeriod.ExamRoomAmphitheaterName,
                        examRoomExamPeriod.ExamRoomComputerNumber.ToString(),
                        examRoomExamPeriod.ExamDate.Date.ToString("dd/MM/yyyy"),
                        examRoomExamPeriod.StartHour.ToString(),
                        examRoomExamPeriod.FinishHour.ToString(),
                        examRoomExamPeriod.CurrentNumberOfStudentRegistered.ToString()
                    },
                    new string[]
                    {
                        "STT",
                        "Mã số sinh viên",
                        "Họ",
                        "Tên",
                        "Ngày sinh"
                    }
                };
                // tạo data
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < studentsInExamRoomExamPeriod.Count; i++)
                {
                    data.Add(new object[] {
                        i + 1,
                        studentsInExamRoomExamPeriod[i].StudentNumber,
                        studentsInExamRoomExamPeriod[i].LastName,
                        studentsInExamRoomExamPeriod[i].GivenName,
                        studentsInExamRoomExamPeriod[i].Birthday
                    });
                }
                // tạo worksheet
                excel.GenerateWorksheet("Sinh viên - Phòng thi - Ca thi", examRoomExamPeriodHeaders, data);
                // trả về dữ liệu dạng byte
                return excel.GetAsByteArray();
            }
        }
    }
}
