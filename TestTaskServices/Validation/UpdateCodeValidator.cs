using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTaskServices.Models;

namespace TestTaskServices.Validation
{
    public class UpdateCodeValidator : AbstractValidator<UpdateCodeModel>
    {
        public UpdateCodeValidator()
        {
            RuleFor(x => x.Id)
                .NotNull();

            RuleFor(u => u.Name)
                .NotNull()
                .When(w => w.Number == null);

            RuleFor(u => u.Number)
                .NotNull()
                .When(w => w.Name == null)
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
        }
    }
}
