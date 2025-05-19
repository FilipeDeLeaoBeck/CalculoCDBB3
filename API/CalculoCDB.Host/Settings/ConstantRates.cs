namespace CalculoCDB.API.Settings
{
    public record ConstantRates(
        decimal BankTax,
        decimal CDI,
        decimal IRUpto6Months,
        decimal IRUpTo12Months,
        decimal IRUpTo24Months,
        decimal IRAbove24Months
    );
}
