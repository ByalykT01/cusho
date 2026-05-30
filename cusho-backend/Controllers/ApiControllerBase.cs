using cusho.Infrastructure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace cusho.Controllers;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected ProblemHttpResult BadRequestProblem(string? detail = null) =>
        TypedResults.Problem(ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: ProblemDescriptors.BadRequest.StatusCode,
            title: ProblemDescriptors.BadRequest.Title,
            detail: detail ?? ProblemDescriptors.BadRequest.Detail,
            type: ProblemDescriptors.BadRequest.Type));

    protected ProblemHttpResult NotFoundProblem(string? detail = null) =>
        TypedResults.Problem(ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: ProblemDescriptors.NotFound.StatusCode,
            title: ProblemDescriptors.NotFound.Title,
            detail: detail ?? ProblemDescriptors.NotFound.Detail,
            type: ProblemDescriptors.NotFound.Type));

    protected ProblemHttpResult UnauthorizedProblem() =>
        TypedResults.Problem(ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: ProblemDescriptors.Unauthorized.StatusCode,
            title: ProblemDescriptors.Unauthorized.Title,
            detail: ProblemDescriptors.Unauthorized.Detail,
            type: ProblemDescriptors.Unauthorized.Type));

    protected ProblemHttpResult ForbiddenProblem() =>
        TypedResults.Problem(ProblemDetailsFactory.CreateProblemDetails(
            HttpContext,
            statusCode: ProblemDescriptors.Forbidden.StatusCode,
            title: ProblemDescriptors.Forbidden.Title,
            detail: ProblemDescriptors.Forbidden.Detail,
            type: ProblemDescriptors.Forbidden.Type));
}
