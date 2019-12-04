using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Services.MExamRoom
{
    public interface IExamRoomValidator : IServiceScoped
    {
        Task<bool> Create(ExamRoom examRoom);
        Task<bool> Update(ExamRoom examRoom);
        Task<bool> Delete(ExamRoom examRoom);
    }
    public class ExamRoomValidator : IExamRoomValidator
    {
        public enum ERROR
        {
            IdNotFound,
            ExamRoomExisted,
            NotExisted,
            StringEmpty,
            StringLimited
        }
        private IUOW UOW;


        public ExamRoomValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        private async Task<bool> ValidateNotExist(ExamRoom ExamRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Take = Int32.MaxValue,
                RoomNumber = new ShortFilter { Equal = ExamRoom.RoomNumber }
            };

            int count = await UOW.ExamRoomRepository.Count(filter);
            if (count > 0)
            {
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom.RoomNumber), ERROR.ExamRoomExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateId(ExamRoom ExamRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                OrderBy = ExamRoomOrder.RoomNumber,
                OrderType = OrderType.ASC
            };
            int count = await UOW.ExamRoomRepository.Count(filter);

            if (count == 0)
                ExamRoom.AddError(nameof(ExamRoomValidator), nameof(ExamRoom.Id), ERROR.IdNotFound);

            return count == 1;
        }
        //private bool ValidateStringLength
        public async Task<bool> Create(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Delete(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ExamRoom examRoom)
        {
            throw new NotImplementedException();
        }
    }
}
