using System.Net;
using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Utils;
using Microsoft.AspNetCore.Http.Extensions;

namespace kafkaclient.web.Core.Config;

public class ErrorCustomHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorCustomHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            var response = context.Response;
            switch(error)
            {
                case NotFoundException e:
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.ContentType = "application/json";
                    var responseMessageNotFound = new ErrorResponse()
                    {
                        GeneralMessage = "Resource not found or not valid",
                        Method = context.Request.Method,
                        Endpoint = context.Request.GetEncodedPathAndQuery(),
                        ErrorsGeneral = new List<Error>(){
                            new Error(){
                                Code = "003",
                                Text = "The resource does not exist or is not valid. Check the data",
                                Hints = "Review that the information that is sent in the request is correct",
                                Info = $"{e.Message}"
                            }
                        }
                    }.ToString();
                    
                    await response.WriteAsync(responseMessageNotFound);
                    break;
                case BadRequestException e:
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ContentType = "application/json";
                     var responseMessageBadRequest = new ErrorResponse()
                    {
                        GeneralMessage = "BadRequest",
                        Method = context.Request.Method,
                        Endpoint = context.Request.GetEncodedPathAndQuery(),
                        ErrorsGeneral = new List<Error>(){
                            new Error(){
                                Code = "000",
                                Text = "The resource  is not valid. Check the data",
                                Hints = "Review that the information that is sent in the request is correct",
                                Info = $"{e.Message}"
                            }
                        }
                    }.ToString();
                    await response.WriteAsync(responseMessageBadRequest);
                    break;
                case UnexpectedException e:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.ContentType = "application/json";
                    var responseMessageInternalServer = new ErrorResponse()
                    {
                        GeneralMessage = "There was an unexpected error",
                        Method = context.Request.Method,
                        Endpoint = context.Request.GetEncodedPathAndQuery(),
                        ErrorsGeneral = new List<Error>(){
                            new Error(){
                                Code = "004",
                                Text = "An error occurred inside the application on execution time",
                                Hints = "Report the issue to the support team. Try again later",
                                Info = $"{e.Message}"
                            } 
                        }
                    }.ToString();
                    await response.WriteAsync(responseMessageInternalServer);
                    break;
                case ConflictException e:
                    response.StatusCode = (int)HttpStatusCode.Conflict;
                    response.ContentType = "application/json";
                    var responseMessageConflict = new ErrorResponse()
                    {
                        GeneralMessage = "Already exists",
                        Method = context.Request.Method,
                        Endpoint = context.Request.GetEncodedPathAndQuery(),
                        ErrorsGeneral = new List<Error>(){
                            new Error(){
                                Code = "004",
                                Text = "The resource already exists",
                                Hints = "Review that the information that is sent in the request is correct",
                                Info = $"{e.Message}"
                            } 
                        }
                    }.ToString();
                    await response.WriteAsync(responseMessageConflict);
                    break;
                default:
                    await _next(context);
                    break;
            }
        }
    }
}