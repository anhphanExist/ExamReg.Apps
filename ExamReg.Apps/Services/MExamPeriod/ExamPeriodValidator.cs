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
            IdNotFound,
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

        private async Task<bool> ValidateNotExist(ExamPeriod examPeriod)
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                Take = Int32.MaxValue,
                ExamDate = new DateTimeFilter { Equal = examPeriod.ExamDate }, // Kiểm tra trùng lịch
                StartHour = examPeriod.StartHour,
                FinishHour = examPeriod.FinishHour,
                SubjectName = new StringFilter { Equal = examPeriod.SubjectName },
                ExamProgramId = new GuidFilter { Equal = examPeriod.ExamProgramId }
            };

            int count = await UOW.ExamPeriodRepository.Count(filter);
            if (count > 0)
            {
                examPeriod.AddError(nameof(ExamPeriodValidator), nameof(examPeriod), ERROR.ExamPeriodExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateExist(ExamPeriod examPeriod)
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                Take = Int32.MaxValue,
                ExamDate = new DateTimeFilter { Equal = examPeriod.ExamDate },
                StartHour = examPeriod.StartHour,
                FinishHour = examPeriod.FinishHour,
                SubjectName = new StringFilter { Equal = examPeriod.SubjectName },
                ExamProgramId = new GuidFilter { Equal = examPeriod.ExamProgramId }
            };

            int count = await UOW.ExamPeriodRepository.Count(filter);
            if (count == 0)
            {
                examPeriod.AddError(nameof(ExamPeriodValidator), nameof(examPeriod), ERROR.NotExisted);
                return false;
            }
            return true;
        }
        private async Task<bool> ValidateId(ExamPeriod ExamPeriod)
        {
            ExamPeriodFilter filter = new ExamPeriodFilter
            {
                Id = new GuidFilter { Equal = ExamPeriod.Id }
            };
            int count = await UOW.ExamPeriodRepository.Count(filter);

            if (count == 0)
                ExamPeriod.AddError(nameof(ExamPeriodValidator), nameof(ExamPeriod), ERROR.IdNotFound);

            return count == 1;
        }
        private bool ValidateDateTime(ExamPeriod examPeriod)
        {
            if (examPeriod.StartHour < 0 || examPeriod.StartHour >= 24)
            {
                examPeriod.AddError(nameof(ExamPeriodValidator), nameof(examPeriod.StartHour), ERROR.StartHourInvalid);
                return false;
            }
            if (examPeriod.FinishHour < 0 || examPeriod.FinishHour >= 24)
            {
                examPeriod.AddError(nameof(ExamPeriodValidator), nameof(examPeriod.FinishHour), ERROR.FinishHourInvalid);
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
            IsValid &= await ValidateId(examPeriod);
            return IsValid;
        }

        public async Task<bool> Update(ExamPeriod examPeriod)
        {
            bool IsValid = true;

            IsValid &= await ValidateId(examPeriod);
            //IsValid &= ValidateDateTime(examPeriod);
            return IsValid;
        }
    }
}
