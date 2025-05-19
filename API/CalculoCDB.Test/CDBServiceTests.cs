using CalculoCDB.API.Models.CDB;
using CalculoCDB.API.Services.CDB;
using CalculoCDB.API.Settings;
using FluentValidation.Results;

namespace CalculoCDB.Test
{
    public class CDBServiceTests
    {
        private readonly CdbValidator _validator;
        private readonly ConstantRates _constantRates;
        private readonly CdbService _cdbService;

        public CDBServiceTests()
        {
            _validator = new CdbValidator();
            _constantRates = new ConstantRates(
                BankTax: 1.08M,
                CDI: 0.009M,
                IRUpto6Months: 0.0225M,
                IRUpTo12Months: 0.02M,
                IRUpTo24Months: 0.0175M,
                IRAbove24Months: 0.015M
            );

            _cdbService = new(_constantRates, _validator);
        }

        [Theory]
        [InlineData(100, 2)]
        [InlineData(500, 6)]
        [InlineData(500, 12)]
        [InlineData(500, 24)]
        [InlineData(500, 36)]
        [InlineData(1000, 1000)]
        public async Task CalculateCDB_ShouldSucceed(decimal initialValue, int months)
        {
            var result = await _cdbService.CalculateCDBAsync(initialValue, months);

            Assert.NotNull(result);
            Assert.Empty(result.Errors);
            Assert.True(result.GrossAmount > 0);
            Assert.True(result.NetAmount > 0);
            Assert.True(result.Success);
        }

        [Fact]
        public async Task CalculateCDB_ShouldFail_Overflow()
        {
            decimal initialValue = 100;
            int months = 1000000;

            var result = await _cdbService.CalculateCDBAsync(initialValue, months);

            Assert.NotNull(result);
            Assert.NotEmpty(result.Errors);
            Assert.Equal("Informe valores mais baixos.", result.Errors[0]);
            Assert.False(result.Success);
        }

        [Theory]
        [InlineData(100, 2)]
        [InlineData(500, 6)]
        [InlineData(500, 12)]
        [InlineData(500, 24)]
        [InlineData(500, 36)]
        public async Task ValidateCDBRequest_ShouldSucceed(decimal initialValue, int months)
        {
            CdbRequest request = new(initialValue, months);

            ValidationResult validationResult = await _validator.ValidateAsync(request);

            Assert.True(validationResult.IsValid);
            Assert.Empty(validationResult.Errors);
        }

        [Theory]
        [InlineData(-100, 12)] // Valor inicial inválido
        [InlineData(0, 12)] // Valor inicial inválido
        [InlineData(1000, 0)] // Número de meses inválido
        [InlineData(1000, 1)] // Número de meses inválido
        [InlineData(1000, -1)] // Número de meses inválido
        public async Task ValidateCDBRequest_ShouldFail_NonPositiveAmount(decimal initialValue, int months)
        {
            CdbRequest request = new(initialValue, months);

            ValidationResult validationResult = await _validator.ValidateAsync(request);

            Assert.False(validationResult.IsValid);
            Assert.NotEmpty(validationResult.Errors);
        }
    }
}
