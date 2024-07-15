using FluentValidation;
using kafkaclient.web.Core.Dto;
using kafkaclient.web.Core.Utils;

namespace kafkaclient.web.Validators;

public class ProductRequestValidator : AbstractValidator<ClusterRequest>
{
    public static Dictionary<string, int> Statuses = new Dictionary<string, int>()
    {
        { "Activo", 1},
        { "Inactivo", 0}
    };

    public ProductRequestValidator(IHttpContextAccessor httpContextAccessor) 
    {
        var httpContext = httpContextAccessor.HttpContext;

        RuleFor(_ => _.Name).NotNull().WithMessage("El nombre es requerido")
                .MinimumLength(1).WithMessage("El nombre deberia tener al menos 1 caracter")
                .MaximumLength(100).WithMessage("El nombre name deberia tener menos de 100 caracteres")
                .Matches(GenericRegex.Name).WithMessage("El nombre es invalido");

        RuleFor(_ => _.Version).NotNull().WithMessage("La version es requerida")
                .MinimumLength(1).WithMessage("La version deberia tener al menos 1 caracter")
                .MaximumLength(250).WithMessage("La version deberia tener menos de 250 caracteres")
                .Matches(GenericRegex.Name).WithMessage("La version es invalida");

        RuleFor(_ => _.Host).NotNull().WithMessage("El host es requerido")
                .MinimumLength(1).WithMessage("El host should have at less 1 caracter")
                .MaximumLength(100).WithMessage("El host should have less 100 caracteres");

        RuleFor(_ => _.Path).NotNull().WithMessage("El path es requerido")
                .MinimumLength(1).WithMessage("El path deberia tener al menos 1 caracter")
                .MaximumLength(100).WithMessage("El path deberia tener menos de 100 caracteres");

        RuleFor(_ => _.Status)
                .NotNull().WithMessage("El status es requerido")
                .Must(_ => Statuses.ContainsKey(_?? "NULL")).WithMessage("El status es invalido");
        
        When(_ => httpContext != null && httpContext.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase), () => {
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("El slug es requerido")
                .NotNull().WithMessage("El slug es requerido");
        });

        
    }
}