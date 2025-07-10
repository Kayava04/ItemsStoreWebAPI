using FluentValidation;
using ItemsStoreWebAPI.Application.DTOs.Mobile;

namespace ItemsStoreWebAPI.Domain.Validators
{
    public class MobileRequestValidator : AbstractValidator<RequestMobileDto>
    {
        public MobileRequestValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage(ValidationMessages.MobileObjectCannotBeNull);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.NameIsRequired);

            RuleFor(x => x.OS)
                .NotEmpty()
                .WithMessage(ValidationMessages.OsIsRequired);

            RuleFor(x => x.ScreenSize)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ScreenSizeCannotBeZeroOrNegative);

            RuleFor(x => x.BatteryCapacity)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.BatteryCapacityMustBePositive);

            RuleFor(x => x.RAM)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.RamMustBePositive);

            RuleFor(x => x.Storage)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.StorageMustBePositive);

            RuleFor(x => x.ReleasedYear)
                .InclusiveBetween(2000, DateTime.UtcNow.Year)
                .WithMessage(ValidationMessages.ReleasedYearMustBeCorrect);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.PriceCannotBeNegative);

            RuleFor(x => x.InStock)
                .GreaterThanOrEqualTo(0)
                .WithMessage(ValidationMessages.InStockCannotBeNegative);
        }
    }
}