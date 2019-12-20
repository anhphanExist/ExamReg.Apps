using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MTerm
{
    public interface ITermService : IServiceScoped
    {
        Task<int> Count(TermFilter filter);
        Task<List<Term>> List(TermFilter filter);
        Task<Term> Create(Term term);
        Task<Term> Update(Term term);
        Task<Term> Delete(Term term);
        Task<bool> ImportExcel(byte[] file);
        Task<byte[]> GenerateTemplate();
        Task<byte[]> Export();
    }

    public class TermService : ITermService
    {
        private IUOW UOW;
        private ITermValidator TermValidator;

        public TermService(IUOW UOW, ITermValidator TermValidator)
        {
            this.UOW = UOW;
            this.TermValidator = TermValidator;
        }

        public async Task<int> Count(TermFilter filter)
        {
            return await UOW.TermRepository.Count(filter);
        }

        public async Task<List<Term>> List(TermFilter filter)
        {
            return await UOW.TermRepository.List(filter);
        }
        public async Task<Term> Get(Guid termId)
        {
            return await UOW.TermRepository.Get(termId);
        }
        public async Task<Term> GetSemesterId(Term term)
        {
            SemesterFilter filter = new SemesterFilter
            {
                Code = new StringFilter { Equal = term.SemesterCode }
            };

            Semester semester = await UOW.SemesterRepository.Get(filter);
            term.SemesterId = semester.Id;

            return term;
        }

        public async Task<Term> Create(Term term)
        {
            if (!await TermValidator.Create(term))
                return term;

            using (UOW.Begin())
            {
                try
                {
                    term.Id = Guid.NewGuid();

                    term = await GetSemesterId(term);

                    await UOW.TermRepository.Create(term);
                    await UOW.Commit();
                    return term;
                }
                catch(Exception e)
                {
                    await UOW.Rollback();
                    term.AddError(nameof(TermService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return term;
                }
            }
        }

        public async Task<Term> Update(Term term)
        {
            if (!await TermValidator.Update(term))
                return term;

            using (UOW.Begin())
            {
                try
                {
                    term = await GetSemesterId(term);

                    await UOW.TermRepository.Update(term);
                    await UOW.Commit();
                    return term;
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    term.AddError(nameof(TermService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                    return term;
                }
            }
        }

        public async Task<Term> Delete(Term term)
        {
            if (!await TermValidator.Delete(term))
                return term;

            using (UOW.Begin())
            {
                try
                {
                    await UOW.TermRepository.Delete(term.Id);
                    await UOW.Commit();
                }
                catch (Exception e)
                {
                    await UOW.Rollback();
                    term.AddError(nameof(TermService), nameof(Create), CommonEnum.ErrorCode.SystemError);
                }
            }
            return term;
        }

        public async Task<bool> ImportExcel(byte[] file)
        {
            // Chuyển hoá dữ liệu excel từ byte về Term BO
            List<Term> excelTemplates = await LoadFromExcel(file);
            if (!(excelTemplates.Count > 0))
                return false;

            // Lấy list dữ liệu đã tồn tại trong database
            List<Term> terms = await UOW.TermRepository.List(new TermFilter());
            List<Semester> semesters = await UOW.SemesterRepository.List(new SemesterFilter());

            // Dữ liệu thêm mới
            List<Term> newTerms = new List<Term>();
            List<Semester> newSemesters = new List<Semester>();

            // Duyệt qua các term BO trong excel để gán dữ liệu excel vào dữ liệu database
            foreach (Term template in excelTemplates)
            {
                // Kiểm tra dữ liệu đã tồn tại hay chưa
                Term term = terms.Where(s => s.SubjectName.Equals(template.SubjectName)).FirstOrDefault();
                Semester semester = semesters.Where(s => s.Code.Equals(template.SemesterCode)).FirstOrDefault();
                // Nếu chưa tồn tại thì thêm mới cả semester và term, nếu đã tồn tại thì không làm gì cả
                try
                {
                    if (semester == null && term == null)
                    {
                        string[] codeData = template.SemesterCode.Split("_");
                        semester = new Semester
                        {
                            Id = new Guid(),
                            Code = template.SemesterCode,
                            StartYear = short.Parse(codeData[0]),
                            EndYear = short.Parse(codeData[1]),
                            IsFirstHalf = codeData[2].Equals("1") ? true : false
                        };
                        newSemesters.Add(semester);
                        term = new Term
                        {
                            Id = new Guid(),
                            SubjectName = template.SubjectName,
                            SemesterCode = semester.Code,
                            SemesterId = semester.Id
                        };
                        
                        newTerms.Add(term);
                    }
                    else if (term == null)
                    {
                        term = new Term
                        {
                            Id = new Guid(),
                            SubjectName = template.SubjectName,
                            SemesterCode = semester.Code,
                            SemesterId = semester.Id
                        };
                        newTerms.Add(term);
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            // Tiến hành giao dịch với database thông qua repo
            using (UOW.Begin())
            {
                try
                {
                    // Merge dữ liệu vào repository
                    var resTerms = await UOW.TermRepository.BulkInsert(newTerms);
                    var resSemesters = await UOW.SemesterRepository.BulkInsert(newSemesters);
                    await UOW.Commit();
                    return true;
                }
                catch (Exception)
                {
                    await UOW.Rollback();
                    return false;
                }
            }
        }

        private async Task<List<Term>> LoadFromExcel(byte[] file)
        {
            // tạo List sinh viên để chứa dữ liệu
            List<Term> excelTemplates = new List<Term>();
            using (MemoryStream ms = new MemoryStream(file))
            {
                using (var package = new ExcelPackage(ms))
                {
                    try
                    {
                        // lấy worksheet đầu tiên
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            // bỏ qua các dòng không chứa đủ thông tin
                            string subjectName = worksheet.Cells[i, 2].Value?.ToString().Trim();
                            string semesterCode = worksheet.Cells[i, 3].Value?.ToString().Trim();
                            if (string.IsNullOrEmpty(subjectName) ||
                                string.IsNullOrEmpty(semesterCode))
                                continue;
                            // thêm dòng đủ thông tin vào biến
                            Term excelTemplate = new Term
                            {
                                SubjectName = subjectName,
                                SemesterCode = semesterCode
                            };
                            excelTemplates.Add(excelTemplate);
                        }
                    }
                    catch (Exception e)
                    {
                        return new List<Term>();
                    }
                }
            }
            // trả về list dữ liệu sinh viên để xử lý sau
            return excelTemplates;
        }

        public async Task<byte[]> GenerateTemplate()
        {
            using (ExcelPackage excel = new ExcelPackage())
            {
                // Tạo header
                var termHeaders = new List<string[]>
                {
                    new string[]
                    {
                        "STT",
                        "Tên môn học",
                        "Mã kì học"
                    }
                };

                // Tạo data rỗng
                List<object[]> data = new List<object[]>();

                // Tạo worksheet từ data và header
                excel.GenerateWorksheet("Môn học", termHeaders, data);

                // trả về dữ liệu dạng byte của excelPackage để xuất ra file thật
                return excel.GetAsByteArray();
            }
        }

        public async Task<byte[]> Export()
        {
            // Lấy dữ liệu trong database
            TermFilter termFilter = new TermFilter
            {
                OrderBy = TermOrder.SubjectName
            };
            List<Term> terms = await UOW.TermRepository.List(termFilter);

            // Mở excelPackage
            using (ExcelPackage excel = new ExcelPackage())
            {
                // đặt header
                var termHeaders = new List<string[]>()
                {
                    new string[]
                    {
                        "STT",
                        "Tên môn học",
                        "Mã kì học"
                    }
                };
                // tạo data
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < terms.Count; i++)
                {
                    data.Add(new object[] {
                        i + 1,
                        terms[i].SubjectName,
                        terms[i].SemesterCode
                    }); ;
                }
                // tạo worksheet
                excel.GenerateWorksheet("Môn học", termHeaders, data);
                // trả về dữ liệu dạng byte
                return excel.GetAsByteArray();
            }
        }
    }
}
