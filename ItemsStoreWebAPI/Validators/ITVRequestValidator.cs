using ItemsStoreWebAPI.Models;

namespace ItemsStoreWebAPI.Validators
{
    public interface ITVRequestValidator
    {
        bool IsValid(TV tv, out string errorMessage);
    }
}