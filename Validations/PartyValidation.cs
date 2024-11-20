using FluentValidation;
using Gvz.Laboratory.PartyService.Models;
using System.Globalization;

namespace Gvz.Laboratory.PartyService.Validations
{
    public class PartyValidation : AbstractValidator<PartyModel>
    {
        public PartyValidation()
        {
            RuleFor(x => x.BatchNumber)
                .NotEmpty().WithMessage("Номер партии не может быть пустым")
                .GreaterThan(0).WithMessage("Номер партии должен быть больше 0");

            RuleFor(x => x.DateOfReceipt)
                .NotEmpty().WithMessage("Дата получения не может быть пустой")
                .Must(BeValidDate).WithMessage("Дата получения должна быть в формате yyyy-MM-dd");

            RuleFor(x => x.BatchSize)
            .GreaterThan(0).WithMessage("Объем партии должен быть больше 0");

            RuleFor(x => x.SampleSize)
                .GreaterThan(0).WithMessage("Объем выборки должен быть больше 0")
                .LessThan(x => x.BatchSize).WithMessage("Объем выборки должен быть меньше размера партии");

            RuleFor(x => x.TTN)
                .Must(x => x.ToString().All(char.IsDigit)).WithMessage("ТТН должен содержать только цифры")
                .When(x => x.TTN >= 0);

            RuleFor(x => x.DocumentOnQualityAndSafety)
                .NotEmpty().WithMessage("Документ по качеству и безопасности не может быть пустым");

            RuleFor(x => x.TestReport)
                .NotEmpty().WithMessage("Протокол испытаний не может быть пустым");
        }

        private bool BeValidDate(string date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
