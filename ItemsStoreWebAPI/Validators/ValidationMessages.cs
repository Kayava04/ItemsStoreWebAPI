namespace ItemsStoreWebAPI.Validators
{
    public static class ValidationMessages
    {
        public const string TVObjectCannotBeNull = "TV object can not be null!";
        public const string NameIsRequired = "Name is required!";
        public const string SizeCannotBeZeroOrNegative = "Size can not be zero or negative!";
        public const string ResolutionIsRequired = "Resolution is required!";
        public const string FrequencyCannotBeZeroOrNegative = "Frequency can not be zero or negative!";
        public const string ReleasedYearMustBeCorrect = "ReleasedYear must be a correct!";
        public const string PriceCannotBeNegative = "Price can not be negative!";
        public const string InStockCannotBeNegative = "InStock can not be negative!";
    }
}