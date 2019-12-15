using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MSemester
{
    public interface ISemesterValidator : IServiceScoped
    {
        Task<bool> Create(Semester Semester);
        Task<bool> Update(Semester Semester);
        Task<bool> Delete(Semester Semester);
    }

    public class SemesterValidator : ISemesterValidator
    {
        public enum ERROR
        {
            IdNotFound,
            SemesterExisted,
            NotExisted,
            StringEmpty,
            StartYearInvalid,
            EndYearInvalid
        }
        private IUOW UOW;


        public SemesterValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        private async Task<bool> ValidateNotExist(Semester semester)
        {
            SemesterFilter filter = new SemesterFilter
            {
                StartYear = new ShortFilter { Equal = semester.StartYear },
                EndYear = new ShortFilter { Equal = semester.EndYear },
                IsFirstHalf = semester.IsFirstHalf
            };

            int count = await UOW.SemesterRepository.Count(filter);
            if (count > 0)
            {
                semester.AddError(nameof(SemesterValidator), nameof(semester), ERROR.SemesterExisted);
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateId(Semester Semester)
        {
            SemesterFilter filter = new SemesterFilter
            {
                Id = new GuidFilter { Equal = Semester.Id }
            };

            int count = await UOW.SemesterRepository.Count(filter);
            if (count == 0)
                Semester.AddError(nameof(SemesterValidator), nameof(Semester), ERROR.IdNotFound);

            return count == 1;
        }

        private bool ValidateStringLength(Semester Semester)
        {
            if (Semester.StartYear < 0 || Semester.StartYear.ToString().Length > 4)
            {
                Semester.AddError(nameof(SemesterValidator), nameof(Semester.StartYear), ERROR.StartYearInvalid);
                return false;
            }
            if (Semester.EndYear < 0 || Semester.EndYear.ToString().Length > 4)
            {
                Semester.AddError(nameof(SemesterValidator), nameof(Semester.EndYear), ERROR.EndYearInvalid);
                return false;
            }
            return true;
        }

        public async Task<bool> Create(Semester Semester)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(Semester);
            IsValid &= ValidateStringLength(Semester);
            return IsValid;
        }

        public async Task<bool> Delete(Semester Semester)
        {
            bool IsValid = true;
            IsValid &= await ValidateId(Semester);
            return IsValid;
        }

        public async Task<bool> Update(Semester Semester)
        {
            bool IsValid = true;

            IsValid &= await ValidateId(Semester);
            IsValid &= ValidateStringLength(Semester);
            return IsValid;
        }
    }
}
