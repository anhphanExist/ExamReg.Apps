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
        Task<bool> Update(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<bool> Delete(ExamRoomExamPeriod ExamRoomExamPeriod);
        Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter);

        //Task<List<ExamRoomExamPeriod>> List();
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
            try
            {
                ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = examRegContext.ExamRoomExamPeriod
                    .Where(s => (s.ExamRoomId.Equals(ExamRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(ExamRoomExamPeriod.ExamPeriodId)))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.ExamRoomExamPeriod.Remove(ExamRoomExamPeriodDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<ExamRoomExamPeriod> Get(ExamRoomExamPeriodFilter filter)
        {
            IQueryable<ExamRoomExamPeriodDAO> ExamRoomExamPeriodDAOs = examRegContext.ExamRoomExamPeriod;
            ExamRoomExamPeriodDAO ExamRoomExamPeriodDAO = DynamicFilter(ExamRoomExamPeriodDAOs, filter).FirstOrDefault();

            return new ExamRoomExamPeriod()
            {
                ExamRoomId = ExamRoomExamPeriodDAO.ExamRoomId,
                RoomNumber = ExamRoomExamPeriodDAO.ExamRoom.RoomNumber,
                AmphitheaterName = ExamRoomExamPeriodDAO.ExamRoom.AmphitheaterName,
                ExamPeriodId = ExamRoomExamPeriodDAO.ExamPeriodId,
                ExamDate = ExamRoomExamPeriodDAO.ExamPeriod.ExamDate,
                StartHour = ExamRoomExamPeriodDAO.ExamPeriod.StartHour,
                FinishHour = ExamRoomExamPeriodDAO.ExamPeriod.FinishHour
            };
        }
        public async Task<List<ExamRoomExamPeriod>> List()
        {
            List<ExamRoomExamPeriod> list = await examRegContext.ExamRoomExamPeriod.Select(s => new ExamRoomExamPeriod()
            {
                ExamRoomId = s.ExamRoomId,
                RoomNumber = s.ExamRoom.RoomNumber,
                AmphitheaterName = s.ExamRoom.AmphitheaterName,
                ExamPeriodId = s.ExamPeriodId,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour
            }).ToListAsync();
            return list;
        }

        public async Task<List<ExamRoomExamPeriod>> List(ExamRoomExamPeriodFilter filter)
        {
            if (filter == null) return new List<ExamRoomExamPeriod>();

            IQueryable<ExamRoomExamPeriodDAO> query = examRegContext.ExamRoomExamPeriod;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<ExamRoomExamPeriod> list = await query.Select(s => new ExamRoomExamPeriod()
            {
                ExamRoomId = s.ExamRoomId,
                RoomNumber = s.ExamRoom.RoomNumber,
                AmphitheaterName = s.ExamRoom.AmphitheaterName,
                ExamPeriodId = s.ExamPeriodId,
                ExamDate = s.ExamPeriod.ExamDate,
                StartHour = s.ExamPeriod.StartHour,
                FinishHour = s.ExamPeriod.FinishHour

            }).ToListAsync();
            return list;

        }

        public async Task<bool> Update(ExamRoomExamPeriod ExamRoomExamPeriod)
        {
            await examRegContext.ExamRoomExamPeriod
                .Where(s => (s.ExamRoomId.Equals(ExamRoomExamPeriod.ExamRoomId) && s.ExamPeriodId.Equals(ExamRoomExamPeriod.ExamPeriodId)))
                .UpdateFromQueryAsync(s => new ExamRoomExamPeriodDAO()
                {
                    // (?)
                });

            return true;
        }
        private IQueryable<ExamRoomExamPeriodDAO> DynamicFilter(IQueryable<ExamRoomExamPeriodDAO> query, ExamRoomExamPeriodFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.RoomNumber != null)
                query = query.Where(q => q.ExamRoom.RoomNumber, filter.RoomNumber);
            if (filter.AmphitheaterName != null)
                query = query.Where(q => q.ExamRoom.AmphitheaterName, filter.AmphitheaterName);
            if (filter.ExamDate != null)
                query = query.Where(q => q.ExamPeriod.ExamDate, filter.ExamDate);
            if (filter.StartHour != null)
                query = query.Where(q => q.ExamPeriod.StartHour, filter.StartHour);
            if (filter.FinishHour != null)
                query = query.Where(q => q.ExamPeriod.FinishHour, filter.FinishHour);

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
