using ExamReg.Apps.Entities;
using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IExamRoomExamPeriodRepository
    {
        Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter);
        Task<bool> Create(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<bool> Delete(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);
    }
    public class ExamRoomExamPeriodRepository : IExamRoomExamPeriodRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public ExamRoomExamPeriodRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<bool> Create(ExamRoomExamPeriod examRoomExamPeriod)
        {
            ExamRoomExamPeriodDAO examRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(examRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(examRoomExamPeriod.ExamPeriodId)))
                .FirstOrDefault();
            if (examRoomExamPeriodDAO == null)
            {
                examRoomExamPeriodDAO = new ExamRoomExamPeriodDAO()
                {
                    ExamRoomId = examRoomExamPeriod.ExamRoomId,
                    ExamPeriodId = examRoomExamPeriod.ExamPeriodId
                };

                await examRegContext.ExamRoomExamPeriod.AddAsync(examRoomExamPeriodDAO);
            }
            else
            {
                examRoomExamPeriodDAO.ExamRoomId = examRoomExamPeriod.ExamRoomId;
                examRoomExamPeriodDAO.ExamPeriodId = examRoomExamPeriod.ExamPeriodId;
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(ExamRoomExamPeriod ExamRoomExamPeriod)
        {
            ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(ExamRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(ExamRoomExamPeriod.ExamPeriodId)))
                .FirstOrDefault();

            examRegContext.ExamRoomExamPeriod.Remove(ExamRoomExamPeriodDAO);
            await examRegContext.SaveChangesAsync();
            return true;
            
        }

        public async Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter)
        {
            IQueryable<ExamRoomExamPeriodDAO> examRoomExamPeriodDAOs = examRegContext.ExamRoomExamPeriod.AsNoTracking();
            ExamRoomExamPeriodDAO examRoomExamPeriodDAO = DynamicFilter(examRoomExamPeriodDAOs, filter).FirstOrDefault();
            if (examRoomExamPeriodDAO == null)
                return null;
            return new ExamRoomExamPeriod()
            {
                ExamRoomId = examRoomExamPeriodDAO.ExamRoomId,
                ExamPeriodId = examRoomExamPeriodDAO.ExamPeriodId,
                ExamProgramName = examRoomExamPeriodDAO.ExamPeriod.ExamProgram.Name,
                ExamDate = examRoomExamPeriodDAO.ExamPeriod.ExamDate,
                StartHour = examRoomExamPeriodDAO.ExamPeriod.StartHour,
                FinishHour = examRoomExamPeriodDAO.ExamPeriod.FinishHour,
                SubjectName = examRoomExamPeriodDAO.ExamPeriod.Term.SubjectName,
                ExamRoomNumber = examRoomExamPeriodDAO.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = examRoomExamPeriodDAO.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = examRoomExamPeriodDAO.ExamRoom.ComputerNumber
            };
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return new List<ExamRoomExamPeriod>();

            IQueryable<ExamRoomExamPeriodDAO> query = examRegContext.ExamRoomExamPeriod.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<ExamRoomExamPeriod> list = await query.Select(s => new ExamRoomExamPeriod()
            {
                ExamRoomId = s.ExamRoomId,
                ExamPeriodId = s.ExamPeriodId,
                ExamProgramName = s.ExamPeriod.ExamProgram.Name,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour,
                SubjectName = s.ExamPeriod.Term.SubjectName,
                ExamRoomNumber = s.ExamRoom.RoomNumber,
                ExamRoomAmphitheaterName = s.ExamRoom.AmphitheaterName,
                ExamRoomComputerNumber = s.ExamRoom.ComputerNumber
            }).ToListAsync();
            return list;
        }

        private IQueryable<ExamRoomExamPeriodDAO> DynamicFilter(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.ExamProgramName != null)
                query = query.Where(q => q.ExamPeriod.ExamProgram.Name, filter.ExamProgramName);
            if (filter.SubjectName != null)
                query = query.Where(q => q.ExamPeriod.Term.SubjectName, filter.SubjectName);
            if (filter.ExamDate != null)
                query = query.Where(q => q.ExamPeriod.ExamDate, filter.ExamDate);
            if (filter.StartHour != null)
                query = query.Where(q => q.ExamPeriod.StartHour, filter.ExamDate);
            if (filter.FinishHour != null)
                query = query.Where(q => q.ExamPeriod.FinishHour, filter.ExamDate);
            if (filter.ExamRoomNumber != null)
                query = query.Where(q => q.ExamRoom.RoomNumber, filter.ExamRoomNumber);
            if (filter.ExamRoomAmphitheaterName != null)
                query = query.Where(q => q.ExamRoom.AmphitheaterName, filter.ExamRoomAmphitheaterName);
            return query;
        }
        private IQueryable<ExamRoomExamPeriodDAO> DynamicOrder(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case ExamRoomExamPeriodOrder.AmphitheaterName:
                            query = query.OrderBy(q => q.ExamRoom.AmphitheaterName);
                            break;
                        case ExamRoomExamPeriodOrder.RoomNumber:
                            query = query.OrderBy(q => q.ExamRoom.RoomNumber);
                            break;
                        case ExamRoomExamPeriodOrder.ExamDate:
                            query = query.OrderBy(q => q.ExamPeriod.ExamDate);
                            break;
                        case ExamRoomExamPeriodOrder.StartHour:
                            query = query.OrderBy(q => q.ExamPeriod.StartHour);
                            break;
                        case ExamRoomExamPeriodOrder.FinishHour:
                            query = query.OrderBy(q => q.ExamPeriod.FinishHour);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case ExamRoomExamPeriodOrder.AmphitheaterName:
                            query = query.OrderByDescending(q => q.ExamRoom.AmphitheaterName);
                            break;
                        case ExamRoomExamPeriodOrder.RoomNumber:
                            query = query.OrderByDescending(q => q.ExamRoom.RoomNumber);
                            break;
                        case ExamRoomExamPeriodOrder.ExamDate:
                            query = query.OrderByDescending(q => q.ExamPeriod.ExamDate);
                            break;
                        case ExamRoomExamPeriodOrder.StartHour:
                            query = query.OrderByDescending(q => q.ExamPeriod.StartHour);
                            break;
                        case ExamRoomExamPeriodOrder.FinishHour:
                            query = query.OrderByDescending(q => q.ExamPeriod.FinishHour);
                            break;
                        default:
                            query = query.OrderByDescending(q => q.CX);
                            break;
                    }
                    break;
                default:
                    query = query.OrderBy(q => q.CX);
                    break;
            }
            return query.Skip(filter.Skip).Take(filter.Take);
        }
    }
}
