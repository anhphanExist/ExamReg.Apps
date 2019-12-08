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
                Code = new StringFilter { Equal = semester.Code }
            };

            int count = await UOW.SemesterRepository.Count(filter);
            if (count > 0)
            {
                semester.AddError(nameof(SemesterValidator), nameof(semester), ERROR.SemesterExisted);
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateExist(Semester Semester)
        {
            SemesterFilter filter = new SemesterFilter
            {
                Code = new StringFilter { Equal = Semester.Code }
            };

            int count = await UOW.SemesterRepository.Count(filter);
            if (count == 0)
            {
                Semester.AddError(nameof(SemesterValidator), nameof(Semester), ERROR.NotExisted);
                return false;
            }
            return true;
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
            IsValid &= await ValidateExist(Semester);
            return IsValid;
        }

        public async Task<bool> Update(Semester Semester)
        {
            bool IsValid = true;

            IsValid &= await ValidateExist(Semester);
            IsValid &= ValidateStringLength(Semester);
            return IsValid;
        }
    }
}
