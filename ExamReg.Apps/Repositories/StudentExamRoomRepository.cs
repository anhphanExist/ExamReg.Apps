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
    public interface IStudentExamRoomRepository
    {
        Task<StudentExamRoom> Get(StudentExamRoomFilter filter);
        //Task<bool> Create(StudentExamRoom studentExamRoom);
        //Task<bool> Update(StudentExamRoom studentExamRoom);
        //Task<bool> Delete(StudentExamRoom studentExamRoom);
        //Task<List<StudentExamRoom>> List(StudentExamRoomFilter filter);
        Task<List<StudentExamRoom>> List();
    }
    public class StudentExamRoomRepository : IStudentExamRoomRepository
    {
        private ExamRegContext examRegContext;
        public StudentExamRoomRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        /*public async Task<bool> Create(StudentExamRoom studentExamRoom)
        {
            StudentExamRoomDAO studentExamRoomDAO = examRegContext.StudentExamRoom
                .Where(s => (s.StudentId.Equals(studentExamRoom.StudentId) && s.ExamRoomId.Equals(studentExamRoom.ExamRoomId)))
                .FirstOrDefault();
            if (studentExamRoomDAO == null)
            {
                studentExamRoomDAO = new StudentExamRoomDAO()
                {
                    StudentId = studentExamRoom.StudentId,
                    ExamRoomId = studentExamRoom.ExamRoomId
                };

                await examRegContext.StudentExamRoom.AddAsync(studentExamRoomDAO);
            }
            else
            {
                //
            };
            await examRegContext.SaveChangesAsync();
            return true;
        }*/

        /*public async Task<bool> Delete(StudentExamRoom studentExamRoom)
        {
            try
            {
                StudentExamRoomDAO studentExamRoomDAO = examRegContext.StudentExamRoom
                    .Where(s => (s.StudentId.Equals(studentExamRoom.StudentId) && s.ExamRoomId.Equals(studentExamRoom.ExamRoomId)))
                    .AsNoTracking()
                    .FirstOrDefault();

                examRegContext.StudentExamRoom.Remove(studentExamRoomDAO);
                await examRegContext.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }*/

        public async Task<StudentExamRoom> Get(StudentExamRoomFilter filter)
        {
            IQueryable<StudentExamRoomDAO> studentExamRoomDAOs = examRegContext.StudentExamRoom;
            StudentExamRoomDAO studentExamRoomDAO = DynamicFilter(studentExamRoomDAOs, filter).FirstOrDefault();

            return new StudentExamRoom()
            {
                StudentId = studentExamRoomDAO.StudentId,
                ExamRoomId = studentExamRoomDAO.ExamRoomId
            };
        }
        public async Task<List<StudentExamRoom>> List()
        {
            List<StudentExamRoom> list = await examRegContext.StudentExamRoom.Select(s => new StudentExamRoom()
            {
                StudentId = s.StudentId,
                ExamRoomId = s.ExamRoomId
            }).ToListAsync();
            return list;
        }

        /*public async Task<List<StudentExamRoom>> List(StudentExamRoomFilter filter)
        {
            if (filter == null) return new List<StudentExamRoom>();

            IQueryable<StudentExamRoomDAO> query = examRegContext.StudentExamRoom;
            query = DynamicFilter(query, filter);

            List<StudentExamRoom> list = await query.Select(s => new StudentExamRoom()
            {
                StudentId = s.StudentId,
                ExamRoomId = s.ExamRoomId

            }).ToListAsync();
            return list;

        }*/

        /*public async Task<bool> Update(StudentExamRoom StudentExamRoom)
        {
            await examRegContext.StudentExamRoom
                .Where(s => (s.StudentId.Equals(StudentExamRoom.StudentId) && s.ExamRoomId.Equals(StudentExamRoom.ExamRoomId)))
                .UpdateFromQueryAsync(s => new StudentExamRoomDAO()
                {
                    //
                });

            await examRegContext.SaveChangesAsync();
            return true;
        }*/
        private IQueryable<StudentExamRoomDAO> DynamicFilter(IQueryable<StudentExamRoomDAO> query, StudentExamRoomFilter filter)
        {
            if (filter == null)
                return query.Where(q => 1 == 0);
            query = query.Where(q => q.StudentId, filter.StudentId);
            query = query.Where(q => q.ExamRoomId, filter.ExamRoomId);

            return query;
        }
    }
}
