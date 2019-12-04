using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
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
            StringLimited
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

        private async Task<bool> ValidateId(Term Term)
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
        }

        public async Task<bool> Create(Term term)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(Term term)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Term term)
        {
            throw new NotImplementedException();
        }
    }
}
