using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExamReg.Apps.Common
{
    public interface IPathProvider : IServiceScoped
    {
        string MapPath(string path);
    }
    public class PathProvider : IPathProvider
    {
        private IHostingEnvironment _hostingEnvironment;

        public PathProvider(IHostingEnvironment environment)
        {
            _hostingEnvironment = environment;
        }

        public string MapPath(string path)
        {
            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, path);
            return filePath;
        }
    }
}
