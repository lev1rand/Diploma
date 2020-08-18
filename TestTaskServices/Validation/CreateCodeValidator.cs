using FluentValidation;
using System.Linq;
using TestTaskServices.Models;

namespace TestTaskServices.Validation
{
    public class CreateCodeValidator : AbstractValidator<CreateCodeModel>
    {
        public CreateCodeValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(n => n.Number)
                .NotNull()
                .Length(3)
                .WithMessage("Length {TotalLength} of {PropertyName} is invalid")
                .Custom((n, valContext) =>
                {
                    if (n != null)
                    {
                        string checkedNumber = n;

                        bool isAllDigits() => n.All(char.IsDigit);

                        if (!isAllDigits())
                        {
                            valContext.AddFailure("You should input only numbers!");
                        }
                    }
                });
            RuleFor(n => n.Name)
                .NotNull();

        }
    }
}
