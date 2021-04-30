using FluentValidation;
using TestTaskServices.Models;

namespace TestTaskServices.Validation
{
    public class CreateUserValidator : AbstractValidator<CreateCodeModel>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Id).NotNull();
            
            RuleFor(n => n.Name)
                .NotNull();

        }
    }
}
