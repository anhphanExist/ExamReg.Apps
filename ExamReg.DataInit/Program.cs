using ExamReg.Apps.Repositories.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Z.EntityFramework.Extensions;

namespace ExamReg.DataInit
{
    class Program
    {
        private static ExamRegContext examRegContext;
        static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build();
            string connectionString = config.GetConnectionString("ExamRegContext");
            var options = new DbContextOptionsBuilder<ExamRegContext>()
                .UseNpgsql(connectionString)
                .Options;
            examRegContext = new ExamRegContext(options);
            EntityFrameworkManager.ContextFactory = DbContext => examRegContext;

            DataInit dataInit = new DataInit(examRegContext);
            dataInit.Init();
        }
    }
}
