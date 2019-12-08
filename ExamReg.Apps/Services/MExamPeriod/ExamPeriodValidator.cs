using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamPeriod
{
    public interface IExamPeriodValidator : IServiceScoped
    {
        Task<bool> Create(ExamPeriod examPeriod);
        Task<bool> Update(ExamPeriod examPeriod);
        Task<bool> Delete(ExamPeriod examPeriod);
    }
    public class ExamPeriodValidator : IExamPeriodValidator
    {
        public enum ERROR
        {
            ExamPeriodExisted,
            NotExisted,
            DateEmpty,
            StringEmpty,
            StartHourInvalid,
            FinishHourInvalid
        }
        private IUOW UOW;

        public ExamPeriodValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        private async Task<bool> ValidateNotExist(ExamPeriod ExamPeriod)
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                Take = Int32.MaxValue,
                ExamDate = new DateTimeFilter { Equal = ExamPeriod.ExamDate },
                SubjectName = new StringFilter { Equal = ExamPeriod.SubjectName },
                ExamProgramId = new GuidFilter { Equal = ExamPeriod.ExamProgramId }
            };

            int count = await UOW.ExamPeriodRepository.Count(filter);
            if (count > 0)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod), ERROR.ExamPeriodExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateExist(ExamPeriod ExamPeriod)
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                Take = Int32.MaxValue,
                ExamDate = new DateTimeFilter { Equal = ExamPeriod.ExamDate },
                SubjectName = new StringFilter { Equal = ExamPeriod.SubjectName },
                ExamProgramId = new GuidFilter { Equal = ExamPeriod.ExamProgramId }
            };

            int count = await UOW.ExamPeriodRepository.Count(filter);
            if (count == 0)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod), ERROR.NotExisted);
                return false;
            }
            return true;
        }
        private bool ValidateDateTime(ExamPeriod ExamPeriod)
        {
            if (ExamPeriod.ExamDate == null)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod), ERROR.DateEmpty);
                return false;
            }
            if (ExamPeriod.StartHour.ToString() == null)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod.StartHour), ERROR.StringEmpty);
                return false;
            }
            else if (ExamPeriod.StartHour < 0 || ExamPeriod.StartHour.ToString().Length > 2)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod.StartHour), ERROR.StartHourInvalid);
                return false;
            }
            if (ExamPeriod.FinishHour.ToString() == null)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod.FinishHour), ERROR.StringEmpty);
                return false;
            }
            else if (ExamPeriod.FinishHour < 0 || ExamPeriod.FinishHour.ToString().Length > 2)
            {
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod.FinishHour), ERROR.FinishHourInvalid);
                return false;
            }
            return true;
        }
        public async Task<bool> Create(ExamPeriod examPeriod)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(examPeriod);
            IsValid &= ValidateDateTime(examPeriod);
            return IsValid;
        }

        public async Task<bool> Delete(ExamPeriod examPeriod)
        {
            bool IsValid = true;
            IsValid &= await ValidateExist(examPeriod);
            return IsValid;
        }

        public async Task<bool> Update(ExamPeriod examPeriod)
        {
            bool IsValid = true;

            IsValid &= await ValidateExist(examPeriod);
            IsValid &= ValidateDateTime(examPeriod);
            return IsValid;
        }
    }
}
