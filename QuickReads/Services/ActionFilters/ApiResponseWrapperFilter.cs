using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using QuickReads.Entities;

namespace QuickReads.Services.ActionFilters;

public class ApiResponseWrapperFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var statusCode = objectResult.StatusCode ?? 200;

            if (objectResult.Value is ApiResponseModel<object>) return;

            var apiResponse = new ApiResponseModel<object>
            {
                Error = statusCode >= 400,
                Message = statusCode == 200 ? "OK" : GetDefaultMessageForStatusCode(statusCode),
                Result = objectResult.Value,
                StatusCode = statusCode,
                ErrorCode = 0
            };

            context.Result = new ObjectResult(apiResponse)
            {
                StatusCode = statusCode
            };
        }
        else if (context.Result is EmptyResult)
        {
            context.Result = new ObjectResult(new ApiResponseModel<object>
            {
                Error = false,
                Message = "No Content",
                Result = null,
                StatusCode = 204,
                ErrorCode = 0
            })
            {
                StatusCode = 204
            };
        }
    }

    private static string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            500 => "Internal Server Error",
            _ => "Error"
        };
    }
}