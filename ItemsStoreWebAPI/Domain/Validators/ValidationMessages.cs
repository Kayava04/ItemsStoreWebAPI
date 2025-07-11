namespace ItemsStoreWebAPI.Domain.Validators
{
    public static class ValidationMessages
    {
        // TV
        public const string TvObjectCannotBeNull = "TV object can not be null!";
        public const string NameIsRequired = "Name is required!";
        public const string ScreenSizeCannotBeZeroOrNegative = "Screen size must be greater than zero!";
        public const string ResolutionIsRequired = "Resolution is required!";
        public const string FrequencyCannotBeZeroOrNegative = "Frequency can not be zero or negative!";
        public const string ReleasedYearMustBeCorrect = "ReleasedYear must be a correct!";
        public const string PriceCannotBeNegative = "Price can not be negative!";
        public const string InStockCannotBeNegative = "InStock can not be negative!";
        
        // Mobile
        public const string MobileObjectCannotBeNull = "Mobile object cannot be null!";
        public const string OsIsRequired = "Operating system is required!";
        public const string BatteryCapacityMustBePositive = "Battery capacity must be greater than zero!";
        public const string RamMustBePositive = "RAM must be greater than zero!";
        public const string StorageMustBePositive = "Storage must be greater than zero!";
    }
}