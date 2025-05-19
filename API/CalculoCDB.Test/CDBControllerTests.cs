using CalculoCDB.API.Controllers;
using CalculoCDB.API.Models.CDB;
using CalculoCDB.API.Services.CDB;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CalculoCDB.Test
{
    public class CDBControllerTests
    {
        private readonly Mock<ICdbService> _mockService;
        private readonly CdbController _controller;

        public CDBControllerTests()
        {
            _mockService = new Mock<ICdbService>();

            _controller = new CdbController(_mockService.Object);
        }

        [Fact]
        public async Task CalculateCDB_ShouldReturnOK()
        {
            CdbRequest request = new(1000, 12);

            _mockService.Setup(s => s.ValidateRequestAsync(request))
                .ReturnsAsync(await new CdbValidator().ValidateAsync(request));

            _mockService.Setup(s => s.CalculateCDBAsync(request.InitialValue, request.Months))
                .ReturnsAsync(new CdbResult { Success = true });

            var requestResult = await _controller.CalculateCDBAsync(request);
            Assert.NotNull(requestResult);

            var okResult = Assert.IsType<OkObjectResult>(requestResult.Result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.NotNull(okResult.Value);

            var resultValue = Assert.IsType<CdbResult>(okResult.Value);
            Assert.True(resultValue.Success);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 1)]
        [InlineData(0.0, 6)]
        public async Task CalculateCDB_ShouldReturnUnprocessableEntity(decimal initialValue, int months)
        {
            CdbRequest request = new(initialValue, months);

            _mockService.Setup(s => s.ValidateRequestAsync(request))
                .ReturnsAsync(new CdbValidator().Validate(request));

            var requestResult = await _controller.CalculateCDBAsync(request);
            Assert.NotNull(requestResult);

            var unprocessableResult = Assert.IsType<UnprocessableEntityObjectResult>(requestResult.Result);
            Assert.Equal(422, unprocessableResult.StatusCode);

            var resultValue = Assert.IsType<CdbResult>(unprocessableResult.Value);
            Assert.False(resultValue.Success);
        }

        [Fact]
        public async Task CalculateCDB_ShouldReturnBadRequest()
        {
            CdbRequest request = new(1000, 100000);

            _mockService.Setup(s => s.ValidateRequestAsync(request))
                .ReturnsAsync(await new CdbValidator().ValidateAsync(request));

            _mockService.Setup(s => s.CalculateCDBAsync(request.InitialValue, request.Months))
                .ReturnsAsync(new CdbResult { Success = false });

            var requestResult = await _controller.CalculateCDBAsync(request);
            Assert.NotNull(requestResult);

            var unprocessableResult = Assert.IsType<BadRequestObjectResult>(requestResult.Result);
            Assert.Equal(400, unprocessableResult.StatusCode);

            var resultValue = Assert.IsType<CdbResult>(unprocessableResult.Value);
            Assert.False(resultValue.Success);
        }
    }
}
