using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Validators
{
    public class TVRequestValidator : ITVRequestValidator
    {
        public bool IsValid(TV tv, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (tv == null)
            {
                errorMessage = ValidationMessages.TVObjectCannotBeNull;
                return false;
            }

            if (string.IsNullOrEmpty(tv.Name))
            {
                errorMessage = ValidationMessages.NameIsRequired;
                return false;
            }

            if (tv.Size <= 0)
            {
                errorMessage = ValidationMessages.SizeCannotBeZeroOrNegative;
                return false;
            }

            if (string.IsNullOrEmpty(tv.Resolution))
            {
                errorMessage = ValidationMessages.ResolutionIsRequired;
                return false;
            }

            if (tv.Frequency <= 0)
            {
                errorMessage = ValidationMessages.FrequencyCannotBeZeroOrNegative;
                return false;
            }

            if (tv.ReleasedYear < 1900 || tv.ReleasedYear > DateTime.UtcNow.Year)
            {
                errorMessage = ValidationMessages.ReleasedYearMustBeCorrect;
                return false;
            }

            if (tv.Price < 0)
            {
                errorMessage = ValidationMessages.PriceCannotBeNegative;
                return false;
            }

            if (tv.InStock < 0)
            {
                errorMessage = ValidationMessages.InStockCannotBeNegative;
                return false;
            }

            return true;
        }
    }
}