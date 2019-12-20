using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using OfficeOpenXml;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoomExamPeriod
{
    public interface IExamRoomExamPeriodService : IServiceScoped
    {
        Task<ExamRoomExamPeriod> Get(Guid examPeriodId, Guid examRoomId);
        Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter);
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
        Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod);
        Task<byte[]> ExportStudent(ExamRoomExamPeriodFilter filter);
        Task<MemoryStream> PrintExamRegisterResult(ExamRoomExamPeriodFilter filter);
    }
    public class ExamRoomExamPeriodService : IExamRoomExamPeriodService
    {
        private IUOW UOW;
        public ExamRoomExamPeriodService(IUOW UOW)
        {
            this.UOW = UOW;
        }
        public async Task<ExamRoomExamPeriod> Get(Guid examRoomId, Guid examPeriodId)
        {
            ExamRoomExamPeriodFilter filter = new ExamRoomExamPeriodFilter
            {
                ExamPeriodId = new GuidFilter { Equal = examPeriodId},
                ExamRoomId = new GuidFilter { Equal = examRoomId}

            };
            return await UOW.ExamRoomExamPeriodRepository.Get(filter);
        }

        public async Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter)
        {
            return await UOW.ExamRoomExamPeriodRepository.Get(filter);
        }
     
        public async Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod)
        {
            using (UOW.Begin())
            {
                try
                {
                    await UOW.ExamRoomExamPeriodRepository.Delete(examRoomExamPeriod.ExamRoomId, examRoomExamPeriod.ExamPeriodId);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    examRoomExamPeriod.AddError(nameof(ExamRoomExamPeriodService), nameof(Delete), CommonEnum.ErrorCode.SystemError);
                }
            }
            return examRoomExamPeriod;
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
                        examRoomExamPeriod.ExamDate.ToString("dd-MM-yyyy"),
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
                        examRoomExamPeriod.Students[i].Birthday.ToString("dd-MM-yyyy")
                    });
                }
                // tạo worksheet
                excel.GenerateWorksheet("Phòng thi - Ca thi", examRoomExamPeriodHeaders, data);
                // trả về dữ liệu dạng byte
                return excel.GetAsByteArray();
            }
        }

        public async Task<MemoryStream> PrintExamRegisterResult(ExamRoomExamPeriodFilter filter)
        {
            // Lấy dữ liệu trong database
            List<ExamRoomExamPeriod> examRoomExamPeriods = await UOW.ExamRoomExamPeriodRepository.List(filter);
            Student student = await UOW.StudentRepository.Get(new StudentFilter
            {
                StudentNumber = new IntFilter { Equal = filter.StudentNumber }
            });

            using (WordDocument document = new WordDocument())
            {
                //Adds new section to the document
                IWSection section = document.AddSection();
                //Adds new paragraph to the section
                IWParagraph titleParagraph = section.AddParagraph();
                //Adds new text to the paragraph
                IWTextRange textRange = titleParagraph.AppendText("Kết quả đăng ký dự thi");
                textRange.CharacterFormat.FontSize = 20;
                textRange.CharacterFormat.Bold = true;
                textRange.CharacterFormat.TextColor = Color.DarkBlue;
                titleParagraph.ParagraphFormat.AfterSpacing = 18f;
                titleParagraph.ParagraphFormat.BeforeSpacing = 18f;
                titleParagraph.ParagraphFormat.FirstLineIndent = 10f;
                titleParagraph.ParagraphFormat.LineSpacing = 10f;
                titleParagraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Center;

                IWParagraph studentNumberParagraph = section.AddParagraph();
                studentNumberParagraph.AppendText("Mã số sinh viên: " + student.StudentNumber.ToString());
                IWParagraph studentNameParagraph = section.AddParagraph();
                studentNameParagraph.AppendText("Họ và tên: " + student.LastName + " " + student.GivenName);
                IWParagraph studentBirthdayParagraph = section.AddParagraph();
                studentBirthdayParagraph.AppendText("Ngày sinh: " + student.Birthday.ToString("dd-MM-yyyy"));
                IWParagraph studentEmailParagraph = section.AddParagraph();
                studentEmailParagraph.AppendText("Email: " + student.Email);


                string[] tableHeaders = new string[]
                {
                    "STT",
                    "Tên môn học",
                    "Ngày thi",
                    "Giờ bắt đầu",
                    "Giờ kết thúc",
                    "Phòng thi",
                    "Giảng đường"
                };
                //Adds a new table into Word document
                IWTable table = section.AddTable();
                //Specifies the total number of rows & columns
                table.ResetCells(examRoomExamPeriods.Count + 1, tableHeaders.Length);
                table.TableFormat.HorizontalAlignment = RowAlignment.Center;
                for (int i = 0; i < tableHeaders.Length; i++)
                {
                    textRange = table[0, i].AddParagraph().AppendText(tableHeaders[i]);
                    textRange.CharacterFormat.Bold = true;
                }

                for (int i = 1; i < table.Rows.Count; i++)
                {
                    table[i, 0].AddParagraph().AppendText(i.ToString());
                    table[i, 1].AddParagraph().AppendText(examRoomExamPeriods[i - 1].SubjectName);
                    table[i, 2].AddParagraph().AppendText(examRoomExamPeriods[i - 1].ExamDate.ToString("dd-MM-yyyy"));
                    table[i, 3].AddParagraph().AppendText(examRoomExamPeriods[i - 1].StartHour.ToString());
                    table[i, 4].AddParagraph().AppendText(examRoomExamPeriods[i - 1].FinishHour.ToString());
                    table[i, 5].AddParagraph().AppendText(examRoomExamPeriods[i - 1].ExamRoomNumber.ToString());
                    table[i, 6].AddParagraph().AppendText(examRoomExamPeriods[i - 1].ExamRoomAmphitheaterName);
                }


                MemoryStream stream = new MemoryStream();
                document.Save(stream, FormatType.Docx);
                //Closes the Word document
                document.Close();
                stream.Position = 0;
                //Download Word document in the browser
                return stream;
            }
        }
    }
}
