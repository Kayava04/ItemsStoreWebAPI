using ItemsStoreWebAPI.Models;


namespace ItemsStoreWebAPI.Validators
{
    public class TVRequestValidator : ITVRequestValidator
    {
        public bool IsValid(TV tv, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (string.IsNullOrEmpty(tv.Name))
            {
                errorMessage = "Name is required!";
                return false;
            }

            if (tv.Size <= 0)
            {
                errorMessage = "Size can not be zero or negative!";
                return false;
            }

            if (string.IsNullOrEmpty(tv.Resolution))
            {
                errorMessage = "Resolution is required!";
                return false;
            }

            if (tv.Frequency <= 0)
            {
                errorMessage = "Frequency can not be zero or negative!";
                return false;
            }

            if (tv.ReleasedYear < 1900 || tv.ReleasedYear > DateTime.UtcNow.Year)
            {
                errorMessage = "ReleasedYear must be a correct!";
                return false;
            }

            if (tv.Price < 0)
            {
                errorMessage = "Price can not be negative!";
                return false;
            }

            if (tv.InStock < 0)
            {
                errorMessage = "InStock can not be negative!";
                return false;
            }

            return true;
        }
    }
}