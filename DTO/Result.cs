using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Blog.DTO
{
    public class Result<T>
    {
        public bool Success {get;}
        public ResultStatus Status {get;set;}
        public T? Value {get;set;}
        public string? Error {get;set;}

        public Result(ResultStatus status, T? val,  string? error)
        {
            Status=status;
            switch (status)
            {
                case ResultStatus.Ok:
                    {
                        Success=true;
                        Value=val;
                        Error=null;
                     break;   
                    }
                case ResultStatus.NoContent:
                    {
                        Success = true;
                        Value= default(T);
                        Error = null;
                        break;
                    }
                case ResultStatus.BadRequest or ResultStatus.Unauthorized or ResultStatus.Forbidden or ResultStatus.NotFound or ResultStatus.Conflict:
                    {
                        Success =false;
                        Value= default(T);
                        Error = error;
                        break;
                    }
                default:
                    {
                        Success= false;
                        Value = default(T);
                        Error = null;
                        break;
                    }
            }
        }

        public IActionResult ToAction()
        {
            switch (Status)
            {
                case ResultStatus.Ok: return new  OkObjectResult(Value);
                case ResultStatus.NoContent: return new NoContentResult();
                case ResultStatus.NotFound: return new NotFoundObjectResult(Error);
                case ResultStatus.Unauthorized: return new UnauthorizedResult();
                case ResultStatus.BadRequest: return new BadRequestObjectResult(Error);
                case ResultStatus.Conflict: return new ConflictObjectResult(Error);
                case ResultStatus.Forbidden: return new ForbidResult();
            }
            return new BadRequestObjectResult(Error);
        }
    }
}