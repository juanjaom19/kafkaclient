using System.Net;
using kafkaclient.web.Core.Dto;
using Microsoft.AspNetCore.Http.Extensions;

namespace kafkaclient.web.Core.Config;

public static class HandlerErrorNotFound
{
    public static void ConfigureHandlerErrorNotFound(this IApplicationBuilder app)
    {
        app.Use(async (context, next) =>
        {
            await next();
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                if (context.Response.ContentType is null){
                    context.Response.ContentType = "application/problem+json";
                }

                var response = new ErrorResponse()
                {
                    GeneralMessage = "Resource not found or not valid",
                    Method = context.Request.Method,
                    Endpoint = context.Request.GetEncodedPathAndQuery(),
                    ErrorsGeneral = new List<Error>(){
                        new Error(){
                            Code = "003",
                            Text = "The resource does not exist or is not valid. Check the data",
                            Hints = "Review that the information that is sent in the request is correct",
                            Info = ""
                        }
                        
                    }
                }.ToString();

                await context.Response.WriteAsync(response);
            }
        });
    }
}