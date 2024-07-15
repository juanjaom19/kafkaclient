using kafkaclient.web.Core.Dto;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace kafkaclient.web.Core.Config;

public static class HandlerErrorRequest
{
    public static ErrorResponse ErrorDetail { get; set; } = new ErrorResponse();
    public static IActionResult ConfigureHandlerErrorRequest(ActionContext context)
    {
        ErrorDetail.GeneralMessage = "Not valid request";
        ErrorDetail.Method = context.HttpContext.Request.Method;
        ErrorDetail.Endpoint = context.HttpContext.Request.GetEncodedPathAndQuery();
        ErrorDetail.ErrorsGeneral = new List<Error>(){
            new Error(){
                Code = "002",
                Text = "The key/value attributes of the request are not valid",
                Hints = "Review that all parameters.",
                Info = ""
            }  
        };
        ErrorDetail.ErrorsProps = new Dictionary<string, List<string>>();

        foreach (var keyModelStatePair in context.ModelState)
        {
            var key = keyModelStatePair.Key;
            var errors = keyModelStatePair.Value.Errors;
            List<String> errorsList = new List<string>();
            if (errors != null && errors.Count > 0)
            {
                if (errors.Count == 1)
                {
                    var errorMessage = GetErrorMessage(errors[0]);
                    errorsList.Add(errorMessage);
                    ErrorDetail.ErrorsProps.Add(key, errorsList);
                }
                else
                {
                    var errorMessages = new string[errors.Count];
                    for (var i = 0; i < errors.Count; i++)
                    {
                        errorMessages[i] = GetErrorMessage(errors[i]);
                    }
                    ErrorDetail.ErrorsProps.Add(key, errorMessages.ToList());
                }
            }
        }
        
        var result = new BadRequestObjectResult(ErrorDetail);
        result.ContentTypes.Add("application/problem+json");
        return result;
    }

    static string GetErrorMessage(ModelError error)
    {
        return string.IsNullOrEmpty(error.ErrorMessage) ?
        "The input was not valid." :
        error.ErrorMessage;
    }
}