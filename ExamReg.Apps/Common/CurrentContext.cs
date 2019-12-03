using System;

namespace ExamReg.Apps.Common
{
    public interface ICurrentContext : IServiceScoped
    {
        Guid UserId { get; set; }
        string Username { get; set; }
        bool IsAdmin { get; set; }
        Guid StudentId { get; set; }
        int StudentNumber { get; set; }
    }

    public class CurrentContext : ICurrentContext
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public Guid StudentId { get; set; }
        public int StudentNumber { get; set; }
    }
}