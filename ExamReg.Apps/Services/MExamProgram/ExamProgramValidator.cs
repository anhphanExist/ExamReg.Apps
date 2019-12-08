using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamProgram
{
    public interface IExamProgramValidator : IServiceScoped
    {
        Task<bool> Create(ExamProgram examProgram);
        Task<bool> Update(ExamProgram examProgram);
        Task<bool> Delete(ExamProgram examProgram);
    }
    public class ExamProgramValidator : IExamProgramValidator
    {
        public enum ERROR
        {
            ExamProgramExisted,
            NotExisted,
            StringEmpty,
            StringLimited,
            SemesterCodeInValid,
            SemesterCodeEmpty
        }
        private IUOW UOW;


        public ExamProgramValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        private async Task<bool> ValidateNotExist(ExamProgram ExamProgram)
        {
            ExamProgramFilter filter = new ExamProgramFilter
            {
                Take = Int32.MaxValue,
                Name = new StringFilter { Equal = ExamProgram.Name },
                SemesterCode = new StringFilter { Equal = ExamProgram.SemesterCode }
            };

            int count = await UOW.ExamProgramRepository.Count(filter);
            if (count > 0)
            {
                ExamProgram.AddError(nameof(ExamProgramValidator), nameof(ExamProgram), ERROR.ExamProgramExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateExist(ExamProgram ExamProgram)
        {
            ExamProgramFilter filter = new ExamProgramFilter
            {
                Take = Int32.MaxValue,
                Name = new StringFilter { Equal = ExamProgram.Name },
                SemesterCode = new StringFilter { Equal = ExamProgram.SemesterCode }
            };

            int count = await UOW.ExamProgramRepository.Count(filter);
            if (count == 0)
            {
                ExamProgram.AddError(nameof(ExamProgramValidator), nameof(ExamProgram), ERROR.NotExisted);
                return false;
            }
            return true;
        }
        private bool ValidateStringLength(ExamProgram ExamProgram)
        {
            if (string.IsNullOrEmpty(ExamProgram.Name))
            {
                ExamProgram.AddError(nameof(ExamProgramValidator), nameof(ExamProgram.Name), ERROR.StringEmpty);
                return false;
            }
            else if (ExamProgram.Name != null && (ExamProgram.Name.Length > 100))
            {
                ExamProgram.AddError(nameof(ExamProgramValidator), nameof(ExamProgram.Name), ERROR.StringLimited);
                return false;
            }

            if (string.IsNullOrEmpty(ExamProgram.SemesterCode))
            {
                ExamProgram.AddError(nameof(ExamProgramValidator), nameof(ExamProgram.SemesterCode), ERROR.SemesterCodeEmpty);
                return false;
            }
            else if (ExamProgram.SemesterCode != null && Regex.IsMatch(ExamProgram.SemesterCode, @"^\d{4}_\d{4}_\d") == false)
            {
                ExamProgram.AddError(nameof(ExamProgramValidator), nameof(ExamProgram.SemesterCode), ERROR.SemesterCodeInValid);
                return false;
            }
            return true;
        }
        public async Task<bool> Create(ExamProgram examProgram)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(examProgram);
            IsValid &= ValidateStringLength(examProgram);
            return IsValid;
        }

        public async Task<bool> Delete(ExamProgram examProgram)
        {
            bool IsValid = true;
            IsValid &= await ValidateExist(examProgram);
            return IsValid;
        }

        public async Task<bool> Update(ExamProgram examProgram)
        {
            bool IsValid = true;

            IsValid &= await ValidateExist(examProgram);
            IsValid &= ValidateStringLength(examProgram);
            return IsValid;
        }
    }
}
