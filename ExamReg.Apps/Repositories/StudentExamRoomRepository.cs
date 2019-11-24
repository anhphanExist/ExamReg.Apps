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
        Task<List<StudentExamRoom>> List();
    }
    public class StudentExamRoomRepository : IStudentExamRoomRepository
    {
        private ExamRegContext examRegContext;
        public StudentExamRoomRepository(ExamRegContext examReg)
        {
            this.examRegContext = examReg;
        }
        public async Task<List<StudentExamRoom>> List()
        {
            IQueryable<StudentExamRoomDAO> studentExamRoomDAOs = examRegContext.StudentExamRoom;
            //studentExamRoomDAOs = DynamicFilter(studentExamRoomDAOs, examRegContext.BusinessGroupId);
            return new List<StudentExamRoom>();
        }
        private IQueryable<StudentExamRoomDAO> DynamicFilter(IQueryable<StudentExamRoomDAO> studentExamRooms, Guid StudentId)
        {
            /*if (StudentId != null)
                studentExamRooms = studentExamRooms.Where(u => u..StudentId..Equals(StudentId));*/

            return studentExamRooms;
        }
    }
}
