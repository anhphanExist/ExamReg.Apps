using ExamReg.Apps.Common;
using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
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
        IUserRepository UserRepository { get; }
        IExamPeriodRepository ExamPeriodRepository { get; }
        IExamProgramRepository ExamProgramRepository { get; }
        IExamRegisterRepository ExamRegisterRepository { get; }
        IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository { get; }
        IExamRoomRepository ExamRoomRepository { get; }
        ISemesterRepository SemesterRepository { get; }
        IStudentRepository StudentRepository { get; }
        IStudentTermRepository StudentTermRepository { get; }
        ITermRepository TermRepository { get; }
    }
    public class UOW : IUOW
    {
        private ExamRegContext examRegContext;
        private ICurrentContext CurrentContext;
        public IUserRepository UserRepository { get; }
        public IExamPeriodRepository ExamPeriodRepository { get; }
        public IExamProgramRepository ExamProgramRepository { get; }
        public IExamRegisterRepository ExamRegisterRepository { get; }
        public IExamRoomExamPeriodRepository ExamRoomExamPeriodRepository { get; }
        public IExamRoomRepository ExamRoomRepository { get; }
        public ISemesterRepository SemesterRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public IStudentTermRepository StudentTermRepository { get; }
        public ITermRepository TermRepository { get; }

        public UOW(ExamRegContext examReg, ICurrentContext CurrentContext)
        {
            this.examRegContext = examReg;
            this.CurrentContext = CurrentContext;
            UserRepository = new UserRepository(this.examRegContext, CurrentContext);
            ExamPeriodRepository = new ExamPeriodRepository(this.examRegContext, CurrentContext);
            ExamProgramRepository = new ExamProgramRepository(this.examRegContext, CurrentContext);
            ExamRegisterRepository = new ExamRegisterRepository(this.examRegContext, CurrentContext);
            ExamRoomExamPeriodRepository = new ExamRoomExamPeriodRepository(this.examRegContext, CurrentContext);
            ExamRoomRepository = new ExamRoomRepository(this.examRegContext, CurrentContext);
            SemesterRepository = new SemesterRepository(this.examRegContext, CurrentContext);
            StudentRepository = new StudentRepository(this.examRegContext, CurrentContext);
            StudentTermRepository = new StudentTermRepository(this.examRegContext, CurrentContext);
            TermRepository = new TermRepository(this.examRegContext, CurrentContext);
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
