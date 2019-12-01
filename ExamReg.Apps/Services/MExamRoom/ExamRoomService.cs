using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoom
{
    public interface IExamRoomService : IServiceScoped
    {
        Task<int> Count(ExamRoomFilter filter);
        Task<List<ExamRoom>> List(ExamRoomFilter filter);
        Task<ExamRoom> Create(ExamRoom examRoom);
        Task<ExamRoom> Update(ExamRoom examRoom);
        Task<ExamRoom> Delete(ExamRoom examRoom);
    }
    public class ExamRoomService : IExamRoomService
    {
        public Task<int> Count(ExamRoomFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoom> Create(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoom> Delete(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public Task<List<ExamRoom>> List(ExamRoomFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<ExamRoom> Update(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }
    }
}
