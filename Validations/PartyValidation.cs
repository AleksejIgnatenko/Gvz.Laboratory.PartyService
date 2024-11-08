using FluentValidation;
using Gvz.Laboratory.PartyService.Models;

namespace Gvz.Laboratory.PartyService.Validations
{
    public class PartyValidation : AbstractValidator<PartyModel>
    {
        public PartyValidation()
        {
            RuleFor(x => x.BatchNumber)
                .NotEmpty().WithMessage("Номер партии не может быть пустым")
                .GreaterThan(0).WithMessage("Номер партии должен быть больше 0");
        }
    }
}
