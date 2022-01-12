using Xunit;
using FluentAssertions;
using BruteForcePasswordFinder.Helpers;

namespace Tests
{
    public class HelperTests
    {
        [Fact]
        public void Test_GetAllCasePermutations()
        {
            var result = "fox".GetAllCasePermutations();

            result.Should().NotBeNullOrEmpty().And.HaveCount(8);

            result.Should().Contain("fox");
            result.Should().Contain("Fox");
            result.Should().Contain("FOx");
            result.Should().Contain("FOX");
            result.Should().Contain("fOX");
            result.Should().Contain("foX");
            result.Should().Contain("fOx");
            result.Should().Contain("FoX");
        }

        [Fact]
        public void Test_Reverse()
        {
            "123456789".Reverse().Should().Be("987654321");
        }

        [InlineData("1234567", false)]
        [InlineData("abcd1234567", true)]
        [InlineData("abcdefghijl", false)]
        [InlineData("a1245", false)]
        [Theory]
        public void Test_MatchesPasswordRequirements(string input, bool isOk)
        {
            input.MatchesPasswordRequirements().Should().Be(isOk);
        }
    }
}
