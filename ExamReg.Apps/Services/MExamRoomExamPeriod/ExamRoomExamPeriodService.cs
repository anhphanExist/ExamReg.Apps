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
        Task<ExamRoomExamPeriod> Create(Guid examPeriodId);
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

        public Task<ExamRoomExamPeriod> Create(Guid examPeriodId)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            return await UOW.ExamRoomExamPeriodRepository.List(filter);
        }

        public async Task<byte[]> ExportStudent(ExamRoomExamPeriodFilter filter)
        {
            // Lấy dữ liệu trong database
            ExamRoomExamPeriod examRoomExamPeriod = await UOW.ExamRoomExamPeriodRepository.Get(filter);
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
                        examRoomExamPeriod.Students.Count.ToString()
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
                for (int i = 0; i < examRoomExamPeriod.Students.Count; i++)
                {
                    data.Add(new object[] {
                        i + 1,
                        examRoomExamPeriod.Students[i].StudentNumber,
                        examRoomExamPeriod.Students[i].LastName,
                        examRoomExamPeriod.Students[i].GivenName,
                        examRoomExamPeriod.Students[i].Birthday
                    });
                }
                // tạo worksheet
                excel.GenerateWorksheet("Phòng thi - Ca thi", examRoomExamPeriodHeaders, data);
                // trả về dữ liệu dạng byte
                return excel.GetAsByteArray();
            }
        }
    }
}
