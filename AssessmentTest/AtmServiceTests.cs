using Assessment.Implementations;
using System;
using Xunit;

namespace AssessmentTest
{
    public class AtmServiceTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(20)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(1860)]
        public void Withdraw_Success(int amount)
        {
            var atmService = new AtmService();
            Assert.True(atmService.Withdraw(amount));
        }

        [Fact]
        [InlineData(1861)]
        public void Withdraw_Failure_Insufficient(int amount)
        {
            var atmService = new AtmService();
            Assert.True(atmService.Withdraw(amount));
        }
    }
}
