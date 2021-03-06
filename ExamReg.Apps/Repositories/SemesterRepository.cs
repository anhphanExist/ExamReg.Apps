﻿using ExamReg.Apps.Entities;
using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface ISemesterRepository
    {
        Task<Semester> Get(Guid Id);
        Task<Semester> Get(SemesterFilter filter);
        Task<bool> Create(Semester semester);
        Task<bool> Update(Semester semester);
        Task<bool> Delete(Guid Id);
        Task<int> Count(SemesterFilter filter);
        Task<List<Semester>> List(SemesterFilter filter);
        Task<bool> BulkInsert(List<Semester> semesters);
    }
    public class SemesterRepository : ISemesterRepository
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public SemesterRepository(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
        }
        public async Task<int> Count(SemesterFilter filter)
        {
            IQueryable<SemesterDAO> semesterDAOs = examRegContext.Semester.AsNoTracking();
            semesterDAOs = DynamicFilter(semesterDAOs, filter);

            return await semesterDAOs.CountAsync();
        }

        public async Task<bool> Create(Semester semester)
        {
            SemesterDAO semesterDAO = examRegContext.Semester.Where(s => s.Id.Equals(semester.Id)).FirstOrDefault();
            if(semesterDAO == null)
            {
                semesterDAO = new SemesterDAO()
                {
                    Id = semester.Id,
                    StartYear = semester.StartYear,
                    EndYear = semester.EndYear,
                    IsFirstHalf = semester.IsFirstHalf
                };

                await examRegContext.Semester.AddAsync(semesterDAO);
            }
            else
            {
                semesterDAO.StartYear = semester.StartYear;
                semesterDAO.EndYear = semester.EndYear;
                semesterDAO.IsFirstHalf = semester.IsFirstHalf;
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Guid Id)
        {
            await examRegContext.ExamRegister
            .Where(t => t.Exam.ExamPeriod.ExamProgram.SemesterId.Equals(Id))
            .DeleteFromQueryAsync();

            await examRegContext.ExamRoomExamPeriod
            .Where(t => t.ExamPeriod.ExamProgram.SemesterId.Equals(Id))
            .DeleteFromQueryAsync();

            await examRegContext.ExamPeriod
            .Where(t => t.ExamProgram.SemesterId.Equals(Id))
            .DeleteFromQueryAsync();

            await examRegContext.ExamProgram
            .Where(t => t.SemesterId.Equals(Id))
            .DeleteFromQueryAsync();

            await examRegContext.StudentTerm
            .Where(t => t.Term.SemesterId.Equals(Id))
            .DeleteFromQueryAsync();

            await examRegContext.Term
            .Where(t => t.SemesterId.Equals(Id))
            .DeleteFromQueryAsync();

            SemesterDAO semesterDAO = examRegContext.Semester
                .Where(s => s.Id.Equals(Id))
                .FirstOrDefault();

            examRegContext.Semester.Remove(semesterDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<Semester> Get(Guid Id)
        {
            IQueryable<SemesterDAO> query = examRegContext.Semester
                .AsNoTracking()
                .Where(s => s.Id.Equals(Id));

            List<Semester> list = await query.Select(s => new Semester()
            {
                Id = s.Id,
                Code = string.Format(s.StartYear + "_" + s.EndYear + "_" + (s.IsFirstHalf ? 1 : 2)),
                StartYear = s.StartYear,
                EndYear = s.EndYear,
                IsFirstHalf = s.IsFirstHalf
            }).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<Semester> Get(SemesterFilter filter)
        {
            if (filter == null) return null;

            IQueryable<SemesterDAO> query = examRegContext.Semester.AsNoTracking();
            query = DynamicFilter(query, filter);

            List<Semester> list = await query.Select(s => new Semester()
            {
                Id = s.Id,
                Code = string.Format(s.StartYear + "_" + s.EndYear + "_" + (s.IsFirstHalf ? 1 : 2)),
                StartYear = s.StartYear,
                EndYear = s.EndYear,
                IsFirstHalf = s.IsFirstHalf
            }).ToListAsync();

            return list.FirstOrDefault();
        }

        public async Task<List<Semester>> List(SemesterFilter filter)
        {
            if (filter == null) return new List<Semester>();

            IQueryable<SemesterDAO> query = examRegContext.Semester.AsNoTracking();
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);

            List<Semester> list = await query.Select(s => new Semester
            {
                Id = s.Id,
                Code = string.Format(s.StartYear + "_" + s.EndYear + "_" + (s.IsFirstHalf ? 1 : 2)),
                StartYear = s.StartYear,
                EndYear = s.EndYear,
                IsFirstHalf = s.IsFirstHalf
            }).ToListAsync();
            return list;

        }

        public async Task<bool> Update(Semester semester)
        {
            await examRegContext.Semester.Where(s => s.Id.Equals(semester.Id)).UpdateFromQueryAsync(s => new SemesterDAO()
            {
                Id = semester.Id,
                StartYear = semester.StartYear,
                EndYear = semester.EndYear,
                IsFirstHalf = semester.IsFirstHalf
            });
            return true;
        }

        public async Task<bool> BulkInsert(List<Semester> semesters)
        {
            List<SemesterDAO> semesterDAOs = semesters.Select(s => new SemesterDAO
            {
                Id = s.Id,
                StartYear = s.StartYear,
                EndYear = s.EndYear,
                IsFirstHalf = s.IsFirstHalf
            }).ToList();
            await examRegContext.Semester.BulkInsertAsync(semesterDAOs);
            return true;
        }

        private IQueryable<SemesterDAO> DynamicFilter(IQueryable<SemesterDAO> query, SemesterFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.Code != null)
            {
                string[] codeData = filter.Code.Equal.Split("_");
                query = query.Where(q => q.StartYear, new ShortFilter { Equal = short.Parse(codeData[0]) });
                query = query.Where(q => q.EndYear, new ShortFilter { Equal = short.Parse(codeData[1]) });
                query = query.Where(q => q.IsFirstHalf == (codeData[2] == "1" ? true : false));
            }
            if (filter.StartYear != null)
                query = query.Where(q => q.StartYear, filter.StartYear);
            if (filter.EndYear != null)
                query = query.Where(q => q.EndYear, filter.EndYear);
            if (filter.IsFirstHalf != null)
            query = query.Where(c => c.IsFirstHalf == filter.IsFirstHalf);

            return query;
        }

        private IQueryable<SemesterDAO> DynamicOrder(IQueryable<SemesterDAO> query, SemesterFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case SemesterOrder.Code:
                            query = query.OrderBy(q => q.StartYear);
                            break;
                        case SemesterOrder.IsFirstHalf:
                            query = query.OrderBy(q => q.IsFirstHalf);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case SemesterOrder.Code:
                            query = query.OrderByDescending(q => q.StartYear);
                            break;
                        case SemesterOrder.IsFirstHalf:
                            query = query.OrderByDescending(q => q.IsFirstHalf);
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
