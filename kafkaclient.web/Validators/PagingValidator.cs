using FluentValidation;
using kafkaclient.web.Core.Dto;

namespace kafkaclient.web.Validators;

public class PagingValidator : AbstractValidator<PagingRequest>
{
    public PagingValidator() 
    {
        // Validations
        RuleFor(_ => _.Offset).NotNull().GreaterThan(0);
        RuleFor(_ => _.Limit).NotNull().GreaterThan(0);
    }
}