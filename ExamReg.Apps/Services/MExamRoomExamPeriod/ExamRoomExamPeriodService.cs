using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoomExamPeriod
{
    public interface IExamRoomExamPeriodService : IServiceScoped
    {
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
        Task<ExamRoomExamPeriod> Create(ExamRoomExamPeriod examRoomExamPeriod);
        Task<ExamRoomExamPeriod> Update(ExamRoomExamPeriod examRoomExamPeriod);
        Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod);
        Task<byte[]> Export(ExamRoomExamPeriodFilter filter);
    }
    public class ExamRoomExamPeriodService : IExamRoomExamPeriodService
    {
        private IUOW UOW;
        public ExamRoomExamPeriodService(IUOW UOW)
        {
            this.UOW = UOW;
        }

        public Task<ExamRoomExamPeriod> Create(ExamRoomExamPeriod examRoomExamPeriod)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoomExamPeriod> Delete(ExamRoomExamPeriod examRoomExamPeriod)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoomExamPeriod> Update(ExamRoomExamPeriod examRoomExamPeriod)
        {
            throw new NotImplementedException();
        }

        public Task<byte[]> Export(ExamRoomExamPeriodFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
