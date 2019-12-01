using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ExamReg.Apps.Common
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate next;
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError; // 500 if unexpected

            if (ex is NotFoundException) code = HttpStatusCode.NotFound;
            else if (ex is UnauthorizedException) code = HttpStatusCode.Unauthorized;
            else if (ex is BadRequestException) code = HttpStatusCode.BadRequest;
            
            string result = ex.Message;
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex is MessageException ? 420 : (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }

    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }

    public class BadRequestException : Exception
    {
        public DataDTO DataDTO { get; }
        public object DataDTOs { get; }
        private static string ModifyMessage(object DataDTO)
        {
            return JsonConvert.SerializeObject(DataDTO, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        public BadRequestException(DataDTO DataDTO) : base(ModifyMessage(DataDTO))
        {
            this.DataDTO = DataDTO;
        }

        public BadRequestException(object DataDTOs) : base(ModifyMessage(DataDTOs))
        {
            this.DataDTOs = DataDTOs;
        }
    }

    public class MessageException : Exception
    {
        private static string ModifyMessage(object DataDTO)
        {
            return JsonConvert.SerializeObject(DataDTO, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        public MessageException(string message) : base(message)
        {
        }
        public MessageException(Exception ex) : base(ModifyMessage(ex))
        {

        }
    }
}
