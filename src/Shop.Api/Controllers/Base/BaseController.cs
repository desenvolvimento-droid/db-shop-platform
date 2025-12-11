using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Shop.Common.Errors;

namespace Shop.Api.Controllers.Base;

public abstract class BaseController(ILogger logger) : ControllerBase
{
    #region HandleResult
    protected IActionResult HandleResult(Result result)
    {
        var callDetails = GetCallDetails();

        if (result.IsSuccess)
        {
            logger.LogInformation("""
                Request processed successfully | 
                Details: {CallDetails}
                """, callDetails);

            return Ok();
        }

        return HandleFailedResult(result, callDetails);
    }
    #endregion

    #region HandleResult<T>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        var callDetails = GetCallDetails();

        if (result.IsSuccess)
        {
            logger.LogInformation("""
                Request processed successfully | 
                Value type: {ValueType} | 
                Details: {CallDetails}
                """, typeof(T).Name, callDetails);

            return Ok(result.Value);
        }

        return HandleFailedResult(result.ToResult(), callDetails);
    }
    #endregion

    #region [PRIVATES]
    private IActionResult HandleFailedResult(Result result, string callDetails)
    {
        var errorDetails = string.Join(" | ", result.Errors.Select(e => e.Message));

        logger.LogError("""
            Unknown error in handler | 
            Errors: {ErrorDetails} | 
            Details: {CallDetails}
            """, errorDetails, callDetails);

        if (result.HasError<RuleError>())
        {
            return MapRuleErrorToActionResult(result, callDetails);
        }

        if (result.TryGetErrorMessages<ValidationError>(out var validationErrors))
        {
            logger.LogWarning("""
                Validation errors detected | 
                Errors: {ValidationErrors} | 
                Details: {CallDetails}
                """, string.Join(", ", validationErrors), callDetails);

            return BadRequest(new { Errors = validationErrors });
        }

        throw new ApplicationException($"Unknown error in handler: {errorDetails} | {callDetails}");
    }

    private IActionResult MapRuleErrorToActionResult(Result result, string callDetails)
    {
        var error = result.GetRuleError();

        logger.LogWarning("""
            Rule error detected |
            Code: {ErrorCode} |
            Message: {ErrorMessage} |
            Details: {CallDetails}
            """, error.Codigo, error.Message, callDetails);

        if (result.IsResourceAlreadyExists())
        {
            return Conflict(error);
        }

        if (result.IsResourceNotFound())
        {
            return NotFound(error);
        }

        if (result.IsUserWithoutUnauthorized())
        {
            return Unauthorized(error);
        }

        return BadRequest(error);
    }

    private string GetCallDetails()
    {
        try
        {
            var routeData = HttpContext.GetRouteData();
            var request = HttpContext.Request;

            var controllerName = routeData.Values["controller"]?.ToString() ?? "Unknown";
            var actionName = routeData.Values["action"]?.ToString() ?? "Unknown";
            var httpMethod = request.Method;
            var path = request.Path;
            var traceId = HttpContext.TraceIdentifier;
            var userId = HttpContext.User?.FindFirst("sub")?.Value
                ?? HttpContext.User?.FindFirst("id")?.Value
                ?? "Anonymous";

            return $"Method: {httpMethod} | Route: {path} | Controller: {controllerName} | Action: {actionName} | TraceId: {traceId} | UserId: {userId}";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting call details");
            return "Details unavailable";
        }
    }
    #endregion

}