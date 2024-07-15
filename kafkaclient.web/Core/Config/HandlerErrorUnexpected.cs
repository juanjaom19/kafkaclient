using System.Net;
using kafkaclient.web.Core.Dto;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Extensions;

namespace kafkaclient.web.Core.Config;

public static class HandlerErrorUnexpected
{
    public static void ConfigureHandlerErrorUnexpected(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if(contextFeature != null)
                {
                    var response = new ErrorResponse()
                    {
                        GeneralMessage = "There was an unexpected error",
                        Method = context.Request.Method,
                        Endpoint = context.Request.GetEncodedPathAndQuery(),
                        ErrorsGeneral = new List<Error>(){
                            new Error(){
                                Code = "004",
                                Text = "An error occurred inside the application on execution time",
                                Hints = "Report the issue to the support team. Try again later",
                                Info = $"{contextFeature.Error.Message}"
                            }
                            
                        }
                    }.ToString();
                    await context.Response.WriteAsync(response);
                }
            });
        });
    }
}