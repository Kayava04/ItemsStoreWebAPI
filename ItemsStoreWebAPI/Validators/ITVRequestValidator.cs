using ItemsStoreWebAPI.DTOs;

namespace ItemsStoreWebAPI.Validators
{
    public interface ITVRequestValidator
    {
        bool IsValid(RequestTvDto tv, out string errorMessage);
    }
}