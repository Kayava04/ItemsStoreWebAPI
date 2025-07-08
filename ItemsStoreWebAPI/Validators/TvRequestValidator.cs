using FluentValidation;
using ItemsStoreWebAPI.DTOs.TV;

namespace ItemsStoreWebAPI.Validators
{
    public class TvRequestValidator : AbstractValidator<RequestTvDto>
    {
        public TvRequestValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .WithMessage(ValidationMessages.TvObjectCannotBeNull);

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage(ValidationMessages.NameIsRequired);

            RuleFor(x => x.ScreenSize)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.ScreenSizeCannotBeZeroOrNegative);

            RuleFor(x => x.Resolution)
                .NotEmpty()
                .WithMessage(ValidationMessages.ResolutionIsRequired);

            RuleFor(x => x.Frequency)
                .GreaterThan(0)
                .WithMessage(ValidationMessages.FrequencyCannotBeZeroOrNegative);

            RuleFor(x => x.ReleasedYear)
                .InclusiveBetween(1900, DateTime.UtcNow.Year)
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