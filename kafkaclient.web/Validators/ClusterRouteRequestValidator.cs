using FluentValidation;
using kafkaclient.web.Core.Dto;

namespace kafkaclient.web.Validators;

public class ClusterRouteRequestValidator : AbstractValidator<ClusterRouteRequest>
{
    public ClusterRouteRequestValidator() 
    {
        RuleFor(_ => _.Id)
            .NotEmpty().NotNull().WithMessage("id is required")
            .GreaterThan(0).WithMessage("id should be grater than 0");
    }
}