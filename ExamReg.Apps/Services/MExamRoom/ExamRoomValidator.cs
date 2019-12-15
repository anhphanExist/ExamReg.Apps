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
            AmphitheaterNameEmpty,
            AmphitheaterNameInvalid,
            RoomNumberEmpty,
            RoomNumberInvalid,
            ComputerNumberInvalid
        }
        private IUOW UOW;

        public ExamRoomValidator(IUOW UOW)
        {
            this.UOW = UOW;
        }

        private async Task<bool> ValidateNotExist(ExamRoom examRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Take = Int32.MaxValue,
                RoomNumber = new ShortFilter { Equal = examRoom.RoomNumber },
                AmphitheaterName = new StringFilter { Equal = examRoom.AmphitheaterName }
            };

            int count = await UOW.ExamRoomRepository.Count(filter);
            if (count > 0)
            {
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.ExamRoomExisted);
                return false;
            }
            return true;
        }

        private async Task<bool> ValidateExist(ExamRoom examRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Take = Int32.MaxValue,
                RoomNumber = new ShortFilter { Equal = examRoom.RoomNumber },
                AmphitheaterName = new StringFilter { Equal = examRoom.AmphitheaterName }
            };

            int count = await UOW.ExamRoomRepository.Count(filter);
            if (count == 0)
            {
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.NotExisted);
                return false;
            }
            return true;
        }
        public async Task<bool> ValidateId(ExamRoom examRoom)
        {
            ExamRoomFilter filter = new ExamRoomFilter
            {
                Id = new GuidFilter { Equal = examRoom.Id}
            };

            int count = await UOW.ExamRoomRepository.Count(filter);

            if (count == 0)
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.IdNotFound);

            return count == 1;
        }
        private bool ValidateStringLength(ExamRoom examRoom)
        {
            if (examRoom.RoomNumber < 0)
            {
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.RoomNumberInvalid);
                return false;
            }
            if (examRoom.ComputerNumber <= 0)
            {
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.ComputerNumberInvalid);
                return false;
            }
            if (string.IsNullOrEmpty(examRoom.AmphitheaterName))
            {
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.AmphitheaterNameEmpty);
                return false;
            }
            else if (examRoom.AmphitheaterName.Length > 100)
            {
                examRoom.AddError(nameof(ExamRoomValidator), nameof(examRoom), ERROR.AmphitheaterNameInvalid);
                return false;
            }
            return true;
        }
        public async Task<bool> Create(ExamRoom examRoom)
        {
            bool IsValid = true;
            IsValid &= await ValidateNotExist(examRoom);
            IsValid &= ValidateStringLength(examRoom);
            return IsValid;
        }

        public async Task<bool> Delete(ExamRoom examRoom)
        {
            bool IsValid = true;
            IsValid &= await ValidateId(examRoom);
            return IsValid;
        }

        public async Task<bool> Update(ExamRoom examRoom)
        {
            bool IsValid = true;

            IsValid &= await ValidateId(examRoom);
            IsValid &= ValidateStringLength(examRoom);
            return IsValid;
        }
    }
}
