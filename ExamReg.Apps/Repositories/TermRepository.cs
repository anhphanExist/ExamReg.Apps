using ExamReg.Apps.Entities;
using ExamReg.Apps.Common;
using ExamReg.Apps.Entities;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface ITermRrepository
    {
        Task<Term> Get(Guid Id);
        Task<Term> Get(TermFilter filter);
        Task<bool> Create(Term term);
        Task<bool> Update(Term term);
        Task<bool> Delete(Term term);
        Task<int> Count(TermFilter filter);
        Task<List<Term>> List(TermFilter filter);
    }
    public class TermRepository : ITermRrepository
    {
        private ExamRegContext examRegContext;
        public TermRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<int> Count(TermFilter filter)
        {
            IQueryable<TermDAO> termDAOs = examRegContext.Term;
            termDAOs = DynamicFilter(termDAOs, filter);
            return await termDAOs.CountAsync();
        }

        public async Task<bool> Create(Term term)
        {
            TermDAO termDAO = new TermDAO()
            {
                Id = term.Id,
                SubjectName = term.SubjectName,
                SemesterId = term.SemesterId
            };
            await examRegContext.Term.AddAsync(termDAO);
            await examRegContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(Term term)
        {
            try
            {
                // ràng buộc (?)
                TermDAO termDAO = examRegContext.Term
                    .Where(s => s.Id.Equals(term.Id))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.Term.Remove(termDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Term> Get(Guid Id)
        {
            TermDAO termDAO = examRegContext.Term
                .Where(t => t.Id.Equals(Id))
                .AsNoTracking()
                .FirstOrDefault();
            return new Term()
            {
                Id = termDAO.Id,
                SubjectName = termDAO.SubjectName,
                SemesterId = termDAO.SemesterId
            };
        }

        public async Task<Term> Get(TermFilter filter)
        {
            IQueryable<TermDAO> query = examRegContext.Term;
            TermDAO termDAO = DynamicFilter(query, filter).FirstOrDefault();

            return new Term()
            {
                Id = termDAO.Id,
                SubjectName = termDAO.SubjectName,
                SemesterId = termDAO.SemesterId
            };
        }

        public async Task<List<Term>> List(TermFilter filter)
        {
            if (filter == null) return new List<Term>();
            IQueryable<TermDAO> query = examRegContext.Term;
            query = DynamicFilter(query, filter);
            query = DynamicOrder(query, filter);
            List<Term> list = await query.Select(t => new Term()
            {
                Id = t.Id,
                SubjectName = t.SubjectName,
                SemesterId = t.SemesterId
            }).ToListAsync();
            return list;
        }

        public async Task<bool> Update(Term term)
        {
            await examRegContext.Term.Where(t => t.Id.Equals(term.Id)).UpdateFromQueryAsync(t => new TermDAO
            {
                Id = term.Id,
                SubjectName = term.SubjectName,
                SemesterId = term.SemesterId
            });
            return true;
        }
        private IQueryable<TermDAO> DynamicFilter(IQueryable<TermDAO> query, TermFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            // nối ràng buộc (?)
            //query = query.Where(q => q.Id, filter.Id);
            if (filter.Id != null)
                query = query.Where(q => q.Id, filter.Id);
            if (filter.SubjectName != null)
                query = query.Where(q => q.SubjectName, filter.SubjectName);
            if (filter.SemesterId != null)
                query = query.Where(q => q.SemesterId, filter.SemesterId);
            
            return query;
        }

        private IQueryable<TermDAO> DynamicOrder(IQueryable<TermDAO> query, TermFilter filter)
        {
            switch (filter.OrderType)
            {
                case OrderType.ASC:
                    switch (filter.OrderBy)
                    {
                        case TermOrder.Id:
                            query = query.OrderBy(q => q.Id);
                            break;
                        case TermOrder.SubjectName:
                            query = query.OrderBy(q => q.SubjectName);
                            break;
                        case TermOrder.SemesterId:
                            query = query.OrderBy(q => q.SemesterId);
                            break;
                        default:
                            query = query.OrderBy(q => q.CX);
                            break;
                    }
                    break;
                case OrderType.DESC:
                    switch (filter.OrderBy)
                    {
                        case TermOrder.Id:
                            query = query.OrderByDescending(q => q.Id);
                            break;
                        case TermOrder.SubjectName:
                            query = query.OrderByDescending(q => q.SubjectName);
                            break;
                        case TermOrder.SemesterId:
                            query = query.OrderByDescending(q => q.SemesterId);
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
