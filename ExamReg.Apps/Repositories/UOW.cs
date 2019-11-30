using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Repositories
{
    public interface IUOW : IServiceScoped
    {
        Task Begin();
        Task Commit();
        Task Rollback();
        IExamPeriodRepository ExamPeriodRepository { get; }
        IExamProgramRepository ExamProgramRepository { get; }
        IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository { get; }
        IExamRoomRepository ExamRoomRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        IStudentExamPeriodRepository StudentExamPeriodRepository { get; }
        IStudentExamRoomRepository StudentExamRoomRepository { get; }
        IStudentRepository StudentRepository { get; }
        IStudentTermRepository StudentTermRepository { get; }
        ITermRepository TermRepository { get; }
        IUserRepository UserRepository { get; }

    }

    public class UOW : IUOW
    {
        private ExamRegContext examRegContext;
        public IExamPeriodRepository ExamPeriodRepository { get; private set; }

        public IExamProgramRepository ExamProgramRepository { get; private set; }

        public IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository { get; private set; }

        public IExamRoomRepository ExamRoomRepository { get; private set; }

        public ISemesterRepository SemesterRepository { get; private set; }

        public IStudentExamPeriodRepository StudentExamPeriodRepository { get; private set; }

        public IStudentExamRoomRepository StudentExamRoomRepository { get; private set; }

        public IStudentRepository StudentRepository { get; private set; }

        public IStudentTermRepository StudentTermRepository { get; private set; }

        public ITermRepository TermRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }
        public UOW(ExamRegContext examRegContext,
            IExamPeriodRepository ExamPeriodRepository,
            IExamProgramRepository ExamProgramRepository,
            IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository,
            IExamRoomRepository ExamRoomRepository,
            ISemesterRepository SemesterRepository,
            IStudentExamPeriodRepository StudentExamPeriodRepository,
            IStudentExamRoomRepository StudentExamRoomRepository,
            IStudentRepository StudentRepository,
            IStudentTermRepository StudentTermRepository,
            ITermRepository TermRepository,
            IUserRepository UserRepository)
        {
            this.examRegContext = examRegContext;
            this.ExamPeriodRepository = ExamPeriodRepository;
            this.ExamProgramRepository = ExamProgramRepository;
            this.ExamRoomExamPeriodRepository = ExamRoomExamPeriodRepository;
            this.ExamRoomRepository = ExamRoomRepository;
            this.SemesterRepository = SemesterRepository;
            this.StudentExamPeriodRepository = StudentExamPeriodRepository;
            this.StudentExamRoomRepository = StudentExamRoomRepository;
            this.StudentRepository = StudentRepository;
            this.StudentTermRepository = StudentTermRepository;
            this.TermRepository = TermRepository;
            this.UserRepository = UserRepository;
        }



        public async Task Begin()
        {
            await examRegContext.Database.BeginTransactionAsync();
        }

        public Task Commit()
        {
            examRegContext.Database.CommitTransaction();
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            examRegContext.Database.RollbackTransaction();
            return Task.CompletedTask;
        }
    }
}
