using CalculoCDB.API.Models.CDB;
using CalculoCDB.API.Settings;
using FluentValidation;
using FluentValidation.Results;

namespace CalculoCDB.API.Services.CDB
{
    public class CdbService : ICdbService
    {
        private readonly ConstantRates _rates;
        private readonly IValidator<CdbRequest> _validator;

        public CdbService(ConstantRates rates, IValidator<CdbRequest> validator)
        {
            _rates = rates;
            _validator = validator;
        }

        /// <inheritdoc/>
        public async Task<CdbResult> CalculateCDBAsync(decimal initialValue, int months)
        {
            CdbResult result = new();

            try
            {
                var grossAmount = initialValue * (decimal)Math.Pow(1 + (double)(_rates.CDI * _rates.BankTax), months);

                decimal rateIR;
                if (months <= 6)
                    rateIR = _rates.IRUpto6Months;
                else if (months <= 12)
                    rateIR = _rates.IRUpTo12Months;
                else if (months <= 24)
                    rateIR = _rates.IRUpTo24Months;
                else
                    rateIR = _rates.IRAbove24Months;

                var tax = (grossAmount - initialValue) * rateIR;
                var netAmount = grossAmount - tax;

                result.Success = true;
                result.GrossAmount = Math.Round(grossAmount, 2);
                result.NetAmount = Math.Round(netAmount, 2);
                return await Task.FromResult(result);
            }
            catch (OverflowException)
            {
                result.Errors.Add("Informe valores mais baixos.");
                return await Task.FromResult(result);
            }
        }

        public async Task<ValidationResult> ValidateRequestAsync(CdbRequest request)
        {
            return await _validator.ValidateAsync(request);
        }
    }
}
