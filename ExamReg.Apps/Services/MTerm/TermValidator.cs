using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MTerm
{
    public interface ITermValidator : IServiceScoped
    {
        Task<bool> Create(Term term);
        Task<bool> Update(Term term);
        Task<bool> Delete(Term term);
    }

    public class TermValidator : ITermValidator
    {
        public enum ERROR
        {
            IdNotFound,
            TermExisted,
            NotExisted,
            StringEmpty,
            StringLimited,
            SemesterCodeEmpty,
            SemesterCodeInValid
        }
        private IUOW UOW;


        public TermValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }
        private async Task<bool> ValidateNotExist(Term Term)
        {
            TermFilter filter = new TermFilter
            {
                Take = Int32.MaxValue,
                SubjectName = new StringFilter { Equal = Term.SubjectName },
                SemesterCode = new StringFilter { Equal = Term.SemesterCode }
            };

            int count = await UOW.TermRepository.Count(filter);
            if (count > 0)
            {
                Term.AddError(nameof(TermValidator), nameof(Term), ERROR.TermExisted);
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateExist(Term Term)
        {
            TermFilter filter = new TermFilter
            {
                Take = Int32.MaxValue,
                SubjectName = new StringFilter { Equal = Term.SubjectName },
                SemesterCode = new StringFilter { Equal = Term.SemesterCode }
            };

            int count = await UOW.TermRepository.Count(filter);
            if (count == 0)
            {
                Term.AddError(nameof(TermValidator), nameof(Term), ERROR.NotExisted);
                return false;
            }
            return true;
        }

        /*private async Task<bool> ValidateId(Term Term)
        {
            TermFilter filter = new TermFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                OrderBy = TermOrder.SubjectName,
                OrderType = OrderType.ASC
            };
            int count = await UOW.TermRepository.Count(filter);

            if (count == 0)
                Term.AddError(nameof(TermValidator), nameof(Term.Id), ERROR.IdNotFound);

            return count == 1;
        }*/

        private bool ValidateStringLength(Term term)
        {
            if (string.IsNullOrEmpty(term.SubjectName))
            {
                term.AddError(nameof(TermValidator), nameof(term), ERROR.StringEmpty);
                return false;
            }
            else if (term.SubjectName != null && (term.SubjectName.Length > 100))
            {
                term.AddError(nameof(TermValidator), nameof(term), ERROR.StringLimited);
                return false;
            }
            if (string.IsNullOrEmpty(term.SemesterCode))
            {
                term.AddError(nameof(TermValidator), nameof(term.SemesterCode), ERROR.SemesterCodeEmpty);
                return false;
            }
            else if (term.SemesterCode != null && Regex.IsMatch(term.SemesterCode, @"^\d{4}_\d{4}_\d") == false)
            {
                term.AddError(nameof(TermValidator), nameof(term.SemesterCode), ERROR.SemesterCodeInValid);
                return false;
            }
            return true;
        }

        public async Task<bool> Create(Term term)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(term);
            IsValid &= ValidateStringLength(term);
            return IsValid;
        }

        public async Task<bool> Delete(Term term)
        {
            bool IsValid = true;
            IsValid &= await ValidateExist(term);
            return IsValid;
        }

        public async Task<bool> Update(Term term)
        {
            bool IsValid = true;

            IsValid &= await ValidateExist(term);
            IsValid &= ValidateStringLength(term);
            return IsValid;
        }
    }
}
