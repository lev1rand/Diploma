using FluentValidation;
using System.Linq;
using TestTaskServices.Models;

namespace TestTaskServices.Validation
{
    public class UpdateUserValidator : AbstractValidator<UpdateCodeModel>
    {
        #region 

        private const int MAX_NAME_LENGTH = 1073741791;

        #endregion
        public UpdateUserValidator()
        {
            RuleFor(n => n.Name)
                .NotNull()
                .Length(1, 1073741791)
                .Custom((s, valContext) =>
                {
                    if (s != null)
                    {
                        string checkedName = s;

                        bool isAllChars() => s.All(char.IsWhiteSpace);

                        if (isAllChars())
                        {
                            valContext.AddFailure("You should input text!");
                        }
                    }
                });
            RuleFor(x => x.Id)
                .NotNull();

            /*RuleFor(u => u.Number)
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
                });*/

            
        }
    }
}
