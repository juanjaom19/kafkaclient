using FluentValidation;
using kafkaclient.web.Core.Dto;

namespace kafkaclient.web.Validators;

public class ClusterOrderByRequestValidator : AbstractValidator<ClusterOrderByRequest>
{
    public string[] orderByValids = new string[]{
        "Asc",
        "Desc"
    };

    public ClusterOrderByRequestValidator()
    {
        When(_ => _.OrderById != null, () => {
            RuleFor(_ => _.OrderById)
                .NotNull().WithMessage("order_by_id is not valid")
                .NotEmpty().WithMessage("order_by_id is not valid")
                .Must(_ => orderByValids.Contains(_)).WithMessage("order_by_id solo se aceptan los valores: Asc o Desc");
        });

        When(_ => _.OrderByName != null, () => {
            RuleFor(_ => _.OrderByName)
                .NotNull().WithMessage("order_by_name is not valid")
                .NotEmpty().WithMessage("order_by_name is not valid")
                .Must(_ => orderByValids.Contains(_)).WithMessage("order_by_name solo se aceptan los valores: Asc o Desc");
        });

        When(_ => _.OrderByVersion != null, () => {
            RuleFor(_ => _.OrderByVersion)
                .NotNull().WithMessage("order_by_version is not valid")
                .NotEmpty().WithMessage("order_by_version is not valid")
                .Must(_ => orderByValids.Contains(_)).WithMessage("order_by_version solo se aceptan los valores: Asc o Desc");
        });

        When(_ => _.OrderByHost != null, () => {
            RuleFor(_ => _.OrderByHost)
                .NotNull().WithMessage("order_by_host is not valid")
                .NotEmpty().WithMessage("order_by_host is not valid")
                .Must(_ => orderByValids.Contains(_)).WithMessage("order_by_host solo se aceptan los valores: Asc o Desc");
        });

        When(_ => _.OrderByPath != null, () => {
            RuleFor(_ => _.OrderByPath)
                .NotNull().WithMessage("order_by_path is not valid")
                .NotEmpty().WithMessage("order_by_path is not valid")
                .Must(_ => orderByValids.Contains(_)).WithMessage("order_by_path solo se aceptan los valores: Asc o Desc");
        });
        
        When(_ => _.OrderByStatus != null, () => {
            RuleFor(_ => _.OrderByStatus)
                .NotNull().WithMessage("order_by_status is not valid")
                .NotEmpty().WithMessage("order_by_status is not valid")
                .Must(_ => orderByValids.Contains(_)).WithMessage("order_by_status solo se aceptan los valores: Asc o Desc");
        });
    }
}