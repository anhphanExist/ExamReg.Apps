using ExamReg.Apps.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IStudentExamRoomRepository
    {

    }
    public class StudentExamRoomRepository : IStudentExamRoomRepository
    {
        private ExamRegContext examRegContext;
        public StudentExamRoomRepository(ExamRegContext examRegContext)
        {
            this.examRegContext = examRegContext;
        }
    }
}
